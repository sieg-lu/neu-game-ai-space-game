using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random = System.Random;

public class SceneManager : MonoBehaviour {

	#region Variables (private)

    System.Random mRnd = new System.Random();
    List<PcgRoom> mRoomList = new List<PcgRoom>();
    /*
     * x
     * ^
     * |
     * |
     * +----->y
     * mapTerrain[x, y]
     */
    private GridType[,] mapTerrain;
    private enum GridType {
        eFilled,
        eEmpty,
    }

    public PathNodesManager pathNodesMgr;

	#endregion
	
	#region Variables (public)

    public int mapWidth = 30;
    public int mapHeight = 30;
    public int roomCount = 10;
    public int maxTriedTimes = 400;

    public int minRoomSize = 3;
    public int maxRoomSize = 8;

//    public PrimitiveType meshType = PrimitiveType.Cube;
    public Transform wallPrefab;
    public Material floorMaterial;
    public Camera mainCamera;
    public GameObject pathNodesManager;

    public Transform myBase;
    public Transform enemyBasis;

    private Transform MyBaseObject;

    public int enemyCount;
    private List<Transform> mEnemySpriteMgrList;

    public Transform fireEffect;
    public Transform detonateEffect;

    public bool isGameOver;
    public bool isGameWin;

    public Texture CrosshairTex;
    public int CrosshairWidth;
    public int CrosshairHeight;

    public Texture GameOverTex;
    public int GameOverWidth;
    public int GameOverHeight;

    public Texture GameWinTex;
    public int GameWinWidth;
    public int GameWinHeight;

    public int spriteCountLimit;

    public Transform flagPrefab;
    public Transform flagObject;

	#endregion

	#region Unity Event Functions
	
	//// <summary>
	//// Use this for initialization
	//// </summary>
	void Start() {
        Initialize();
        PlaceRooms(maxTriedTimes);
//        SetupLights();
        GenerateMap();
        pathNodesMgr.GeneratePathNodes();
	    SetupSpriteManagers();
	    SetupCamera();
//        pathNodesMgr.DebugOutput();
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
/*        if (pathNodesMgr != null) {
            foreach (PathNodesManager.PathNode p in pathNodesMgr.nodeList) {
                foreach (PathNodesManager.PathNode i in p.adjPathNodeList) {
                    Gizmos.DrawLine(p.nodePos, i.nodePos);
                }
            }
        }*/
	}

