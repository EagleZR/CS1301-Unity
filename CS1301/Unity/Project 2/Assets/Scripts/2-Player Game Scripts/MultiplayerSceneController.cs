using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MultiplayerSceneController : MonoBehaviour {
	
	public Text keymapText;
	public Text centerText;

	public GameObject keymap;

	public List<GameObject> players;

	private bool keymapOn; // Used to toggle the keymap image on/off.

	// Use this for initialization
	void Start () {
		players = new List<GameObject> (GameObject.FindGameObjectsWithTag("Player"));
	}
	
	// Update is called once per frame
	void Update () {
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
}
