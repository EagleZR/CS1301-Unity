/* Author: Mark Zeagler
 * Class: CS 1301
 * Instructor: Mona Chavoshi
 * Project: Game 3
 * 
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneController : MonoBehaviour {
	
	public Text keymapText;
	public Text centerText;
	public Text newGameText;

	public GameObject keymap;
	public GameObject enemy;

	public AudioSource sceneAudio;
	public AudioClip winSong;
	public AudioClip music;

	public bool isAlive = true;

	private List<GameObject> enemies;

	private float winTextLifetime = 3.0f;
	private float winTextTimer = 0.0f;
	private float winSongTimer = 0.0f;
	private float winSongLength = 2.0f;

	private Vector3 waypoint1 = new Vector3 (0f, 0f, 0f);

	// For Reference A floors
	private Vector3 waypoint2A = new Vector3 (37.5f, 0f, 37.5f);
	private Vector3 waypoint3A = new Vector3 (-37.5f, 0f, 37.5f);
	private Vector3 waypoint4A = new Vector3 (37.5f, 0f, -37.5f);
	private Vector3 waypoint5A = new Vector3 (-37.5f, 0f, -37.5f);

	// For Reference A floors
	private Vector3 waypoint2B = new Vector3 (37.5f, 0f, 0f);
	private Vector3 waypoint3B = new Vector3 (-37.5f, 0f, 0f);
	private Vector3 waypoint4B = new Vector3 (0f, 0f, 37.5f);
	private Vector3 waypoint5B = new Vector3 (0f, 0f, -37.5f);

	// Floor multipliers
	private float [] floorHeight = {1.0f, 5.0f, 9.0f, 13.0f, 17.0f};

	private char [] floorType = {'A', 'B', 'A', 'B', 'A'}; 

	private bool keymapOn = true; // Used to toggle the keymap image on/off.

	void Start () {
		GenerateEnemies ();
		centerText.text = "Defeat the other player to win!";
		newGameText.text = "";
	}

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

		// Reloads the game if one of the players has already won
		if(!this.isAlive && Input.GetKeyDown (KeyCode.Alpha1)) {
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Multiplayer Scene", UnityEngine.SceneManagement.LoadSceneMode.Single);
		}

		// At the beginning of the game, hides the center instructions after a certain amount of time
		if (winTextTimer < winTextLifetime) {
			winTextTimer += 1 * Time.deltaTime;
		} else if (isAlive && winTextTimer >= winTextLifetime) {
			centerText.text = "";
		}

		if (!this.isAlive) {
			if (this.sceneAudio.clip == this.winSong) {
				this.winSongTimer += 1.0f * Time.deltaTime;
				if (this.winSongTimer >= this.winSongLength) {
					this.sceneAudio.clip = this.music;
					this.sceneAudio.loop = true;
					this.sceneAudio.Play ();
				}
			}
		}
	}

	/* 
	 * Creates the enemies at the beginning of the game.
	 */
	void GenerateEnemies () {
		this.enemy.GetComponent<EnemyController> ().scene = gameObject;
		for (int i = 0; i < 5; i++) {
			if (floorType [i] == 'A') {
				Instantiate (this.enemy, new Vector3 (this.waypoint2A.x, this.floorHeight [i], this.waypoint2A.z), Quaternion.identity);
				Instantiate (this.enemy, new Vector3 (this.waypoint3A.x, this.floorHeight [i], this.waypoint3A.z), Quaternion.identity);
				Instantiate (this.enemy, new Vector3 (this.waypoint4A.x, this.floorHeight [i], this.waypoint4A.z), Quaternion.identity);
				Instantiate (this.enemy, new Vector3 (this.waypoint5A.x, this.floorHeight [i], this.waypoint5A.z), Quaternion.identity);
			} else {
				Instantiate (this.enemy, new Vector3 (this.waypoint2B.x, this.floorHeight [i], this.waypoint2B.z), Quaternion.identity);
				Instantiate (this.enemy, new Vector3 (this.waypoint3B.x, this.floorHeight [i], this.waypoint3B.z), Quaternion.identity);
				Instantiate (this.enemy, new Vector3 (this.waypoint4B.x, this.floorHeight [i], this.waypoint4B.z), Quaternion.identity);
				Instantiate (this.enemy, new Vector3 (this.waypoint5B.x, this.floorHeight [i], this.waypoint5B.z), Quaternion.identity);
			}
		}
		this.enemies = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Enemy"));
	}

	/*
	 * Generates a new destination for a requesting Enemy
	 */
	public Vector3 GenerateDestination (GameObject requester) {
		
		List<GameObject>[] currLevels = new List<GameObject>[5];
		List<GameObject>[] projLevels = new List<GameObject>[5];

		int requesterLevel;

		if (requester.transform.position.y > 16) {
			requesterLevel = 4;
		} else if (requester.transform.position.y > 12) {
			requesterLevel = 3;
		} else if (requester.transform.position.y > 8) {
			requesterLevel = 2;
		} else if (requester.transform.position.y > 4) {
			requesterLevel = 1;
		} else if (requester.transform.position.y > 0) {
			requesterLevel = 0;
		} else {
			return new Vector3 (0.0f, 0.0f, 0.0f);
		}

		for (int i = 0; i < 5; i++) {
			currLevels [i] = new List<GameObject> ();
			projLevels [i] = new List<GameObject> ();
		}

		// Find how many enemies are, and will be, on each level
		for (int i = 0; i < enemies.Count; i++) {
			// Check which level each enemy is currently on
			float height = enemies [i].transform.position.y;
			int level = -1;

			if (height > 16) {
				level = 4;
			} else if (height > 12) {
				level = 3;
			} else if (height > 8) {
				level = 2;
			} else if (height > 4) {
				level = 1;
			} else if (height > 0) {
				level = 0;
			} // else falling

			// Add 1 to count for that level
			if (level >= 0) {
				currLevels [level].Add (enemies [i]);
			}

			// Check which level each enemy is going towards
			height = enemies [i].GetComponent <EnemyController> ().GetDestination ().y;
			level = -1;

			if (height > 16) {
				level = 4;
			} else if (height > 12) {
				level = 3;
			} else if (height > 8) {
				level = 2;
			} else if (height > 4) {
				level = 1;
			} else if (height > 0) {
				level = 0;
			} // else falling

			// Add 1 to count for that level
			if (level >= 0) {
				projLevels [level].Add (this.enemies [i]);
			}
		}

		// Find the emptiest level && emptiest projected level
		int emptiestLevel = -1;
		int emptiestProjectedLevel = -1;

		for (int i = 0; i < 5; i++) {
			if (currLevels [i].Count < 4) {
				if (emptiestLevel != -1) {
					if (currLevels [i].Count < currLevels [emptiestLevel].Count) {
						emptiestLevel = i;
					}
				} else {
					emptiestLevel = i;
				}
			} // else, don't need to flip-flop any

			if (projLevels [i].Count < 4) {
				if (emptiestProjectedLevel != -1) {
					if (projLevels [i].Count < projLevels [emptiestProjectedLevel].Count) {
						emptiestLevel = i;
					}
				} else {
					emptiestProjectedLevel = i;
				}
			} // else, don't need to flip-flop any
		}

		int newDestinationLevel;

		// idk what I'm doing here, or if it even matters
		if (emptiestLevel >= 0 && emptiestProjectedLevel >= 0) {
			if (projLevels [emptiestProjectedLevel].Count < currLevels [emptiestLevel].Count) {
				newDestinationLevel = emptiestProjectedLevel;
			} else {
				newDestinationLevel = emptiestLevel;
			}
		} else if (emptiestLevel >= 0) {
			newDestinationLevel = emptiestLevel;
		} else if (emptiestProjectedLevel >= 0) {
			newDestinationLevel = emptiestProjectedLevel;
		} else {
			newDestinationLevel = requesterLevel;
		}

		Vector3 [] floorWaypoint = new Vector3[5];

		// Find waypoint with farthest distance from enemies' destinations
		if (floorType [newDestinationLevel] == 'A') {
			floorWaypoint [0] = new Vector3 (waypoint1.x, floorHeight [newDestinationLevel], waypoint1.z);
			floorWaypoint [1] = new Vector3 (waypoint2A.x, floorHeight [newDestinationLevel], waypoint2A.z);
			floorWaypoint [2] = new Vector3 (waypoint3A.x, floorHeight [newDestinationLevel], waypoint3A.z);
			floorWaypoint [3] = new Vector3 (waypoint4A.x, floorHeight [newDestinationLevel], waypoint4A.z);
			floorWaypoint [4] = new Vector3 (waypoint5A.x, floorHeight [newDestinationLevel], waypoint5A.z);
		} else {
			floorWaypoint [0] = new Vector3 (waypoint1.x, floorHeight [newDestinationLevel], waypoint1.z);
			floorWaypoint [1] = new Vector3 (waypoint2B.x, floorHeight [newDestinationLevel], waypoint2B.z);
			floorWaypoint [2] = new Vector3 (waypoint3B.x, floorHeight [newDestinationLevel], waypoint3B.z);
			floorWaypoint [3] = new Vector3 (waypoint4B.x, floorHeight [newDestinationLevel], waypoint4B.z);
			floorWaypoint [4] = new Vector3 (waypoint5B.x, floorHeight [newDestinationLevel], waypoint5B.z);
		}

		// Find the waypoint furthest away from where everyone is going to
		float furthestDistance = 0.0f;
		int waypointFurthestDistance = 0;

		for (int i = 0; i < 5; i++) {
			float sum = 0.0f;
			for (int u = 0; u < projLevels [newDestinationLevel].Count; u++) {
				sum += Vector3.Distance (floorWaypoint [i], projLevels [newDestinationLevel] [u].GetComponent<EnemyController> ().GetDestination ());
			}

			if (sum > furthestDistance && Vector3.Distance(floorWaypoint [i], requester.transform.position) > 10) {
				furthestDistance = sum;
				waypointFurthestDistance = i;
			}
		}

		return floorWaypoint [waypointFurthestDistance];
	}

	/*
	 * Ends the game when there is a winner.
	 */
	public void DeclareWinner (GameObject winner) {
		this.centerText.text = winner.name + " won!";
		this.sceneAudio.clip = this.winSong;
		this.sceneAudio.loop = false;
		this.sceneAudio.Play ();
		this.isAlive = false;
	}
}
