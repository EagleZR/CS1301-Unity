using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerPlayerController : MonoBehaviour {

	public Text countdownText;
	public Text restartText;
	public Text winText; 
	public Text debugText;

	public int spawnDelay;

	public bool won = false;

	public GameObject scene;

	private MultiplayerTankController tankController;
	private MultiplayerSceneController sceneController;

	private double currDelay; // Timing variable for the respawn timer.
	private bool play = false;
	private bool end = false;

	private KeyCode[] keys;

	private enum Keys : int {Forward = 0, Back, Left, Right, R_Left, R_Right, Action, Reset};

	public TextAsset controls;

	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		this.sceneController = this.scene.GetComponent<MultiplayerSceneController> ();
		this.tankController = gameObject.GetComponent<MultiplayerTankController> ();

		this.startPosition = gameObject.transform.position;

		this.winText.text = "You will spawn in:";
		this.debugText.text = "";
		this.restartText.text = ""; 

		// Codes for the keys
		this.keys = new KeyCode[8];

		DefineControls ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!this.sceneController.isAlive) {
			EndGame ();
		} else {
			// If play is disabled, but it is not the end, this signals that the respawn timer needs to count down.
			if (!this.play && !this.end) {
				this.currDelay = this.currDelay + 1 * Time.deltaTime;
				int interval = (int)System.Math.Ceiling (this.spawnDelay - this.currDelay);
				this.countdownText.text = interval.ToString ();
				double int2 = this.spawnDelay - this.currDelay;
				this.debugText.text = int2.ToString ();
				if (this.currDelay >= this.spawnDelay) {
					this.play = true;
					this.countdownText.text = "";
					this.winText.text = "";
					this.debugText.text = "";
				}
			}

			// Resets the player. 
			if (this.end && Input.GetKey (this.keys [(int)Keys.Reset])) {
				this.end = false;
				ResetPlayer ();
				this.restartText.text = "";
			}

			if (Input.GetKeyDown (this.keys [(int)Keys.Action])) {
				this.tankController.Fire ();
				// print ("Fire!");
			}
		}
	}

	void FixedUpdate () {
		if (this.play) {
			// Rotate right
			if (Input.GetKey (this.keys[(int)Keys.R_Right])) {
				this.tankController.setRotation (1.0f);
			}
			// Rotate Left
			if (Input.GetKey (this.keys[(int)Keys.R_Left])) {
				this.tankController.setRotation (-1.0f);
			}
			// Move Forward
			if (Input.GetKey (this.keys[(int)Keys.Forward])) {
				this.tankController.setMovement (1.0f);
			}
			// Move Back
			if (Input.GetKey (this.keys[(int)Keys.Back])) {
				this.tankController.setMovement (-1.0f);
			}
		}
	}

	void DefineControls() {
		string controlsText = this.controls.ToString ();
		for (int i = 0; i < 8; i++) {
			int u = controlsText.IndexOf (':');
			controlsText = controlsText.Substring (u + 2);
			u = controlsText.IndexOf ("\n");
			string key = controlsText.Substring (0, u + 1);
			this.keys[i] = (KeyCode)System.Enum.Parse(typeof(KeyCode), key);
		}
	}

	// Resets the player at the start position with a 0 rotation.
	// Starts the respawn text. 
	void ResetPlayer(){
		this.play = false;
		this.winText.text = "You will spawn in:";
		this.currDelay = 0;
		// rb.velocity = new Vector3 (0, 0, 0);
		transform.position = this.startPosition;
		transform.rotation = Quaternion.identity;
		tankController.Spawn ();
	}

	// Trigger to reset player after falling over edge. 
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Bottom Plane")) {
			ResetPlayer ();
		} 
	}

	public void DeclareWinner () {
		this.won = true;
		sceneController.DeclareWinner (gameObject);
		this.winText.text = "You Win!";
		EndGame ();
	}

	void EndGame () {
		if (!won) {
			this.winText.text = "You Lose";
		}
		this.play = false;
		this.end = true;
	}

	public void Kill () {
		this.play = false;
		this.end = true;
		this.winText.text = "You've been hit";
		this.restartText.text = "Please press '" + keys [(int)Keys.Reset].ToString () + "' to reset.";
	}
}