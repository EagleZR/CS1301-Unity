/* Author: Mark Zeagler
 * Class: CS 1301
 * Instructor: Mona Chavoshi
 * Project: Game 2
 *
 * This class handles the basic commands for the tank objects. The specific behaviors are listed below. 
 * - Tank Movement 
 * - Tank Firing 
 * - Tank Death
 * - Tank Respawn
 * - Tank Roll Reset 
 * 
 * Each of these behaviors are handled called by a separate script, depending on the type of GameObject it is attached to. 
 * This is a consolidation of those behaviors into a single script to ensure that the Players and Enemies are basically 
 * using the same kind of tanks, but with different forms of input and control. 
 */

using UnityEngine;
using System.Collections;

public class MultiplayerTankController : MonoBehaviour {

	public float turnSpeed;
	public float movementSpeed;
	public float projectileSpeed;
	public float fireDelay;

	public Rigidbody shell;
	public GameObject smoke;
	public GameObject explosion;

	public bool isAlive;

	public Transform shellStartLocation; 

	public GameObject tankMesh;

	private float moveDirection = 0.0f; // -1.0f indicates full backwards, 1.0f indicates full forwards
	private float turnDirection = 0.0f; // -1.0f indicates full left, 1.0f indicates full right
	private float currFireDelay = 1.0f; // To count the delay between firing
	private float flippedTimerMax = 2.0f;
	private float flippedTimerCount = 0.0f;

	private Vector3 startPosition;
	private Quaternion startRotation; 
	private Rigidbody rb;

	void Start () {
		isAlive = true;
		startPosition = gameObject.transform.position;
		startRotation = gameObject.transform.rotation;
		rb = gameObject.GetComponent <Rigidbody> ();
	}

	void Update () {
		FixRotation();
	}

	void LateUpdate() {
		currFireDelay += 1 * Time.deltaTime;
		Move ();
		Turn ();
	}

	/*
	 * When the tanks flip upside down or sideways, this turns them upright again
	 */
	void FixRotation () {
		Quaternion rotation = rb.rotation;
		if ((Mathf.Abs(rotation.eulerAngles.x) > 50 && Mathf.Abs(rotation.eulerAngles.x) < 310) || (Mathf.Abs(rotation.eulerAngles.z) > 50 && Mathf.Abs(rotation.eulerAngles.z) < 310) && !IsFalling ()) {
			if (flippedTimerCount < flippedTimerMax) {
				flippedTimerCount += 1.0f * Time.deltaTime;
			} else {
				gameObject.transform.rotation = Quaternion.Euler (new Vector3 (0, rotation.y, 0));
				flippedTimerCount = 0.0f;
			}
		} else {
			flippedTimerCount = 0.0f;
		}
	}

	/*
	 * Called by another script to set the movement.
	 * NOTE: Setting the movement will not immediately 
	 * move the object, but will tell this script how to
	 * move next time LateUpdate() moves the object.
	 */
	public void setMovement (float movement) {
		moveDirection = movement;
	}

	/*
	 * Called by another script to set the rotation.
	 * NOTE: Setting the rotation will not immediately 
	 * rotate the object, but will tell this script how to
	 * rotate next time LateUpdate() rotates the object.
	 */
	public void setRotation (float rotation) {
		turnDirection = rotation;
	}

	/*
	 * Moves the object
	 */
	void Move () {
		Vector3 movement = new Vector3 (0, 0, moveDirection) * Time.deltaTime * movementSpeed;
		gameObject.transform.Translate (movement);
		moveDirection = 0;
	}

	/*
	 * Turns the object
	 */
	void Turn () {
		Vector3 currRotation = gameObject.transform.rotation.eulerAngles;
		currRotation += (new Vector3 (0, turnDirection, 0) * Time.deltaTime * turnSpeed);
		Quaternion rotation = Quaternion.Euler (currRotation);
		// rb.MoveRotation (rotation);
		gameObject.transform.rotation = rotation;
		turnDirection = 0.0f;
	}

	/*
	 * Creates, then "fires" a new projectile
	 */
	public void Fire () {
		if (currFireDelay >= fireDelay) {
			Rigidbody projectile = Instantiate(shell, shellStartLocation.position, shellStartLocation.rotation) as Rigidbody;
			GameObject smoke = Instantiate (this.smoke, shellStartLocation.position, shellStartLocation.rotation) as GameObject;
			projectile.velocity = shellStartLocation.forward * projectileSpeed;
			projectile.GetComponent<MultiplayerProjectileController>().firingSource = gameObject;
			currFireDelay = 0.0f;
		} 
	}

	/*
	 * "Disappears" the object, then tells the player or enemy scripts
	 * that they're dead
	 */
	public void Kill () {
		isAlive = false;
		tankMesh.SetActive (false);
		rb.isKinematic = true;
		gameObject.GetComponent<Collider> ().enabled = false;
		GameObject explosion = Instantiate (this.explosion, transform.position, transform.rotation) as GameObject;
		#pragma warning disable 0168 // Disable the unused variable warning
		try {
			gameObject.GetComponent <MultiplayerPlayerController> ().Kill ();
		} catch (System.NullReferenceException e) {
		}
		try {
			gameObject.GetComponent <MultiplayerEnemyController> ().Kill ();
		} catch (System.NullReferenceException e) {
		}
		#pragma warning restore 0168
	} 

	/*
	 * "Reappears" the objects. This is called by the player or 
	 * enemy scripts. 
	 */
	public void Spawn () {
		gameObject.transform.position = startPosition;
		gameObject.transform.rotation = startRotation;
		tankMesh.SetActive (true);
		rb.isKinematic = false;
		gameObject.GetComponent<Collider> ().enabled = true;
	}

	/*
	 * Checks to see if the object is falling (not on the ground) or not
	 */
	// http://answers.unity3d.com/questions/478240/detect-falling.html
	bool IsFalling () {
		return Physics.Raycast (gameObject.transform.position, Vector3.down, 1.0f);
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.CompareTag ("Bottom Plane")) {
			Kill ();
		}
	} 
}
