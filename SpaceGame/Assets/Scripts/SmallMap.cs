using UnityEngine;
using System.Collections;

public class SmallMap : MonoBehaviour {

	#region Variables (private)

    private Vector2 mapCenter;

	#endregion
	
	#region Variables (public)

    public Texture playerTex;
    public Texture myFighterTex;
    public Texture enemyFighterTex;
    public Texture wallTex;
    public Texture backgroundTex;

    public Transform centerObject;
    public float mapScale = 4.0f;

    public int mapSize = 256;
	
	#endregion

	#region Unity Event Functions
	
	//// <summary>
	//// Use this for initialization
	//// </summary>
	void Start() {
        mapCenter = new Vector2(Screen.width - 150, Screen.height - 150);
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
	
	}

    void OnGUI() {
        var bX = centerObject.transform.position.x * mapScale;
        var bY = centerObject.transform.position.z * mapScale;

        GUI.DrawTexture(
            new Rect(mapCenter.x - (mapSize >> 1) + 7.0f, mapCenter.y - (mapSize >> 1) + 7.0f, mapSize, mapSize),
            backgroundTex);
//        GUI.DrawTexture(
//            new Rect(mapCenter.x - (mapSize >> 1) + 7.0f, mapCenter.y - (mapSize >> 1) + 7.0f, 1, 1),
//            playerTex);

//        RenderGameObjects("Wall", wallTex, 10);
//        RenderGameObjects("Fighter: My", myFighterTex, 5);
//        RenderObject(GameObject.Find("MySpriteManager"), myFighterTex, 10);
//        RenderGameObjects("Fighter: Enemy", enemyFighterTex, 5);
//        RenderObject(GameObject.Find("EnemySpriteManager"), enemyFighterTex, 10);

        foreach(GameObject obj in FindObjectsOfType(typeof(GameObject))) {
            if (obj.name == "Fighter: My") {
                RenderObject(obj, myFighterTex, 10);
            } else if (obj.name == "Fighter: Enemy") {
                RenderObject(obj, enemyFighterTex, 10);
            }
        }
        GUI.DrawTexture(new Rect(mapCenter.x - 3.5f, mapCenter.y - 3.5f, 20, 20), playerTex);
    }
	
	#endregion
	
	#region Methods

    void RenderObject(GameObject obj, Texture tex, float texSize) {
        Vector3 centerPos = centerObject.position;
        Vector3 extPos = obj.transform.position;

//        var dist = Vector3.Distance(centerPos, extPos);
        var dist = Mathf.Sqrt((centerPos.x - extPos.x)*(centerPos.x - extPos.x) + (centerPos.z - extPos.z)*(centerPos.z - extPos.z));

        var dx = centerPos.x - extPos.x;
        var dz = centerPos.z - extPos.z;

        var deltay = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg - 270 - centerObject.eulerAngles.y;

        var bX = dist * Mathf.Cos(deltay * Mathf.Deg2Rad);
        var bY = dist * Mathf.Sin(deltay * Mathf.Deg2Rad);

        bX = bX * mapScale;
        bY = bY * mapScale;

        if (dist <= 15) {
            GUI.DrawTexture(new Rect(mapCenter.x + bX, mapCenter.y + bY, texSize, texSize), tex);
        }
    }

    void RenderGameObjects(string name, Texture tex, float texSize) {
        foreach(GameObject obj in FindObjectsOfType(typeof(GameObject))) {
            if (obj.name == name) {
                RenderObject(obj, tex, texSize);
            }
        }
    }
	
	#endregion
	
}
