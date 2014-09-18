using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Sprite : MonoBehaviour {
    public Sprite(SceneManager sceneManager) {
        this.sceneManager = sceneManager;
    }

    #region Variables (private)

    private int healthPoints;
    
    private bool isStartMoving;
	private int currentTowardsIndex;
	private Vector3 targetPosition;
	private Vector3 finalLookAtPosition;
	private float currentSpeed;
	private Quaternion rotation;
	private float increment;

    public bool isPlayer;

    public int shootInterval;
    private int shootIntervalCounter;

    class PathPoint : IComparer<PathPoint> {
        public int index;
        public float distToTgt;

        public int Compare(PathPoint x, PathPoint y) {
            if (Math.Abs(x.distToTgt - y.distToTgt) < 1e-4) {
                return 0;
            } 
            if (x.distToTgt < y.distToTgt) {
                return -1;
            }
            return 1;
        }
    }
	
	#endregion

    #region Variables (public)

    public List<PathNodesManager.PathNode> mPath;
    public SceneManager sceneManager;

    public Rigidbody bulletObject;
    public SpriteManager sprMgr;
//    public Transform spriteObject;

    public int myIndex;
	
	#endregion

	#region Unity Event Functions
	
	//// <summary>
	//// Use this for initialization
	//// </summary>
	void Start() {
		isStartMoving = false;
		currentSpeed = 5.0f;
		currentTowardsIndex = 0;
		increment = 0.0f;
	    healthPoints = 100;
        shootInterval = 50;
	}
	
	//// <summary>
	//// Update is called once per frame
	//// </summary>
    void Update() {
        if (isPlayer) {
            return;
        }
		if (mPath != null) {
			if (Utility.Vector3CompareXZ(this.transform.position, targetPosition)) {
//				Quaternion rot = Quaternion.LookRotation(finalLookAtPosition - this.transform.position);
//				this.transform.rotation = rot;
				this.transform.LookAt(mPath[0].nodePos);
				this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
				isStartMoving = false;
				mPath = null;
				return;
			}
//			if (this.transform.position == mPath[currentTowardsIndex].nodePos) {
			if (Utility.Vector3CompareXZ(this.transform.position, mPath[currentTowardsIndex].nodePos)) {
				this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
				currentTowardsIndex++;
				if (currentTowardsIndex >= mPath.Count) {
					isStartMoving = false;
					mPath = null;
					return;
				}
			}
			this.transform.LookAt(mPath[currentTowardsIndex].nodePos);
			this.transform.Rotate(new Vector3(0, -90, 0));
			float step = currentSpeed * Time.deltaTime;
//			rotation = Quaternion.LookRotation(mPath[currentTowardsIndex].nodePos - transform.position);
//			this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, step);
			this.transform.position = Vector3.MoveTowards(transform.position, mPath[currentTowardsIndex].nodePos, step);
        }

        if (shootIntervalCounter > 0) {
            shootIntervalCounter--;
        }
//        if (Input.GetKeyDown(KeyCode.Space)) {
//            Shoot();
//        }
	}
	
	void LateUpdate() {
        if (isPlayer) {
            return;
        }
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
//		print("LateUpdate");
	}

//    void OnCollisionExit(Collision collision) {
//        if (collision.gameObject.name.StartsWith("Fighter: ")) {
//            this.rigidbody.Sleep();
//        }
//    }
	
	//// <summary>
	//// Debugging information should be put here
	//// </summary>
	public void OnDrawGizmos() {
		if (mPath != null && mPath.Count > 1) {
			for (int i = 1; i < mPath.Count; i++) {
				Gizmos.DrawLine(mPath[i].nodePos, mPath[i - 1].nodePos);
			}
		}
	}
	
	#endregion
	
	#region Methods
	
	public void StartMoving(Vector3 target, Vector3 finalLookAt) {
		currentTowardsIndex = 0;
		isStartMoving = true;
		targetPosition = target;
		finalLookAtPosition = finalLookAt;
	}
	
	private PathNodesManager.PathNode FindNearestPathNode(Vector3 current) {
		List<PathNodesManager.PathNode> nodesList = sceneManager.pathNodesMgr.nodeList;
		float dist2 = 1000000.0f;
		int index = -1;
		sceneManager.pathNodesMgr.TurnPathNodes(true);
		for (int i = 0; i < nodesList.Count; i++) {
			if (current == nodesList[i].nodePos) {
				index = i;
				break;
			}
//			float tmp = Vector3.SqrMagnitude(nodesList[i].nodePos - current);
			float tmp = Vector3.Distance(nodesList[i].nodePos, current);
			RaycastHit hit;
			if (dist2 > tmp &&
				Physics.Raycast(current, nodesList[i].nodePos - current, out hit) &&
				hit.collider.gameObject == nodesList[i].nodeObject) {
				/*Physics.Raycast(current, nodesList[i].nodePos - current, out hit) &&
				hit.collider.gameObject == nodesList[i].nodeObject &&*/
//				print("Hit: " + hit.collider.gameObject.name);
//				print("Obj: " + nodesList[i].nodeObject.name);
				dist2 = tmp;
				index = i;
			}
		}
		sceneManager.pathNodesMgr.TurnPathNodes(false);
		return (index == -1 ? null : nodesList[index]);
	}

    public void GeneratePath(Vector3 target) {
		float tmpDist = Vector3.Distance(target, this.transform.position) - 1.0f;
        if (tmpDist <= 0.0f) {
            return;
        }
		if (!Physics.Raycast(this.transform.position, target - this.transform.position, tmpDist)) {
			mPath = new List<PathNodesManager.PathNode>();
			PathNodesManager.PathNode tmp = new PathNodesManager.PathNode();
			tmp.nodePos = target;
			mPath.Add(tmp);
			return;
		}
		
        PathNodesManager.PathNode firstPos = FindNearestPathNode(this.transform.position);
        PathNodesManager.PathNode targetPos = FindNearestPathNode(target);
		if (firstPos == null || targetPos == null) {
//			print("Holyshit!");
			return;
		}
        List<PathNodesManager.PathNode> nodes = sceneManager.pathNodesMgr.nodeList;
//        print(firstPos.id + "; " + targetPos.id);
        bool[] isVisited = new bool[nodes.Count];
        for (int i = 0; i < isVisited.Length; i++) {
            isVisited[i] = false;
        }
		
        foreach (PathNodesManager.PathNode p in nodes) {
            p.distToSrc = 1000000;
			p.preIndex = -1;
        }
		
        for (int i = 0; i < isVisited.Length; i++) {
            isVisited[i] = false;
        }
        PriorityQueue<PathPoint> myQueue = new PriorityQueue<PathPoint>(new PathPoint());
        PathPoint start = new PathPoint();
        start.index = firstPos.id;
        start.distToTgt = Vector3.SqrMagnitude(target - nodes[start.index].nodePos);
		firstPos.distToSrc = 0;
		firstPos.preIndex = -1;
        myQueue.Push(start);

        while (myQueue.Count != 0) {
            PathPoint tmp = myQueue.Pop();

            if (nodes[tmp.index].nodePos == target) {
				continue;
//                break;
            }
//            isVisited[tmp.index] = true;
            List<PathNodesManager.PathNode> adj = nodes[tmp.index].adjPathNodeList;
            for (int i = 0; i < adj.Count; i++) {
                PathNodesManager.PathNode toNode = adj[i];
				float dist = 0;
				if (nodes[tmp.index].preIndex != -1) {
					dist = Vector3.Distance(nodes[tmp.index].nodePos, nodes[nodes[tmp.index].preIndex].nodePos);
				}
                if (nodes[tmp.index].preIndex == -1 || toNode.distToSrc > nodes[tmp.index].distToSrc + dist) {
                    toNode.distToSrc = nodes[tmp.index].distToSrc + dist;
                    PathPoint toPoint = new PathPoint();
                    toPoint.index = toNode.id;
                    toNode.preIndex = tmp.index;
                    toPoint.distToTgt = Vector3.SqrMagnitude(target - nodes[toPoint.index].nodePos);

                    myQueue.Push(toPoint);
                }
            }
        }
		
		mPath = new List<PathNodesManager.PathNode>();
		mPath.Clear();
		PathNodesManager.PathNode tmp1 = targetPos;
		while (tmp1.preIndex != -1) {
			mPath.Add(tmp1);
			tmp1 = nodes[tmp1.preIndex];
		}
		
		mPath.Add(firstPos);
		mPath.Reverse();
		PathNodesManager.PathNode finalPos = new PathNodesManager.PathNode();
		finalPos.nodePos = target;
		mPath.Add(finalPos);
		
		string res = "";
		foreach (PathNodesManager.PathNode p in mPath) {
			res += p.id + " ";
		}
//		print(res);
    }

    public void Shoot() {
        if (shootIntervalCounter > 0) {
            return;
        }
        Rigidbody obj = (Rigidbody)Instantiate(bulletObject, this.transform.position, this.transform.rotation);
        obj.name = "Bullet";
//        obj.transform.parent = this.transform;
        obj.GetComponent<Bullet>().whoAmI = this.gameObject;
        obj.GetComponent<Bullet>().fireEffect = this.sceneManager.fireEffect;
        obj.GetComponent<Bullet>().Shoot(this.transform.right);
        shootIntervalCounter = shootInterval;
    }

    public void UnderAttack(GameObject whoAmI) {
        if (this.gameObject.name.StartsWith("Fighter: Advanced ")) {
            healthPoints -= 15;
        } else {
            healthPoints -= 10;
        }
        if (healthPoints <= 0) {
            Destroy(this.gameObject);
            if (whoAmI != null) {
                whoAmI.GetComponent<Sprite>().AddMoney(12);
            }
            Instantiate(sceneManager.detonateEffect, this.transform.position, Quaternion.identity);
//            Destroy(detonate.gameObject, 4);
            return;
        }
        Transform fire = (Transform)Instantiate(sceneManager.fireEffect, Vector3.zero, Quaternion.identity);
        fire.parent = this.transform;
        fire.localPosition = Vector3.zero;
//        Destroy(fire.gameObject, 2);
    }

    public void AddMoney(int cnt) {
        sprMgr.AddMoneyTotal(cnt);
    }

    public void GetSelected() {
        this.transform.FindChild("Selected").GetComponent<MeshRenderer>().enabled = true;
    }

    public void GetUnselected() {
        this.transform.FindChild("Selected").GetComponent<MeshRenderer>().enabled = false;
    }
	
	#endregion
	
}
