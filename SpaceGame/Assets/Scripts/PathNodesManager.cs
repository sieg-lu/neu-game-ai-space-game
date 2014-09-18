using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PathNodesManager {
    public class PathNode : IEquatable<PathNode> {
        public int id;
        public Vector3 nodePos;
        public List<PathNode> adjPathNodeList;
        public GameObject nodeObject;
        public float distToSrc;
		public int preIndex;
        public bool Equals(PathNode other) {
            return nodePos == other.nodePos;
        }
    }

    public class PathNodeComparer : IEqualityComparer<PathNode> {
        public bool Equals(PathNode x, PathNode y) {
            return x.nodePos == y.nodePos;
        }

        public int GetHashCode(PathNode obj) {
            return obj.nodePos.GetHashCode();
        }
    }

    public List<PathNode> nodeList;
    public GameObject pathNodesObj;

    public void Initialize(GameObject baseObj) {
        pathNodesObj = baseObj;
        nodeList = new List<PathNode>();
    }

    public int AddNode(Vector3 pos) {
        int res = nodeList.Count;
        PathNode n = new PathNode();
        n.id = nodeList.Count;
        n.nodePos = pos;
        n.adjPathNodeList = new List<PathNode>();

        if (!nodeList.Contains(n)) {
            nodeList.Add(n);
        }
        return res;
    }

    public void GeneratePathNodes() {
        int cnt = 0;

        foreach (PathNode n in nodeList) {
            GameObject pathNode = new GameObject("PathNode: " + cnt++);
            pathNode.transform.parent = pathNodesObj.transform;
            pathNode.transform.position = n.nodePos;
            pathNode.AddComponent<SphereCollider>();
            pathNode.GetComponent<SphereCollider>().radius = 0.4f;
            n.nodeObject = pathNode;
//            pathNode.GetComponent<SphereCollider>().enabled = false;
//            nodes.Add(pathNode);
        }

        RaycastHit hit;
        foreach (PathNode iNode in nodeList) {
            foreach (PathNode jNode in nodeList) {
                if (iNode.nodePos == jNode.nodePos) {
                    continue;
                }
                if (Physics.Raycast(iNode.nodePos, jNode.nodePos - iNode.nodePos, out hit)) {
                    if (hit.collider.gameObject == jNode.nodeObject) {
//                        MonoBehaviour.print("HIT");
                        iNode.adjPathNodeList.Add(jNode);
                    }
                }
            }
        }

        TurnPathNodes(false);
    }

//    public void AddNeighbors(int i, int j) {
//        nodeList[i].adjIndexList.Add(j);
//        nodeList[j].adjIndexList.Add(i);
//    }

    public void TurnPathNodes(bool isOn) {
        foreach (PathNode o in nodeList) {
//            Component.Destroy(o.GetComponent<SphereCollider>());
            o.nodeObject.GetComponent<SphereCollider>().enabled = isOn;
        }
    }

    public void DebugOutput() {
        foreach (PathNode p in nodeList) {
            string tmp = p.id + ": ";
            foreach (PathNode i in p.adjPathNodeList) {
                tmp += i.id + " ";
            }
            MonoBehaviour.print(tmp);
        }
    }
}
