using UnityEngine;
using System.Collections;

public class RtsCamera : MonoBehaviour {

    #region Variables (private)

    private int closeMost = 10;
    private int farMost;

    private int leftMost = 0;
    private int rightMost;
    private int downMost = 0;
    private int upMost;

	#endregion

    #region Variables (public)

    public int scrollDistance = 5;
    public float scrollSpeed = 35;

    public int cameraHeight = 30;

    public GameObject sceneManager;
	
	#endregion

	#region Unity Event Functions
	
	//// <summary>
	//// Use this for initialization
	//// </summary>
	public void Start() {
	    int mapWidth = sceneManager.GetComponent<SceneManager>().mapWidth;
        int mapHeight = sceneManager.GetComponent<SceneManager>().mapHeight;
	    farMost = (mapWidth >> 1);
	    rightMost = mapWidth;
	    upMost = mapHeight;
	    transform.position = new Vector3((mapWidth - 1)/2.0f, cameraHeight, (mapHeight - 1)/2.0f);
	    transform.Rotate(90, 0, 0);
	}

    //// <summary>
	//// Update is called once per frame
	//// </summary>
	void Update() {
	    UpdateCameraPosition();
	}
	
	//// <summary>
	//// Debugging information should be put here
	//// </summary>
	void OnDrawGizmos() {
	
	}
	
	#endregion
	
	#region Methods

    private void UpdateCameraPosition() {
        float mousePosX = Input.mousePosition.x;
        float mousePosY = Input.mousePosition.y;

        if (mousePosX < scrollDistance && transform.position.x >= leftMost) {
            transform.Translate(Vector3.right * (-scrollSpeed) * Time.deltaTime);
        } else if (mousePosX >= Screen.width - scrollDistance && transform.position.x <= rightMost) {
            transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
        }
        if (mousePosY < scrollDistance && transform.position.z >= downMost) {
            transform.Translate(Vector3.up * (-scrollSpeed) * Time.deltaTime);
        } else if (mousePosY >= Screen.height - scrollDistance && transform.position.z <= upMost) {
            transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }

        float midScroll = Input.GetAxis("Mouse ScrollWheel");
        if (midScroll > 0 && transform.position.y >= closeMost) {
            transform.Translate(Vector3.forward * scrollSpeed * 2.0f * Time.deltaTime);
        } else if (midScroll < 0 && transform.position.y <= farMost) {
            transform.Translate(Vector3.forward * (-scrollSpeed) * 2.0f * Time.deltaTime);
        }
    }
	
	#endregion
	
}
