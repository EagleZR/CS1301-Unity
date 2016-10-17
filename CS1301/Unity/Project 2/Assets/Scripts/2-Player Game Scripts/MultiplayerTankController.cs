using UnityEngine;
using System.Collections;

public class MultiplayerTankController : MonoBehaviour {

	public float turnSpeed;
	public float movementSpeed;
	public float projectileSpeed;
	public float fireDelay;
	public Rigidbody shell;
	public bool isAlive;

	public Transform shellStartLocation; 

	private float moveDirection = 0.0f; // -1.0f indicates full backwards, 1.0f indicates full forwards
	private float turnDirection = 0.0f; // -1.0f indicates full left, 1.0f indicates full right
	private float currFireDelay = 1.0f; // To count the delay between firing
	private float flippedTimerMax = 2.0f;
	private float flippedTimerCount = 0.0f;

	private Vector3 startPosition;
	private Quaternion startRotation; 
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		isAlive = true;
		startPosition = gameObject.transform.position;
		startRotation = gameObject.transform.rotation;
		rb = gameObject.GetComponent <Rigidbody> ();
		// Vector3 projectileVelocity 
	}
	
	// Update is called once per frame
	void Update () {
		FixRotation();
	}

	// When the tanks flip upside down or sideways, this turns them upright again
	void FixRotation () {
		Quaternion rotation = rb.rotation;
		if ((rotation.eulerAngles.x > 50 && rotation.eulerAngles.x < 330) || (rotation.eulerAngles.z > 50 && rotation.eulerAngles.z < 330) && !IsFalling()) {
			if (flippedTimerCount < flippedTimerMax) {
				flippedTimerCount += 1.0f * Time.deltaTime;
			} else {
				gameObject.transform.rotation = Quaternion.Euler (new Vector3 (0, rotation.y, 0));
				flippedTimerCount = 0.0f;
			}
		}
	}

	void LateUpdate() {
		currFireDelay += 1 * Time.deltaTime;
		Move ();
		Turn ();
	}

	public void setMovement (float movement) {
		moveDirection = movement;
	}

	public void setRotation (float rotation) {
		turnDirection = rotation;
	}

	/* void Move (float movementDirection) {
		Vector3 movement = new Vector3 (movementDirection, 0, 0) * Time.deltaTime * movementSpeed;
		gameObject.transform.Translate (movement);
		moveDirection = 0.0f;
	}

	void Turn (float turnDirection) {
		Vector3 currRotation = gameObject.transform.rotation.eulerAngles;
		currRotation += (new Vector3 (0, turnDirection, 0) * Time.deltaTime * turnSpeed);
		Quaternion rotation = Quaternion.Euler (currRotation);
		gameObject.transform.rotation = rotation;
		turnDirection = 0.0f;
	} */

	void Move () {
		Vector3 movement = new Vector3 (0, 0, moveDirection) * Time.deltaTime * movementSpeed;
		// rb.MovePosition (movement);
		gameObject.transform.Translate (movement);
		moveDirection = 0;
	}

	void Turn () {
		Vector3 currRotation = gameObject.transform.rotation.eulerAngles;
		currRotation += (new Vector3 (0, turnDirection, 0) * Time.deltaTime * turnSpeed);
		Quaternion rotation = Quaternion.Euler (currRotation);
		// rb.MoveRotation (rotation);
		gameObject.transform.rotation = rotation;
		turnDirection = 0.0f;
	}

	public void Fire () {
		// print (currFireDelay);
		if (currFireDelay >= fireDelay) {
			// TODO Fire projectile
			Rigidbody projectile = Instantiate(shell, shellStartLocation.position, shellStartLocation.rotation) as Rigidbody;
			projectile.velocity = shellStartLocation.forward * projectileSpeed;
			projectile.GetComponent<MultiplayerProjectileController>().firingSource = gameObject;
			// projectile.GetComponent<Collider> ().enabled = false;
			currFireDelay = 0.0f;
		} 
	}

	public void Kill () {
		isAlive = false;
		// gameObject.SetActive (false);
		// TODO set kinematic to stop all forces
		// gameObject.GetComponent (Rigidbody)
	} 

	public void Spawn () {
		gameObject.transform.position = startPosition;
		gameObject.transform.rotation = startRotation;
		gameObject.SetActive (true);
		// TODO make not kinematic
	}

	void OnTriggerEnter (Collider collider) {
		MultiplayerProjectileController script = collider.gameObject.GetComponentInParent<MultiplayerProjectileController> ();
		if (collider.gameObject.CompareTag ("Projectile") && script.isAlive) {
			if (script.firingSource != gameObject) {
				Kill ();
			}
		}
	}

	// http://answers.unity3d.com/questions/478240/detect-falling.html
	bool IsFalling () {
		return Physics.Raycast (gameObject.transform.position, Vector3.down, 1.0f);
	}

	// TODO provide projectile trajectory projection (lol)
	// Projectile comes out at 0 deg in X and Z
	// Projectile will not be local, but the plot will be
	// http://answers.unity3d.com/questions/606720/drawing-projectile-trajectory.html
	// TODO test the hell out of this
/*	public Vector3[] plotProjectile () {
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
	}*/

	// TODO determine whether or not a target is in the crosshairs or not

	// Potential TODO map projectile trajectory based on local and target tanks' velocities
}
