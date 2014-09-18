using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;
    float rotationX = 0F;

    int select = 0;
    int showInstr;
    Transform detonate;
    public Transform detonatePrefab;

    public Texture CrosshairTex;
    public int CrosshairWidth;
    public int CrosshairHeight;

    public Texture[] instructionTex;
    public int instrWidth;
    public int instrHeight;

    void OnGUI() {
        GUI.DrawTexture(
            new Rect((Screen.width - CrosshairWidth) >> 1, (Screen.height - CrosshairHeight) >> 1, CrosshairWidth,
                     CrosshairHeight), CrosshairTex);

        if (showInstr != -1) {
            GUI.DrawTexture(
                new Rect((Screen.width - instrWidth) >> 1, (Screen.height - instrHeight) >> 1, instrWidth,
                         instrHeight), instructionTex[showInstr]);
            if (Input.GetKeyDown(KeyCode.Escape)) {
                showInstr = -1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && showInstr - 1 >= 0) {
                showInstr--;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && showInstr + 1 < instructionTex.Length) {
                showInstr++;
            }
        }
    }

    void Update() {
        if (axes == RotationAxes.MouseXAndY) {
            rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
            //            rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);
            if (rotationX <= 300 && rotationX > 180) {
                rotationX = 300;
            }
            if (rotationX >= 60 && rotationX <= 180) {
                rotationX = 60;
            }

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        } else if (axes == RotationAxes.MouseX) {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        } else {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }

        if (select == 0 && showInstr == -1 && Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(0, 0, 1), out hit)) {
                if (hit.collider.gameObject.name == "Start") {
                    select = 1;
                }
                if (hit.collider.gameObject.name == "Exit") {
                    select = 2;
                }
                if (hit.collider.gameObject.name == "Instr") {
                    showInstr = 0;
                }
                detonate = (Transform)Instantiate(detonatePrefab, hit.collider.transform.position, Quaternion.identity);
                Destroy(detonate.gameObject, 5);
            }
        }

        if (select != 0 && detonate == null) {
            if (showInstr != -1) {
                select = 0;
            }
            if (select == 1) {
                Application.LoadLevel("FirstScene");
            }
            if (select == 2) {
                Application.Quit();
            }
        }
    }

    void Start() {
        select = 0;
        detonate = null;
        Screen.showCursor = false;
        showInstr = -1;
        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;
    }
}