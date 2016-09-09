using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float rotateSpeed;
	public float jumpSpeed;
	//public Text countText;
	public Text winText;
	//public Text moveText;
	public int spawnDelay; 
	public Text countdownText;
	public Text debugText;
	public Text restartText;
	public Text keymapText;
	public GameObject keymap;
	public bool keymapOn;
	public Camera camera;

	private Rigidbody rb;
	//private int count;
	//private int moveCount;
	private bool play;
	private bool end;
	private double currDelay; 
	private bool onGround;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		//count = 0;
		//setCountText ();
		winText.text = "You will spawn in:";
		//moveCount = 0;
		//moveText.text = "";
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

	void Update() {
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

		if (end && Input.GetKey (KeyCode.R)) {
			end = false;
			resetPlayer ();
			restartText.text = "";
		}

		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
			// print ("Quitting Application.");
		}

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

	void FixedUpdate () {
		if (play) {
			// Rotate right
			if (Input.GetKey (KeyCode.D)) {
				// transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
				// rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * Time.deltaTime * rotateSpeed));
				// rb.AddTorque (0, rotateSpeed * Time.deltaTime, 0);
				transform.rotation = camera.transform.rotation;
			}
			// Rotate Left
			if (Input.GetKey (KeyCode.A)) {
				// transform.Rotate (Vector3.down * Time.deltaTime * rotateSpeed);
				// rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.down * Time.deltaTime * rotateSpeed));
				// rb.AddTorque (0, -rotateSpeed * Time.deltaTime, 0);
				transform.rotation = camera.transform.rotation;
			}
			// Move Forward
			if (Input.GetKey (KeyCode.W)) {
				rb.AddForce (Vector3.forward * Time.deltaTime * speed);
				// transform.Translate (Vector3.forward * Time.deltaTime * speed);
			}
			// Move Back
			if (Input.GetKey (KeyCode.S)) {
				rb.AddForce (Vector3.back * Time.deltaTime * speed);
				// transform.Translate (Vector3.back * Time.deltaTime * speed);
			}
			// Move Left
			if (Input.GetKey (KeyCode.Q)) {
				rb.AddForce (Vector3.left * Time.deltaTime * speed);
				// transform.Translate (Vector3.left * Time.deltaTime * speed);
			}
			// Move Right
			if (Input.GetKey (KeyCode.E)) {
				rb.AddForce (Vector3.right * Time.deltaTime * speed);
				// transform.Translate (Vector3.right * Time.deltaTime * speed);
			}
			// Jump
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (onGround){
					// print ("Jump.");
					rb.AddForce (Vector3.up * Time.deltaTime * jumpSpeed);
					onGround = false;
					// rb.AddForce (new Vector3 (0, Time.deltaTime * speed, 0));
				}
			}

			/* 
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			*/

			//moveCount += System.Math.Abs((int)System.Math.Round (moveHorizontal, 0)) + System.Math.Abs((int)System.Math.Round (moveVertical, 0));

			// print (moveCount);
			//moveText.text = "Movement: " + moveCount.ToString();

			// rb.AddForce (movement * speed);
		} else {
			rb.velocity = new Vector3 (0, 0, 0);
		}
	}

	void OnTriggerEnter(Collider other) {
		// print ("Collision detected.");
		if (other.gameObject.CompareTag ("Bottom Plane")) {
			resetPlayer ();
		} else if (other.gameObject.CompareTag ("End Plane")) {
			WinGame ();
		} 
	}

	void OnCollisionEnter (Collision other) {
		if (other.gameObject.CompareTag ("Ground")) {
			onGround = true;
		} else if (other.gameObject.CompareTag ("Enemy")) {
			EndGame ();
		} 
	}

	//
//	void setCountText(){
//		countText.text = "Count: " + count.ToString ();
//		if (count >= 12) {
//			winText.text = "You Win!";
//		}
//	}

	void resetPlayer(){
		play = false;
		winText.text = "You will spawn in:";
		currDelay = 0;
		// print ("Resetting.");
		rb.velocity = new Vector3 (0, 0, 0);
		transform.position = new Vector3 (0, 0.5f, 0);
		transform.rotation = Quaternion.identity;
	}

	void EndGame(){
		winText.text = "You Lose";
		restartText.text = "Please press 'R' to play again.";
		play = false;
		end = true;
		rb.velocity = new Vector3 (0, 0, 0);
	}

	void WinGame(){
		// print ("You win!");
		winText.text = "You Win!";
		rb.velocity = new Vector3 (0, 0, 0);
		play = false;
		end = true;
		restartText.text = "Please press 'R' to restart";
	}

	void TankInitiator(){

	}
}
