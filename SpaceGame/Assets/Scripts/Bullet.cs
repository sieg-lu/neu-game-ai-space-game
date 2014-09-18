using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	#region Variables (private)

    private bool hasAlreadyHit;
	
	#endregion
	
	#region Variables (public)

    public float speed;

    public Vector3 direction;
    public GameObject whoAmI;

    public Transform fireEffect;
	
	#endregion

	#region Unity Event Functions
	
	//// <summary>
	//// Use this for initialization
	//// </summary>
	void Start() {
        hasAlreadyHit = false;
	    speed = 20.0f;
        Destroy(this.gameObject, 2.0f);
	}
	
	//// <summary>
	//// Update is called once per frame
	//// </summary>
	void Update() {
	    float step = speed * Time.deltaTime;
	    this.transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, step);
	}
	
	//// <summary>
	//// Debugging information should be put here
	//// </summary>
	void OnDrawGizmos() {
	
	}

    void HitSomething() {
        Transform fire = (Transform)Instantiate(fireEffect, this.transform.position, Quaternion.identity);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject == whoAmI) {
            return;
        }
        // TODO: fire effects
        if (collision.gameObject.name.StartsWith("Wall")) {
            // the hit thing is wall
            HitSomething();
        } else if (collision.gameObject.name.StartsWith("Floor")) {
            HitSomething();
        } else if (collision.gameObject.name.StartsWith("Fighter")) {
            // hit the plane
//            print("Hit the Fighter");
            collision.gameObject.GetComponent<Sprite>().UnderAttack(whoAmI);
        } else if (collision.gameObject.name.StartsWith("EnemySpriteManager")) {
//            print("Hit the Enemy Sprite Mgr");
            collision.gameObject.GetComponent<EnemySpriteManager>().UnderAttack(whoAmI);
//            if (whoAmI != null) {
//                whoAmI.GetComponent<Sprite>().AddMoney(5);
//            }
        } else if (collision.gameObject.name == "MySpriteManager") {
            collision.gameObject.GetComponent<MySpriteManager>().UnderAttack(whoAmI);
//            if (whoAmI != null) {
//                whoAmI.GetComponent<Sprite>().AddMoney(5);
//            }
        } else if (collision.gameObject.name == "MainCamera") {
            collision.gameObject.GetComponent<Sprite>().sprMgr.PlayerUnderAttack(whoAmI);
//            print("I'm hurt");
        } else {
            // hit the others
        }
//        print("Destroy");
        Destroy(this.gameObject);
    }
	
	#endregion
	
	#region Methods

    public void Shoot(Vector3 dire) {
        direction = dire.normalized;
    }
	
	#endregion
	
}
