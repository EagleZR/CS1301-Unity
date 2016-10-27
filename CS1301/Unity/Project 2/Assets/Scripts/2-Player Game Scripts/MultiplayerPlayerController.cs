/* Author: Mark Zeagler
 * Class: CS 1301
 * Instructor: Mona Chavoshi
 * Project: Game 2
 * 
 * This script handles user input, and passes that to the MultiplayerTankController.cs script.
 * Inputs can be customized using the text files in the Assets/Resources/Text Files/ folder,
 * but the keyboard-layout UI image would have to be adjusted manually. I thought about
 * handling the sprite placements with this script, but I didn't really feel like it yet.
 * Maybe a later version will have it. 
 */

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
	public GameObject spawnArea;

	private MultiplayerTankController tankController;
	private MultiplayerSceneController sceneController;

	private double currDelay; // Timing variable for the respawn timer.
	private bool play = false; // If false, player cannot move. 
	private bool end = false; // If true, player prompted to respawn. 

	private KeyCode[] keys;

	private enum Keys : int {Forward = 0, Back, Left, Right, R_Left, R_Right, Action, Reset};

	public TextAsset controls;

	void Start () {
		this.sceneController = this.scene.GetComponent<MultiplayerSceneController> ();
		this.tankController = gameObject.GetComponent<MultiplayerTankController> ();

		this.winText.text = "You will spawn in:";
		this.debugText.text = "";
		this.restartText.text = ""; 

		// Codes for the keys
		this.keys = new KeyCode[8];

		DefineControls ();
	}

	void Update () {
		if (!this.sceneController.isAlive) {
			EndGame ();
			this.restartText.text = "Please press '1' (above letters) to reset.";
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
		}
	}

	void LateUpdate () {
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
			// Fire's tank weapon
			if (Input.GetKeyDown (this.keys [(int)Keys.Action])) {
				this.tankController.Fire ();
			}
		}
	}

	/*
	 * Sets the controls based off of the given text file
	 */
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

	/*
	 * Resets the player at the start position with a 0 rotation.
	 * Starts the respawn text. 
	 */
	void ResetPlayer(){
		this.play = false;
		this.winText.text = "You will spawn in:";
		this.currDelay = 0;
		spawnArea.SetActive (true);
		tankController.Spawn ();
	}

	/*
	 * Called by the projectile. Triggers the scene's end game stuff
	 */
	public void DeclareWinner () {
		this.won = true;
		sceneController.DeclareWinner (gameObject);
		this.winText.text = "You Win!";
		EndGame ();
	}

	/*
	 * Called by the scene script. If the script was not already
	 * notified that it was the winner, this will trigger the lose stuff
	 */
	void EndGame () {
		if (!won) {
			this.winText.text = "You Lose";
		}
		this.play = false;
		this.end = true;
	}

	/*
	 * Called by the tank controller when killed. This prompts for a respawn.
	 */
	public void Kill () {
		this.play = false;
		this.end = true;
		if (this.sceneController.isAlive) {
			this.winText.text = "You've been hit!";
			this.restartText.text = "Please press '" + keys [(int)Keys.Reset].ToString () + "' to reset.";
		} else {
			this.restartText.text = "Please press '" + KeyCode.Alpha1.ToString () + "' to reset.";
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.name.Equals ("Start Exit Wall")) {
			spawnArea.SetActive (false);
		}
	}
}