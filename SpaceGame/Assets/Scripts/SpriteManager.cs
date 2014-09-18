using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public abstract class SpriteManager : MonoBehaviour {

	#region Variables (private)

    public List<Transform> mSpriteList = new List<Transform>();
    protected int mIdCounter = 0;

    protected bool mLeftMouseClicked;
    protected bool mRightMouseClicked;

//    protected bool mIsSpriteSelected = false;
    protected HashSet<int> mCurrentSelectedIndices;

    protected int mMoney;
    protected int healthPoints;

    public bool isDestroyed;
	
	#endregion
	
	#region Variables (public)

    public Camera mainCamera;
    public Transform fighter;
    public Transform advancedFighter;
    public Rigidbody bulletObject;
    public SceneManager sceneManager;
	
	#endregion

	#region Unity Event Functions
	
	//// <summary>
	//// Use this for initialization
	//// </summary>
	void Start() {
	    isDestroyed = false;
	}
	
	//// <summary>
	//// Update is called once per frame
	//// </summary>
	void Update() {

	}
	
	//// <summary>
	//// Debugging information should be put here
	//// </summary>
	void OnDrawGizmos() {
/*		if (mIsSelected) {
			mSpriteList[mCurrentSelectedIndex].gameObject.GetComponent<Sprite>().OnDrawGizmos();
		}*/
	}
	
	#endregion
	
	#region Methods

    protected Transform SpawnSprite(bool advFighter, Vector3 spawnPos, string name) {
        Transform f = fighter;
        if (advFighter) {
            f = advancedFighter;
        }
//        print(mIdCounter);
        Transform obj = (Transform)Instantiate(f, spawnPos, Quaternion.identity);
        obj.name = name;
//        if (advFighter) {
//            obj.name = "Fighter: Advanced " + mIdCounter++;
//        } else {
//            obj.name = "Fighter: Normal " + mIdCounter++;
//        }
        obj.parent = this.transform;
        obj.FindChild("Selected").GetComponent<MeshRenderer>().enabled = false;
//        Sprite spr = new Sprite(sceneManager);
//        spr.spriteObject = obj;
//        obj.gameObject.AddComponent<Sprite>();
		obj.gameObject.GetComponent<Sprite>().sceneManager = this.sceneManager;
        obj.gameObject.GetComponent<Sprite>().bulletObject = this.bulletObject;
        obj.gameObject.GetComponent<Sprite>().sprMgr = this;
        obj.gameObject.GetComponent<Sprite>().isPlayer = false;
        obj.gameObject.GetComponent<Sprite>().myIndex = mSpriteList.Count;
        mSpriteList.Add(obj);
//        print("Count: " + mSpriteList.Count);
        return obj;
    }

    protected void OnTestEvent(bool leftClick, bool rightClick) {
        List<PathNodesManager.PathNode> nodesList = sceneManager.pathNodesMgr.nodeList;

//        if (Input.GetMouseButton(0) && !mLeftMouseClicked) {
        if (leftClick) {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out hit);

            if (!isHit) {
                return;
            }

            if (hit.collider.gameObject.name == "Floor") {
                // Do a path-finding
//                if (mIsSpriteSelected) {
                    foreach (int i in mCurrentSelectedIndices) {
                        mSpriteList[i].gameObject.GetComponent<Sprite>().GeneratePath(hit.point);
                        mSpriteList[i].gameObject.GetComponent<Sprite>().StartMoving(hit.point, Vector3.zero);
                    }
//                }
            } else {
                bool flag = false;
                for (int i = 0; i < mSpriteList.Count; i++) {
                    if (mSpriteList[i].transform.position == hit.collider.gameObject.transform.position) {
                        mCurrentSelectedIndices.Add(i);
//                        mIsSpriteSelected = true;
                        mSpriteList[i].GetComponent<Sprite>().GetSelected();
                        flag = true;
                        break;
                    }
                }
                if (flag) {
                    for (int i = 0; i < mSpriteList.Count; i++) {
                        if (!mCurrentSelectedIndices.Contains(i)) {
                            mSpriteList[i].GetComponent<Sprite>().GetUnselected();
                        }
                    }
                }
            }
        }

//        if (Input.GetMouseButton(1) && !mRightMouseClicked) {
        if (rightClick) {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == "Floor") {
                Vector3 hitPoint = hit.point;
                float dist2 = Vector3.SqrMagnitude(nodesList[0].nodePos - hitPoint);
                int index = 0;
                for (int i = 1; i < nodesList.Count; i++) {
                    float tmp = Vector3.SqrMagnitude(nodesList[i].nodePos - hitPoint);
                    if (dist2 > tmp) {
                        dist2 = tmp;
                        index = i;
                    }
                }
                SpawnSprite(true, nodesList[index].nodePos, "Default");
                print("Spawn: " + nodesList[index].nodePos.ToString());
            }
        }
    }

    protected void OnTestMouseEvent() {
        OnTestEvent(Input.GetMouseButtonDown(0), Input.GetMouseButtonDown(1));
    }

    protected void OnTestKeyEvent() {
        OnTestEvent(Input.GetKeyDown(KeyCode.Z), Input.GetKeyDown(KeyCode.X));
    }

    public bool UnderAttack(GameObject whoAmI) {
        healthPoints -= 5;
        if (healthPoints <= 0) {
//            Destroy(this.gameObject);
            whoAmI.GetComponent<Sprite>().AddMoney(18);
            Instantiate(sceneManager.detonateEffect, this.transform.position, Quaternion.identity);
//            CleanList();
            return true;
        }
//        print(this.transform.position);
        Transform fire = (Transform)Instantiate(sceneManager.fireEffect, Vector3.zero, Quaternion.identity);
        fire.parent = this.transform;
        fire.localPosition = Vector3.zero;
//        Destroy(fire.gameObject, 2);
        return false;
    }

    protected Transform SpawnFighter(bool adv, int money, string name) {
        if (mMoney - money < 0) {
            return null;
        }
        Vector3 spawnPos = this.transform.position + Vector3.right.normalized;
        foreach (Transform spr in mSpriteList) {
            if (spr == null) {
                continue;
            }
            if (Utility.Vector3CompareXZ(spr.position, spawnPos)) {
                return null;
            }
        }
        mMoney -= money;
        return SpawnSprite(adv, spawnPos, name);
    }

    public void AddMoneyTotal(int cnt) {
        mMoney += cnt;
    }

    protected void CleanList() {
//        List<Transform> tmpList = new List<Transform>();
//        for (int i = mSpriteList.Count - 1; i >= 0; i--) {
//            if (mSpriteList[i] != null) {
//                tmpList.Add(mSpriteList[i]);
//            }
//        }
//        mSpriteList = tmpList;
    }

    public abstract void PlayerUnderAttack(GameObject whoAmI);

    #endregion

}
