using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerPlayerController : MonoBehaviour {

	public Text countdownText;
	public Text restartText;
	public Text winText; 
	public Text debugText;

	public int spawnDelay;

	private MultiplayerTankController tankController;

	private double currDelay; // Timing variable for the respawn timer.
	private bool play = false;
	private bool end = false;

	private KeyCode[] keys;
	private int forward, back, left, right, r_left, r_right, action, reset; // Codes for the keys

	public TextAsset controls;

	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		tankController = gameObject.GetComponent<MultiplayerTankController> ();

		startPosition = gameObject.transform.position;

		winText.text = "You will spawn in:";
		debugText.text = "";
		restartText.text = ""; 

		// Codes for the keys
		keys = new KeyCode[8];
		forward = 0;
		back = 1;
		left = 2;
		right = 3;
		r_left = 4;
		r_right = 5;
		action = 6;
		reset = 7;
		DefineControls ();
	}
	
	// Update is called once per frame
	void Update () {
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
		if (end && Input.GetKey (keys[reset])) {
			end = false;
			resetPlayer ();
			restartText.text = "";
		}
		if (Input.GetKeyDown (keys [action])) {
			tankController.Fire ();
			// print ("Fire!");
		}

	}

	void FixedUpdate () {
		if (play) {
			// Rotate right
			if (Input.GetKey (keys[r_right])) {
				tankController.setRotation (1.0f);
			}
			// Rotate Left
			if (Input.GetKey (keys[r_left])) {
				tankController.setRotation (-1.0f);
			}
			// Move Forward
			if (Input.GetKey (keys[forward])) {
				tankController.setMovement (1.0f);
			}
			// Move Back
			if (Input.GetKey (keys[back])) {
				tankController.setMovement (-1.0f);
			}
		}
	}

	void DefineControls() {
		string controlsText = controls.ToString ();
		for (int i = 0; i < 8; i++) {
			int u = controlsText.IndexOf (':');
			controlsText = controlsText.Substring (u + 2);
			u = controlsText.IndexOf ("\n");
			string key = controlsText.Substring (0, u + 1);
			keys[i] = (KeyCode)System.Enum.Parse(typeof(KeyCode), key);
		}
	}

	// Resets the player at the start position with a 0 rotation.
	// Starts the respawn text. 
	void resetPlayer(){
		play = false;
		winText.text = "You will spawn in:";
		currDelay = 0;
		// rb.velocity = new Vector3 (0, 0, 0);
		transform.position = startPosition;
		transform.rotation = Quaternion.identity;
	}

	// Trigger to reset player after falling over edge. 
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Bottom Plane")) {
			resetPlayer ();
		} 
	}
}