using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/*
 * Idle Command: 
 *      produces one plane
 *      asking it to move around randomly
 * Defend Command: 
 *      produces one plane
 *      making it around the base and defend
 * Attach Command:
 *      produces one plane
 *      attacking the player ASAP
 */

class EnemySpriteManager : SpriteManager {
    public MySpriteManager mySpriteMgr;

    public enum eCommand {
        eIdle = 0,
        eDefend,
        eAttack,
    }

    private Queue<eCommand> commandQueue;
    private bool isInAction;

    public List<PcgRoom> roomList;
    public int roomIndex;

    //// <summary>
    //// Use this for initialization
    //// </summary>
    void Start() {
        mMoney = 20;
        healthPoints = 100;
        commandQueue = new Queue<eCommand>();
        isInAction = false;

        commandQueue.Enqueue(eCommand.eDefend);
//        commandQueue.Enqueue(eCommand.eIdle);
        commandQueue.Enqueue(eCommand.eAttack);
    }

    //// <summary>
    //// Update is called once per frame
    //// </summary>
    void Update() {
        DetectAndShoot();
        ExecuteCommand();
    }

    void LateUpdate() {
        OnTestKeyEvent();
    }

    private void ExecuteCommand() {
        if (commandQueue.Count == 0) {
            return;
        }
        if (mSpriteList.Count <= sceneManager.spriteCountLimit) {
            bool adv = ((new System.Random()).Next(5) == 0 ? true : false);
            Transform newFighter = SpawnFighter(adv, (adv ? 7 : 5), "Fighter: Enemy");
            if (newFighter != null) {
                isInAction = false;
                int x = (new System.Random()).Next(roomList[roomIndex].mX1, roomList[roomIndex].mX2);
                int y = (new System.Random()).Next(roomList[roomIndex].mY1, roomList[roomIndex].mY2);
                Vector3 tgt = new Vector3(x, 0, y);

                if (Utility.Vector3CompareXZ(tgt, new Vector3(roomList[roomIndex].mCenterX, 0, roomList[roomIndex].mCenterY))) {
                    int index = (new System.Random()).Next(roomList.Count - 1) + 1;
                    tgt = new Vector3(roomList[index].mCenterX, 0, roomList[index].mCenterY);
                }

                newFighter.gameObject.GetComponent<Sprite>().GeneratePath(tgt);
                newFighter.gameObject.GetComponent<Sprite>().StartMoving(tgt, Vector3.zero);

                healthPoints += 3;
                if (healthPoints > 100) {
                    healthPoints = 100;
                }
            }
        }
//        print("Money: " + mMoney);
        eCommand currentCmd = commandQueue.Peek();
        switch (currentCmd) {
            case eCommand.eIdle: {
//                    if (mSpriteList.Count < 1) {
//                        if (tmp != null) {
//                            int index = 1 + (new System.Random()).Next(roomList.Count - 1);
//                            Vector3 tgt = new Vector3(roomList[index].mCenterX, 0, roomList[index].mCenterY);
//                            tmp.gameObject.GetComponent<Sprite>().GeneratePath(tgt);
//                            tmp.gameObject.GetComponent<Sprite>().StartMoving(tgt, Vector3.zero);
//                        }
//                    }
                for (int i = mSpriteList.Count - 2; i > 2; i--) {
                    if (mSpriteList[i].gameObject.GetComponent<Sprite>().mPath == null) {
                        if (mSpriteList[i] == null) {
                            continue;
                        }
                        int index = (new System.Random()).Next(roomList.Count - 1) + 1;
                        Vector3 tgt = new Vector3(roomList[index].mCenterX, 0, roomList[index].mCenterY);
//                            int x = (new System.Random()).Next(roomList[index].mX1, roomList[index].mX2);
//                            int y = (new System.Random()).Next(roomList[index].mY1, roomList[index].mY2);
//                            Vector3 tgt = new Vector3(x, 0, y);
                        mSpriteList[i].gameObject.GetComponent<Sprite>().GeneratePath(tgt);
                        mSpriteList[i].gameObject.GetComponent<Sprite>().StartMoving(tgt, Vector3.zero);
                    }
                }
            } break;
            case eCommand.eAttack: {
//                if (!isInAction) {
                    for (int i = mSpriteList.Count - 2; i > 2; i--) {
                        if (mSpriteList[i] == null) {
                            continue;
                        }
                        if (mSpriteList[i].gameObject.GetComponent<Sprite>().mPath == null) {
                            int x = (new System.Random()).Next(roomList[0].mX1, roomList[0].mX2);
                            int y = (new System.Random()).Next(roomList[0].mY1, roomList[0].mY2);
                            Vector3 tgt = new Vector3(x, 0, y);

                            mSpriteList[i].gameObject.GetComponent<Sprite>().GeneratePath(tgt);
                            mSpriteList[i].gameObject.GetComponent<Sprite>().StartMoving(tgt, Vector3.zero);
                        }
                    }
                    isInAction = true;
//                }
            } break;
            case eCommand.eDefend: {
//                if (!isInAction) {
                    for (int i = mSpriteList.Count - 1; i > mSpriteList.Count / 2; i--) {
                        if (mSpriteList[i] == null) {
                            continue;
                        }
                        if (mSpriteList[i].gameObject.GetComponent<Sprite>().mPath == null) {
                            int x = (new System.Random()).Next(roomList[roomIndex].mX1, roomList[roomIndex].mX2);
                            int y = (new System.Random()).Next(roomList[roomIndex].mY1, roomList[roomIndex].mY2);
                            Vector3 tgt = new Vector3(x, 0, y);

                            mSpriteList[i].gameObject.GetComponent<Sprite>().GeneratePath(tgt);
                            mSpriteList[i].gameObject.GetComponent<Sprite>().StartMoving(tgt, Vector3.zero);
                        }
                    }
                    isInAction = true;
//                }
            } break;
        }
    }

