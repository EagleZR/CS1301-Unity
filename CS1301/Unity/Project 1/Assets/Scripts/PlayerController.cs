/* Author: Mark Zeagler
 * Class: CS 1301
 * Insructor: Mona Chavoshi
 * Project: Game 1
 * 
 * Project Description: This is a game where a single player attempts to navigate a simple 3D maze while avoiding enemies
 * travelling back and forth between waypoints. 
 * 
 * Class Description: This class handles user input, player movement, and some of the game mechanics. The mechanics
 * include the win/lose mechanic, player resetting, and simple UI commands. It is applied to the Empty in the scene.
 * The Empty has both the camera and the sphere mesh as subordinates.
 * 
 * Development Process: This class initially handled the sphere directly with RigidBody.AddForce. However, this did not 
 * allow for the camera to rotate around the object. Instead transform.Translate was used. This allows for unrealist, 
 * almost weightless movement, but also camera movement. An attempt was made to recreate the CameraController.cs to allow
 * for RigidBody.AddForce movement, but the player sphere did not rotate well. 
 * 
 * */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float rotateSpeed;
	public float jumpSpeed;

	public Text winText; 
	public Text countdownText;
	public Text debugText;
	public Text restartText;
	public Text keymapText;

	public int spawnDelay;
	public GameObject keymap;

	private double currDelay; // Timing variable for the respawn timer. 
	private Rigidbody rb;

	private bool keymapOn; // Used to toggle the keymap image on/off. 
	private bool play; // This is set to false if the player ever comes in contact with the Bottom Plane, the finish area, or an Enemy. Set to true after respawn timer expires.
	private bool end; // Set to true when player collides with enemy or hits the finish area trigger. Set to true after player hits reset button. 
	private bool onGround; // Determines if the player can jump or not. Currently, if the player leaves the ground without jumping (falls), jump is still enabled, but that isn't an issue. 

	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
		winText.text = "You will spawn in:";
		play = false;
		end = false;
		currDelay = 0;
		int interval = (int)System.Math.Ceiling(spawnDelay - currDelay);
		countdownText.text = interval.ToString();
		debugText.text = "";
		restartText.text = ""; 
		onGround = true;
		keymapText.text = "Please press 'F1' to hide the controls.";
		keymapOn = true;
	}

	// Handles non-movement keypresses and respawn timer. 
	void Update() {
		// If play is disabled, but it is not the end, this signals that the respawn timer needs to count down.
		if (!play && !end) {
			currDelay = currDelay + 1 * Time.deltaTime;
			int interval = (int)System.Math.Ceiling(spawnDelay - currDelay);
			countdownText.text = interval.ToString();
			double int2 = spawnDelay - currDelay;
			debugText.text = int2.ToString ();
			if (currDelay >= spawnDelay) {
				play = true;
				countdownText.text = "";
				winText.text = "";
				debugText.text = "";
			}
		}

		// Resets the player. 
		if (end && Input.GetKey (KeyCode.R)) {
			end = false;
			resetPlayer ();
			restartText.text = "";
		}

		// Quits the game (usually). 
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
		}

		// Toggles the keymap on/off. 
		if (Input.GetKeyDown (KeyCode.F1)) {
			if (keymapOn) {
				keymapText.text = "Please press 'F1' to show the controls.";
				keymap.SetActive (false);
				keymapOn = false;
			} else {
				keymapText.text = "Please press 'F1' to hide the controls.";
				keymap.SetActive (true);
				keymapOn = true;
			}
		}
	}

	// Used to handle the movement keypresses. transform.Translate is used instead of
	// RigidBody.AddForce because of the camera rotation. The RigidBody fix I tried that
	// enabled camera rotation had other issues.
	void FixedUpdate () {
		if (play) {
			// Rotate right
			if (Input.GetKey (KeyCode.D)) {
				transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
			}
			// Rotate Left
			if (Input.GetKey (KeyCode.A)) {
				transform.Rotate (Vector3.down * Time.deltaTime * rotateSpeed);
			}
			// Move Forward
			if (Input.GetKey (KeyCode.W)) {
				transform.Translate (Vector3.forward * Time.deltaTime * speed);
			}
			// Move Back
			if (Input.GetKey (KeyCode.S)) {
				transform.Translate (Vector3.back * Time.deltaTime * speed);
			}
			// Move Left
			if (Input.GetKey (KeyCode.Q)) {
				transform.Translate (Vector3.left * Time.deltaTime * speed);
			}
			// Move Right
			if (Input.GetKey (KeyCode.E)) {
				transform.Translate (Vector3.right * Time.deltaTime * speed);
			}
			// Jump
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (onGround){
					rb.AddForce (Vector3.up * Time.deltaTime * jumpSpeed);
					onGround = false;
				}
			}
		} else {
			// Cancels out momentum if character play is disabled (win or Enemy contact). 
			rb.velocity = new Vector3 (0, 0, 0);
		}
	}

	// Trigger to reset player or win game. 
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Bottom Plane")) {
			resetPlayer ();
		} else if (other.gameObject.CompareTag ("End Plane")) {
			WinGame ();
		} 
	}

	// Collision to allow jumping or from contact with Enemy. 
	void OnCollisionEnter (Collision other) {
		if (other.gameObject.CompareTag ("Ground")) {
			onGround = true;
		} else if (other.gameObject.CompareTag ("Enemy")) {
			EndGame ();
		} 
	}

	// Resets the player at the start position with a 0 rotation.
	// Starts the respawn text. 
	void resetPlayer(){
		play = false;
		winText.text = "You will spawn in:";
		currDelay = 0;
		rb.velocity = new Vector3 (0, 0, 0);
		transform.position = new Vector3 (0, 0.5f, 0);
		transform.rotation = Quaternion.identity;
	}

	// Triggered by contact with Enemy.
	void EndGame(){
		winText.text = "You Lose";
		play = false;
		end = true;
		rb.velocity = new Vector3 (0, 0, 0);
		restartText.text = "Please press 'R' to play again.";
	}

	// Triggered by invisible plane above the finish area.
	void WinGame(){
		winText.text = "You Win!";
		play = false;
		end = true;
		rb.velocity = new Vector3 (0, 0, 0);
		restartText.text = "Please press 'R' to restart";
	}
}
