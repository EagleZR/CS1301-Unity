using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject target;
	private float distanceToTarget;

	void Start () {
		this.distanceToTarget = transform.position.x - this.target.transform.position.x;
	}
	
	void Update () {
		float targetObjectX = this.target.transform.position.x;

		Vector3 newCameraPosition = transform.position;
		newCameraPosition.x = targetObjectX + distanceToTarget;
		transform.position = newCameraPosition;
	}
}
