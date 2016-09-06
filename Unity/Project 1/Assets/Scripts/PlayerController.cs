using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	//public Text countText;
	public Text winText;
	//public Text moveText;
	public int spawnDelay; 
	public Text countdownText;
	public Text debugText;
	public Text restartText;

	private Rigidbody rb;
	//private int count;
	//private int moveCount;
	private bool play;
	private bool end;
	private double currDelay; 

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
			print ("Quitting Application.");
		}
	}

	void FixedUpdate () {
		if (play) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

			//moveCount += System.Math.Abs((int)System.Math.Round (moveHorizontal, 0)) + System.Math.Abs((int)System.Math.Round (moveVertical, 0));

			// print (moveCount);
			//moveText.text = "Movement: " + moveCount.ToString();

			rb.AddForce (movement * speed);
		} else {
			rb.velocity = new Vector3 (0, 0, 0);
		}
	}

	void OnTriggerEnter(Collider other) {
		print ("Collision detected.");
		if (other.gameObject.CompareTag ("Bottom Plane")) {
			resetPlayer();
		} else if (other.gameObject.CompareTag ("End Plane")) {
			WinGame ();
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
		currDelay = 0;
		print ("Resetting.");
		rb.velocity = new Vector3 (0, 0, 0);
		transform.position = new Vector3 (0, 0.5f, 0);
	}

	void EndGame(){
		winText.text = "You Lose";
		restartText.text = "Please press 'R' to play again.";
		play = false;
		end = true;
		rb.velocity = new Vector3 (0, 0, 0);
	}

	void WinGame(){
		print ("You win!");
		winText.text = "You Win!";
		rb.velocity = new Vector3 (0, 0, 0);
		play = false;
		end = true;
		restartText.text = "Please press 'R' to restart";
	}

	void TankInitiator(){

	}
}
