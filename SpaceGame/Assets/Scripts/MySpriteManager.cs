using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class MySpriteManager : SpriteManager {
    public List<EnemySpriteManager> enemySpriteMgr;
    private bool isSelected;

    private int playerHealthPoints;

    public int shootInterval;
    private int shootIntervalCounter;

    //// <summary>
    //// Use this for initialization
    //// </summary>
    void Start() {
        GetUnselected();
        mMoney = 30;
        healthPoints = 100;
        shootInterval = 50;
        playerHealthPoints = 100;
//        this.transform.FindChild("Spotlight").GetComponent<Light>().enabled = true;
        mCurrentSelectedIndices = new HashSet<int>();
        mainCamera.GetComponent<Sprite>().sceneManager = this.sceneManager;
        mainCamera.GetComponent<Sprite>().bulletObject = this.bulletObject;
        mainCamera.GetComponent<Sprite>().sprMgr = this;
        mainCamera.GetComponent<Sprite>().myIndex = -1;
    }

    //// <summary>
    //// Update is called once per frame
    //// </summary>
    void Update() {
        DetectAndShoot();
        if (shootIntervalCounter > 0) {
            shootIntervalCounter--;
        }
    }

    void LateUpdate() {
        OnPlayerAction();
    }

    void OnGUI() {
        GUI.Label(new Rect(10, 10, 150, 30), "Normal Fighter(R): 4");
        if (Input.GetKeyDown(KeyCode.R)) {
            print("Fighter");
            Transform tmp = SpawnFighter(false, 4, "Fighter: My");
            if (tmp != null) {
//                if (!Input.GetKey(KeyCode.LeftControl)) {
//                    mCurrentSelectedIndices.Clear();
//                }
                mCurrentSelectedIndices.Add(tmp.gameObject.GetComponent<Sprite>().myIndex);
//                mIsSpriteSelected = true;
                tmp.gameObject.GetComponent<Sprite>().GetSelected();
                UnselectedOthers(mCurrentSelectedIndices);

                healthPoints += 3;
                playerHealthPoints += 5;
                if (healthPoints > 100) {
                    healthPoints = 100;
                }
                if (playerHealthPoints > 100) {
                    playerHealthPoints = 100;
                }
            }
        }
        GUI.Label(new Rect(10, 50, 150, 30), "Advanced Fighter(F): 7");
        if (Input.GetKeyDown(KeyCode.F)) {
            print("AdvFighter");
            Transform tmp = SpawnFighter(true, 7, "Fighter: My");
            if (tmp != null) {
//                if (!Input.GetKey(KeyCode.LeftControl)) {
//                    mCurrentSelectedIndices.Clear();
//                }
                mCurrentSelectedIndices.Add(tmp.gameObject.GetComponent<Sprite>().myIndex);
//                mIsSpriteSelected = true;
                tmp.gameObject.GetComponent<Sprite>().GetSelected();
                UnselectedOthers(mCurrentSelectedIndices);

                healthPoints += 5;
                playerHealthPoints += 8;
                if (healthPoints > 100) {
                    healthPoints = 100;
                }
                if (playerHealthPoints > 100) {
                    playerHealthPoints = 100;
                }
            }
        }
        GUI.Label(new Rect(10, 90, 150, 30), "Money: " + mMoney);
        GUI.Label(new Rect(10, 130, 150, 30), "Base HP: " + healthPoints);
        GUI.Label(new Rect(10, 170, 150, 30), "My HP: " + playerHealthPoints);
    }

    private void UnselectedOthers(HashSet<int> hs) {
        for (int i = 0; i < mSpriteList.Count; i++) {
            if (mSpriteList[i] == null) {
                continue;
            }
            if (!hs.Contains(i)) {
                mSpriteList[i].GetComponent<Sprite>().GetUnselected();
            }
        }
        GetUnselected();
    }

    private void DetectAndShoot() {
        RaycastHit hit;
        for (int i = 0; i < mSpriteList.Count; i++) {
            if (mSpriteList[i] == null) {
                continue;
            }
            for (int k = 0; k < enemySpriteMgr.Count; k++) {
                if (enemySpriteMgr[k] == null) {
                    continue;
                }
                float dist2 = float.MaxValue;
                int tgt = -1;
                if (Physics.Raycast(mSpriteList[i].position,
                    enemySpriteMgr[k].gameObject.transform.position - mSpriteList[i].position,
                    out hit, 5.6f) && hit.collider.gameObject == enemySpriteMgr[k].gameObject) {
                    Vector3 tmpV = new Vector3(enemySpriteMgr[k].gameObject.transform.position.x, 0,
                                               enemySpriteMgr[k].gameObject.transform.position.z);
                        dist2 =
                            Vector3.SqrMagnitude(tmpV - mSpriteList[i].position);
                        tgt = -1;
//                        mSpriteList[i].LookAt(enemySpriteMgr[k].gameObject.transform.position);
//                        mSpriteList[i].transform.Rotate(new Vector3(0, -90, 0));
//                        mSpriteList[i].gameObject.GetComponent<Sprite>().Shoot();
                }
                for (int j = 0; j < enemySpriteMgr[k].mSpriteList.Count; j++) {
                    if (enemySpriteMgr[k].mSpriteList[j] == null) {
                        continue;
                    }
                    Ray ray = new Ray(mSpriteList[i].position,
                                      enemySpriteMgr[k].mSpriteList[j].position - mSpriteList[i].position);
                    float d2 =
                        Vector3.SqrMagnitude(enemySpriteMgr[k].mSpriteList[j].position - mSpriteList[i].position);
                    if (Physics.Raycast(ray, out hit) &&
                        hit.collider.gameObject == enemySpriteMgr[k].mSpriteList[j].gameObject &&
                        dist2 > d2) {
                            dist2 = d2;
                            tgt = j;
//                            mSpriteList[i].LookAt(enemySpriteMgr[k].mSpriteList[j].position);
//                            mSpriteList[i].transform.Rotate(new Vector3(0, -90, 0));
//                            mSpriteList[i].gameObject.GetComponent<Sprite>().Shoot();
                    }
                }
                if (dist2 != float.MaxValue) {
                    if (tgt == -1) {
                        mSpriteList[i].LookAt(enemySpriteMgr[k].gameObject.transform.position);
                    } else {
                        mSpriteList[i].LookAt(enemySpriteMgr[k].mSpriteList[tgt].position);
                    }
                    mSpriteList[i].transform.Rotate(new Vector3(0, -90, 0));
                    mSpriteList[i].gameObject.GetComponent<Sprite>().Shoot();
                    break;
                }
            }
        }
    }

    public void GetSelected() {
        isSelected = true;
//        this.transform.FindChild("Spotlight").GetComponent<Light>().enabled = true;
    }

    public void GetUnselected() {
        isSelected = false;
//        this.transform.FindChild("Spotlight").GetComponent<Light>().enabled = false;
    }

    public void UnderAttack(GameObject whoAmI) {
        if (base.UnderAttack(whoAmI)) {
            Destroy(this.gameObject);
            this.sceneManager.GameOver();
        }
    }

    private void PlayerShoot(float distance2) {
        if (shootIntervalCounter > 0 || distance2 > 5.6f) {
            return;
        }
        Rigidbody obj =
            (Rigidbody)
            Instantiate(bulletObject, this.mainCamera.transform.position, this.mainCamera.transform.rotation);
        obj.transform.LookAt(this.mainCamera.transform.position);
        obj.transform.RotateAround(obj.transform.position, obj.transform.up, -90);
        obj.name = "Bullet";
        obj.GetComponent<Bullet>().whoAmI = this.mainCamera.gameObject;
        obj.GetComponent<Bullet>().fireEffect = this.sceneManager.fireEffect;
        obj.GetComponent<Bullet>().Shoot(this.mainCamera.transform.forward);
        shootIntervalCounter = shootInterval;
    }

    public override void PlayerUnderAttack(GameObject whoAmI) {
        playerHealthPoints -= 5;
        if (playerHealthPoints <= 0) {
            print("Death Effect");
            Destroy(this.gameObject);
            Instantiate(sceneManager.detonateEffect,
                        this.mainCamera.transform.position + this.mainCamera.transform.forward, Quaternion.identity);
            this.sceneManager.GameOver();
            return;
        }
        Transform fire =
            (Transform)
            Instantiate(sceneManager.fireEffect, this.mainCamera.transform.position + this.mainCamera.transform.forward,
                        Quaternion.identity);
//        fire.parent = this.mainCamera.transform;
//        fire.localPosition = Vector3.zero;
        print("Hurt Effect");
    }

    private void OnPlayerAction() {
//        print("SceneMgr: " + (sceneManager == null));
//        print("sceneManager.pathNodesMgr: " + (sceneManager.pathNodesMgr == null));
//        print("sceneManager.pathNodesMgr.nodeList: " + (sceneManager.pathNodesMgr.nodeList == null));
        List<PathNodesManager.PathNode> nodesList = sceneManager.pathNodesMgr.nodeList;

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
//            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out hit);

            if (!isHit) {
                print("OnPlayerAction: No Hit");
                return;
            }

            if (hit.collider.gameObject.name == "Floor") {
                print("OnPlayerAction: Floor");
                if (sceneManager.flagObject == null) {
                    sceneManager.flagObject =
                        (Transform) Instantiate(sceneManager.flagPrefab, hit.point, Quaternion.identity);
                }
                sceneManager.flagObject.gameObject.transform.position = hit.point;
                // Do a path-finding
//                if (mIsSpriteSelected) {
                    foreach (int i in mCurrentSelectedIndices) {
//                        if (i >= mSpriteList.Count) {
//                            print("Out of range: " + i + ", Count: " + mSpriteList.Count);
//                        }
//                        if (i < 0) {
//                            String tmp = "";
//                            foreach (int j in mCurrentSelectedIndices) {
//                                tmp += j + " ";
//                            }
//                            print(tmp);
//                        }
                        if (mSpriteList[i] != null) {
                            float x = hit.point.x + (new System.Random()).Next(3) - 1;
                            float z = hit.point.z + (new System.Random()).Next(3) - 1;
//                            print(x + ", " + z);
                            Vector3 tgt = new Vector3(x, 0, z);
                            mSpriteList[i].gameObject.GetComponent<Sprite>().GeneratePath(tgt);
                            mSpriteList[i].gameObject.GetComponent<Sprite>().StartMoving(tgt, Vector3.zero);
                        }
                    }
//                }
            } else if (hit.collider.gameObject.name == this.name) {
                print("OnPlayerAction: Base");
                print("This is me.");
                GetSelected();
//                if (mIsSpriteSelected) {
                    foreach (int i in mCurrentSelectedIndices) {
                        mSpriteList[i].GetComponent<Sprite>().GetUnselected();
//                        mIsSpriteSelected = false;
                    }
//                }
            } else {
                bool flag = false;
                int current = -1;
                for (int i = 0; i < mSpriteList.Count; i++) {
                    if (mSpriteList[i] == null) {
                        continue;
                    }
                    if (mSpriteList[i].transform.position == hit.collider.gameObject.transform.position) {
//                        mCurrentSelectedIndex = i;
                        mCurrentSelectedIndices.Add(i);
                        current = i;
//                        mIsSpriteSelected = true;
                        mSpriteList[i].GetComponent<Sprite>().GetSelected();
                        flag = true;
                        break;
                    }
                }
                if (flag) {
                    if (!Input.GetKey(KeyCode.LeftShift)) {
                        mCurrentSelectedIndices.Clear();
                        mCurrentSelectedIndices.Add(current);
                    }
                    print("OnPlayerAction: Plane");
                    for (int i = 0; i < mSpriteList.Count; i++) {
                        if (mSpriteList[i] == null) {
                            continue;
                        }
                        if (!mCurrentSelectedIndices.Contains(i)) {
                            mSpriteList[i].GetComponent<Sprite>().GetUnselected();
                        }
                    }
                    GetUnselected();
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
//            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out hit);
            if (!isHit) {
                print("OnPlayerAction: No Hit");
                return;
            }
            PlayerShoot(Vector3.Distance(hit.point, mainCamera.transform.position));
//            for (int k = 0; k < enemySpriteMgr.Count; k++) {
//                if (enemySpriteMgr[k] == null) {
//                    continue;
//                }
//                if (hit.collider.gameObject.name == "EnemySpriteManager") {
//                    print("Player Shoot Base");
//                    PlayerShoot(Vector3.Distance(hit.point, mainCamera.transform.position));
//                    break;
//                }
//                for (int j = 0; j < enemySpriteMgr[k].mSpriteList.Count; j++) {
//                    if (enemySpriteMgr[k].mSpriteList[j] == null) {
//                        continue;
//                    }
//                    if (hit.collider.gameObject == enemySpriteMgr[k].mSpriteList[j].gameObject) {
//                        print("Player Shoot Fighter");
//                        PlayerShoot(Vector3.Distance(hit.point, mainCamera.transform.position));
//                        break;
//                    }
//                }
//            }
        }
    }
}