    void OnGUI() {
        GUI.DrawTexture(
            new Rect((Screen.width - CrosshairWidth) >> 1, (Screen.height - CrosshairHeight) >> 1, CrosshairWidth,
                     CrosshairHeight), CrosshairTex);
        if (isGameOver) {
            GUI.DrawTexture(
                new Rect((Screen.width - GameOverWidth) >> 1, (Screen.height - GameOverHeight) >> 1, GameOverWidth,
                          GameOverHeight), GameOverTex);
            if (GUI.Button(new Rect(100, 100, 100, 50), "Main Menu")) {
                Application.LoadLevel("MainMenu");
            }
        } else {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.LoadLevel("MainMenu");
            }
        }
        if (isGameWin) {
            GUI.DrawTexture(
                new Rect((Screen.width - GameWinWidth) >> 1, (Screen.height - GameWinHeight) >> 1, GameWinWidth,
                          GameWinHeight), GameWinTex);
            if (GUI.Button(new Rect(100, 100, 100, 50), "Main Menu")) {
                Application.LoadLevel("MainMenu");
            }
        }
    }

	#endregion

    #region Methods

    private bool CheckGameWin() {
        for (int i = 0; i < mEnemySpriteMgrList.Count; i++) {
            if (mEnemySpriteMgrList[i] != null && !mEnemySpriteMgrList[i].GetComponent<EnemySpriteManager>().isDestroyed) {
                return false;
            }
        }
        return true;
    }

    public void GameWin() {
        if (CheckGameWin()) {
            print("check win");
            isGameWin = true;
            Screen.showCursor = true;
        }
        print("check win not right");
    }

    public void GameOver() {
        isGameOver = true;
        Screen.showCursor = true;
    }

    private void SetupCamera() {
        this.mainCamera.transform.position = new Vector3(MyBaseObject.position.x, 1.0f, MyBaseObject.position.z);
        Vector3 scale = mainCamera.transform.localScale;
        mainCamera.nearClipPlane = mainCamera.gameObject.GetComponent<CharacterController>().radius *
            Mathf.Max(scale.x, scale.z)*
            Quaternion.Dot(Quaternion.Euler(
                -mainCamera.fieldOfView,
                -2*Mathf.Rad2Deg*
                Mathf.Atan(Mathf.Tan(mainCamera.fieldOfView * .5F * Mathf.Deg2Rad) * mainCamera.aspect),
                0),
                Quaternion.identity);
    }

    private void SetupSpriteManagers() {
        if (enemyCount > mRoomList.Count - 1) {
            return;
        }
        mEnemySpriteMgrList = new List<Transform>(enemyCount);
        for (int t = 0; t < enemyCount; t++) {
            int index = mRoomList.Count - 1 - t;
            Transform mgr = (Transform)Instantiate(
                    enemyBasis, new Vector3(mRoomList[index].mCenterX, 2.5f, mRoomList[index].mCenterY), Quaternion.identity);
            mgr.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
//            mgr.name = "EnemySpriteManager: " + t;
            mgr.name = "EnemySpriteManager";
            mgr.gameObject.GetComponent<EnemySpriteManager>().sceneManager = this;
            mgr.gameObject.GetComponent<EnemySpriteManager>().mainCamera = mainCamera;
            mgr.gameObject.GetComponent<EnemySpriteManager>().roomIndex = index;
            mgr.gameObject.GetComponent<EnemySpriteManager>().roomList = mRoomList;
            mEnemySpriteMgrList.Add(mgr);
        }

        MyBaseObject = (Transform)Instantiate(
            myBase, new Vector3(mRoomList[0].mCenterX, 2.5f, mRoomList[0].mCenterY), 
            Quaternion.identity);
        MyBaseObject.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
        MyBaseObject.GetComponent<MySpriteManager>().sceneManager = this;
        MyBaseObject.GetComponent<MySpriteManager>().mainCamera = mainCamera;
        MyBaseObject.name = "MySpriteManager";
        for (int i = 0; i < mEnemySpriteMgrList.Count; i++) {
            MyBaseObject.gameObject.GetComponent<MySpriteManager>().enemySpriteMgr.Add(mEnemySpriteMgrList[i].GetComponent<EnemySpriteManager>());
            mEnemySpriteMgrList[i].GetComponent<EnemySpriteManager>().mySpriteMgr = MyBaseObject.gameObject.GetComponent<MySpriteManager>();
        }
    }

    private void Initialize() {
        mapTerrain = new GridType[mapWidth, mapHeight];
        for (int i = 0; i < mapWidth; i++) {
            for (int j = 0; j < mapHeight; j++) {
                mapTerrain[i, j] = GridType.eFilled;
            }
        }
        pathNodesMgr = new PathNodesManager();
        isGameOver = false;
        isGameWin = false;
        Screen.showCursor = false;
        flagObject = null;


    }

    private void GenerateMap() {
        GameObject walls = new GameObject("Walls");
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        plane.name = "Floor";
        plane.transform.Rotate(0, 90, 0);
        plane.transform.localScale = new Vector3(mapWidth, 0.01f, mapHeight);
        plane.transform.position = new Vector3(mapWidth / 2.0f - 0.5f, -0.5f, mapHeight / 2.0f - 0.5f);
        plane.transform.parent = walls.transform;
        plane.GetComponent<MeshRenderer>().material = floorMaterial;

        for (int i = 0; i < mapWidth; i++) {
            for (int j = 0; j < mapHeight; j++) {
                if (mapTerrain[i, j] == GridType.eFilled) {
                    Transform obj = (Transform)Instantiate(wallPrefab, new Vector3(i, 0, j), Quaternion.identity);
//                    obj.name = "Wall: (" + i + ", " + j + ")";
                    obj.name = "Wall";
                    obj.transform.parent = walls.transform;
                    
//                    GameObject obj = GameObject.CreatePrimitive(meshType);
//                    obj.transform.position = new Vector3(i, 0, j);
//                    obj.transform.localScale = new Vector3(1, 2, 1);
//                    obj.transform.parent = this.transform;
                }
            }
        }
    }

    private void SetupLights() {
        GameObject lights = new GameObject("Lights");
        lights.transform.position = mainCamera.transform.position;
//        lights.AddComponent<Light>();
//        lights.light.type = LightType.Directional;
//        lights.light.color = Color.cyan;
//        lights.transform.Rotate(30.0f, 0.0f, 0.0f);

        for (int i = 0; i < mRoomList.Count; i++) {
            GameObject light = new GameObject("Light" + i);
            light.transform.parent = lights.transform;
            light.AddComponent<Light>();
            light.light.color = Color.cyan;
            light.light.type = LightType.Point;
//            light.light.intensity = 2.0f;
            light.transform.position = new Vector3(mRoomList[i].mCenterX, 0, mRoomList[i].mCenterY);
        }

        GameObject ltn = new GameObject("FlashLight");
        ltn.transform.parent = mainCamera.transform;
        ltn.AddComponent<Light>();
        ltn.light.color = Color.gray;
        ltn.light.type = LightType.Spot;
        ltn.light.intensity = 3.0f;
        ltn.light.range = 50.0f;
        ltn.light.spotAngle = 45;
        ltn.transform.position = mainCamera.transform.position;
        ltn.transform.Rotate(90.0f, 0.0f, 0.0f);
    }

    private void CreateOneRoom(PcgRoom room) {
        for (int i = room.mX1; i <= room.mX2; i++) {
            for (int j = room.mY1; j <= room.mY2; j++) {
                mapTerrain[i, j] = GridType.eEmpty;
            }
        }
    }

    private void HoriCorridor(int x1, int x2, int y) {
        for (int i = Math.Min(x1, x2); i <= Math.Max(x1, x2); i++) {
            if (mapTerrain[i, y] == GridType.eEmpty) {
                pathNodesMgr.AddNode(new Vector3(i, 0, y));
            }
            mapTerrain[i, y] = GridType.eEmpty;
        }
    }

    private void VertCorridor(int y1, int y2, int x) {
        for (int i = Math.Min(y1, y2); i <= Math.Max(y1, y2); i++) {
            if (mapTerrain[x, i] == GridType.eEmpty) {
                pathNodesMgr.AddNode(new Vector3(x, 0, i));
            }
            mapTerrain[x, i] = GridType.eEmpty;
        }
    }

    private void PlaceRooms(int maxTryNumber) {
        mRoomList.Clear();
        int cnt = 0;
        int numbers = 0;

        pathNodesManager.transform.position = transform.position;
        pathNodesMgr.Initialize(pathNodesManager);

        while (cnt < roomCount) {
            int w = mRnd.Next(minRoomSize, maxRoomSize);
            int h = mRnd.Next(minRoomSize, maxRoomSize);
            // (mapWidth - 1) - w - 1
            // max width index
            // - 1 is for the walls
            int x = mRnd.Next(mapWidth - 2 - w) + 1;
            int y = mRnd.Next(mapHeight - 2 - h) + 1;

            PcgRoom room = new PcgRoom();
            room.CreateRoom(cnt, x, y, w, h);

            bool flag = false;
            foreach (PcgRoom r in mRoomList) {
                if (room.IsIntersected(r)) {
                    flag = true;
                    break;
                }
            }

            if (!flag) {
//                CreateOneRoom(room);

                int i = pathNodesMgr.AddNode(new Vector3(room.mCenterX, 0, room.mCenterY));

                if (mRoomList.Count != 0) {
                    PcgRoom prev = mRoomList[mRoomList.Count - 1];

                    if (mRnd.Next(2) == 1) {
                        HoriCorridor(prev.mCenterX, room.mCenterX, prev.mCenterY);
                        VertCorridor(prev.mCenterY, room.mCenterY, room.mCenterX);
                    } else {
                        VertCorridor(prev.mCenterY, room.mCenterY, room.mCenterX);
                        HoriCorridor(prev.mCenterX, room.mCenterX, prev.mCenterY);
                    }
                    int j = pathNodesMgr.AddNode(new Vector3(room.mCenterX, 0, prev.mCenterY));
//                    int former = i - 2;
//                    former = (former >= 0 ? former : 0);
//                    pathNodesMgr.AddNeighbors(i, j);
//                    pathNodesMgr.AddNeighbors(former, j);
                }
                    
                mRoomList.Add(room);
                cnt++;
            }
            numbers++;
//            print(numbers);
            if (numbers >= maxTryNumber) {
                break;
            }
        }
        for (int i = 0; i < mRoomList.Count; i++) {
            CreateOneRoom(mRoomList[i]);
        }
        roomCount = cnt;
        print("Final Rooms Count: " + roomCount + "; Tried Times: " + numbers);
    }

	#endregion
	
}
