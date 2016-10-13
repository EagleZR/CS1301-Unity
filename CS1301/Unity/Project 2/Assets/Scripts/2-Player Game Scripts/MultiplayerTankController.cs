using UnityEngine;
using System.Collections;

public class MultiplayerTankController : MonoBehaviour {

	public float turnSpeed;
	public float movementSpeed;
	public float projectileSpeed;
	public float fireDelay;

	private Vector3 startPosition;
	private Quaternion startRotation; 

	// Use this for initialization
	void Start () {
		startPosition = gameObject.transform.position;
		startRotation = gameObject.transform.rotation;
		// Vector3 projectileVelocity 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Move (float movementDirection) {
		Vector3 movement = new Vector3 (movementDirection, 0, 0) * Time.deltaTime * movementSpeed;
		gameObject.transform.Translate (movement);
	}

	void Turn (float turnDirection) {
		Vector3 currRotation = gameObject.transform.rotation.eulerAngles;
		currRotation += (new Vector3 (0, turnDirection, 0) * Time.deltaTime * turnSpeed);
		Quaternion rotation = Quaternion.Euler (currRotation);
		gameObject.transform.rotation = rotation;
	} 

	void Fire () {
		// TODO Fire projectile
	}

	void Kill () {
		gameObject.SetActive (false);
		// TODO set kinematic to stop all forces
		// gameObject.GetComponent (Rigidbody)
	} 

	void Spawn () {
		gameObject.transform.position = startPosition;
		gameObject.transform.rotation = startRotation;
		gameObject.SetActive (true);
		// TODO make not kinematic
	}

	// TODO provide projectile trajectory projection (lol)
	// Projectile comes out at 0 deg in X and Z
	// Projectile will not be local, but the plot will be
	// http://answers.unity3d.com/questions/606720/drawing-projectile-trajectory.html
	// TODO test the hell out of this
	void plotProjectile () {
		int vertexCount = 20;
		LineRenderer line = gameObject.GetComponent <LineRenderer>();
		line.SetVertexCount (vertexCount);
		Vector3 currPosition = new Vector3 (0f, 1.5f, .5f);
		// TODO check math
		Vector3 currVelocity = Vector3.forward * Time.deltaTime * Mathf.Cos (10) * projectileSpeed + Vector3.up * Time.deltaTime * Mathf.Sin (10) * projectileSpeed;
		Vector3 gravity = Physics.gravity;
		for (int i = 0; i < vertexCount; i++) {
			line.SetPosition (i, currPosition);
			currVelocity += gravity * Time.deltaTime;
			currPosition += currVelocity * Time.deltaTime; 
		}
	}

	// TODO determine whether or not a target is in the crosshairs or not

	// Potential TODO map projectile trajectory based on local and target tanks' velocities
}
