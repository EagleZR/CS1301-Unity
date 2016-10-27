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

	public bool isAlive;
	public bool movementHandledOutside = false;
	public bool isMoving = false;

	public Transform shellStartLocation; 

	public GameObject tankMesh;

	public AudioSource tankAudio;
	public AudioClip engineDriving;
	public AudioClip engineIdling;

	private float moveDirection = 0.0f; // -1.0f indicates full backwards, 1.0f indicates full forwards
	private float turnDirection = 0.0f; // -1.0f indicates full left, 1.0f indicates full right
	private float currFireDelay = 1.0f; // To count the delay between firing
	private float flippedTimerMax = 2.0f;
	private float flippedTimerCount = 0.0f;

	private Vector3 startPosition;
	private Quaternion startRotation; 
	private Rigidbody rb;

	void Start () {
		this.tankAudio.clip = this.engineIdling;
		this.tankAudio.pitch = Random.Range (this.tankAudio.pitch - 2.0f, this.tankAudio.pitch + 2.0f);
		this.tankAudio.Play ();
		this.isAlive = true;
		this.startPosition = gameObject.transform.position;
		this.startRotation = gameObject.transform.rotation;
		this.rb = gameObject.GetComponent <Rigidbody> ();
	}

	void Update () {
		FixRotation();
	}

	void LateUpdate() {
		this.currFireDelay += 1 * Time.deltaTime;
		Move ();
		Turn ();
		Audio ();
	}

	/*
	 * When the tanks flip upside down or sideways, this turns them upright again
	 */
	void FixRotation () {
		Quaternion rotation = this.rb.rotation;
		if ((Mathf.Abs(rotation.eulerAngles.x) > 50 && Mathf.Abs(rotation.eulerAngles.x) < 310) || (Mathf.Abs(rotation.eulerAngles.z) > 50 && Mathf.Abs(rotation.eulerAngles.z) < 310) && !IsFalling ()) {
			if (this.flippedTimerCount < this.flippedTimerMax) {
				this.flippedTimerCount += 1.0f * Time.deltaTime;
			} else {
				gameObject.transform.rotation = Quaternion.Euler (new Vector3 (0, rotation.y, 0));
				this.flippedTimerCount = 0.0f;
			}
		} else {
			this.flippedTimerCount = 0.0f;
		}
	}

	/*
	 * Called by another script to set the movement.
	 * NOTE: Setting the movement will not immediately 
	 * move the object, but will tell this script how to
	 * move next time LateUpdate() moves the object.
	 */
	public void setMovement (float movement) {
		this.moveDirection = movement;
	}

	/*
	 * Called by another script to set the rotation.
	 * NOTE: Setting the rotation will not immediately 
	 * rotate the object, but will tell this script how to
	 * rotate next time LateUpdate() rotates the object.
	 */
	public void setRotation (float rotation) {
		this.turnDirection = rotation;
	}

	/*
	 * Moves the object
	 */
	void Move () {
		if (Mathf.Abs (this.moveDirection) == 0 && !this.movementHandledOutside) {
			this.isMoving = false;
		} else if (!this.movementHandledOutside) {
			this.isMoving = true;
		}
		Vector3 movement = new Vector3 (0, 0, this.moveDirection) * Time.deltaTime * this.movementSpeed;
		gameObject.transform.Translate (movement);
		this.moveDirection = 0;
	}

	/*
	 * Turns the object
	 */
	void Turn () {
		if (Mathf.Abs (this.turnDirection) == 0 && !this.movementHandledOutside) {
			this.isMoving = false;
		} else if (!this.movementHandledOutside) {
			this.isMoving = true;
		}
		Vector3 currRotation = gameObject.transform.rotation.eulerAngles;
		currRotation += (new Vector3 (0, this.turnDirection, 0) * Time.deltaTime * this.turnSpeed);
		Quaternion rotation = Quaternion.Euler (currRotation);
		gameObject.transform.rotation = rotation;
		this.turnDirection = 0.0f;
	}

	/*
	 * Controls the engine's audio output based on movement.
	 */
	void Audio () {
		if (!this.isMoving) {
			if (this.tankAudio.clip == this.engineDriving) {
				this.tankAudio.clip = this.engineIdling;
				this.tankAudio.Play ();
			}
		} else {
			if (this.tankAudio.clip == this.engineIdling) {
				this.tankAudio.clip = this.engineDriving;
				this.tankAudio.Play ();
			}
		}
	}

	/*
	 * Creates, then "fires" a new projectile
	 */
	public void Fire () {
		if (this.currFireDelay >= this.fireDelay) {
			Rigidbody projectile = Instantiate(this.shell, this.shellStartLocation.position, this.shellStartLocation.rotation) as Rigidbody;
			Instantiate (this.smoke, this.shellStartLocation.position, this.shellStartLocation.rotation);
			projectile.velocity = this.shellStartLocation.forward * this.projectileSpeed;
			projectile.GetComponent<MultiplayerProjectileController>().firingSource = gameObject;
			this.currFireDelay = 0.0f;
		} 
	}

	/*
	 * "Disappears" the object, then tells the player or enemy scripts
	 * that they're dead
	 */
	public void Kill () {
		this.isAlive = false;
		this.tankMesh.SetActive (false);
		this.rb.isKinematic = true;
		gameObject.GetComponent<Collider> ().enabled = false;
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
		gameObject.transform.position = this.startPosition;
		gameObject.transform.rotation = this.startRotation;
		this.tankMesh.SetActive (true);
		this.rb.isKinematic = false;
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
