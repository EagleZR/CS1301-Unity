using UnityEngine;
using System.Collections;

public class CameraRotator : MonoBehaviour {

	public float rotateSpeed;

	void Start () {
	
	}
	
	void FixedUpdate () {
		transform.Rotate( new Vector3( 0, this.rotateSpeed, 0 ) );
	}
}