    private void FinalizeCommand() {
//        CleanList();
        isInAction = false;
        if (commandQueue.Count <= 1) {
            return;
        }
        commandQueue.Dequeue();
//        print("Pop Queue");
    }

    private void DetectAndShoot() {
        if (mySpriteMgr == null) {
            return;
        }
        RaycastHit hit;
        for (int i = 0; i < mSpriteList.Count; i++) {
            if (mSpriteList[i] == null) {
                continue;
            }

            bool flag = false;
            for (int j = 0; j < mySpriteMgr.mSpriteList.Count; j++) {
                if (mySpriteMgr.mSpriteList[j] == null) {
                    continue;
                }
                Ray ray = new Ray(mSpriteList[i].position,
                                  mySpriteMgr.mSpriteList[j].position - mSpriteList[i].position);
                if (Physics.Raycast(ray, out hit) &&
                    hit.collider.gameObject == mySpriteMgr.mSpriteList[j].gameObject) {
                        mSpriteList[i].LookAt(mySpriteMgr.mSpriteList[j].position);
                        mSpriteList[i].transform.Rotate(new Vector3(0, -90, 0));
                        mSpriteList[i].gameObject.GetComponent<Sprite>().Shoot();
                    flag = true;
                    break;
                }
            }
            if (!flag && Physics.Raycast(mSpriteList[i].position,
                         mySpriteMgr.gameObject.transform.position - mSpriteList[i].position,
                         out hit, 5.6f) && hit.collider.gameObject == mySpriteMgr.gameObject) {
                    mSpriteList[i].LookAt(mySpriteMgr.gameObject.transform.position);
                    mSpriteList[i].transform.Rotate(new Vector3(0, -90, 0));
                    mSpriteList[i].gameObject.GetComponent<Sprite>().Shoot();
                flag = true;
            }
            if (!flag && Physics.Raycast(mSpriteList[i].position,
                         mySpriteMgr.mainCamera.gameObject.transform.position - mSpriteList[i].position,
                         out hit, 5.6f) && hit.collider.gameObject == mySpriteMgr.mainCamera.gameObject) {
                mSpriteList[i].LookAt(mySpriteMgr.mainCamera.gameObject.transform.position);
                mSpriteList[i].eulerAngles = new Vector3(mSpriteList[i].eulerAngles.x, mSpriteList[i].eulerAngles.y, 0);
                mSpriteList[i].transform.Rotate(new Vector3(0, -90, 0));
                mSpriteList[i].gameObject.GetComponent<Sprite>().Shoot();
                flag = true;
            }
        }
    }

    public void UnderAttack(GameObject whoAmI) {
        if (base.UnderAttack(whoAmI)) {
            Destroy(this.gameObject);
            this.isDestroyed = true;
            sceneManager.GameWin();
        }
        FinalizeCommand();
    }

    public override void PlayerUnderAttack(GameObject whoAmI) {
        throw new NotImplementedException();
    }
}
