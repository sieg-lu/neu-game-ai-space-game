using UnityEngine;
using System.Collections;

public class MovingTowards : MonoBehaviour {

	#region Variables (private)
	
	#endregion
	
	#region Variables (public)

    public int speed = 1;
	
	#endregion

	#region Unity Event Functions
	
	//// <summary>
	//// Use this for initialization
	//// </summary>
	void Start() {

	}
	
	//// <summary>
	//// Update is called once per frame
	//// </summary>
	void Update() {
        this.transform.position += Vector3.forward * speed;
	}
	
	//// <summary>
	//// Debugging information should be put here
	//// </summary>
	void OnDrawGizmos() {
	
	}
	
	#endregion
	
	#region Methods
	
	#endregion
	
}
