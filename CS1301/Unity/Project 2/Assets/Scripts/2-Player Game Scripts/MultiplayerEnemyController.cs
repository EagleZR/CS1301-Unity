using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerEnemyController : MonoBehaviour {

	public Vector3 destination = new Vector3 (0f, 0f, 0f);

	public int currLevel; // May make this private later

	private ArrayList players = new ArrayList (); // TODO Check if this can be held in SceneController
	private ArrayList playersFound = new ArrayList ();
	private ArrayList enemies = new ArrayList (); // TODO Check if this can be held in SceneController

	void Start () {
		players = new ArrayList(GameObject.FindGameObjectsWithTag ("Player"));
		enemies = new ArrayList(GameObject.FindGameObjectsWithTag ("Enemy"));
	}

	void Update () {
		float yPos = gameObject.transform.position.y;
		if (yPos > 16) {
			currLevel = 4;
		} else if (yPos > 12) {
			currLevel = 3;
		} else if (yPos > 8) {
			currLevel = 2;
		} else if (yPos > 4) {
			currLevel = 1;
		} else {
			currLevel = 0;
		}
	}

	void LateUpdate () {
		if (playersFound.Count == 0) {
			Patrol ();
		} else {
			Attack ();
		}
	}

	// TODO Send out continuous raycasts at the players. Once the player has been located with a Raycast, switch to attack
	// TODO During patrol, enemy will roam around looking for the player. Create a system to handle tank movement
	// TODO Add a terrain mapping/navigation feature, likely using raycasting

		/* 1. Look for player.
		 * 		a. Look using a raycast system. 
		 * 			- If raycasts encounter the player without hitting any obstacles, player is considered found. 
		 * 			- Potentially might set a distance limit to this.
		 * 			- Also, there may be issues with a player falling over the edge, but as a player is blocked again from view by the floor, the player will be "lost", so it won't be an issue.
		 * 		b. If player found:
		 * 			- Set player's position as destination.
		 * 			- Update playersFound ArrayList.
		 * 
		 * 2. Find new destination. ### This may need some updating. ###
		 * 		a. Find all Enemy locations.
		 * 		b. Find all Enemy destinations. 
		 * 		c. If current level (based on location's y value) is over full, pick a new level to go to.
		 * 			- If selected level is projected to be full (check destinations' y values), select different level.
		 * 			- Initially aims for center of selected level.
		 * 		d. If current level is not full, or exactly full, stay on current level.
		 * 			- If the current destination is still substantially far away, continue towards current destination. 
		 * 			- If current destination has been reached, set a new destination.
		 * 				i. Pick a location that is away from other projected locations.
		 * 				ii. Preferrably, there should be some change in position. There's an issue where the tanks may just move in a circle.
		 * 
		 * 3. Move to destination.
		 * 		a. Use pathfinding to navigate there.
		 * 			- Could use a similar raycasting system to the player-seeking one.
		 * 				i. If currently on destination's level: 
		 * 					+ Rays cast within 20 degrees of destination (in y rotation).
		 * 						> If an opening is found (especially if rays go beyond destination), that angle is chosen for the tempDestination.
		 * 						> If no opening is found, though the obstacle is still far away, the tank sets a tempDestination on the target.
		 * 						> If no opening is found, and the obstacle is close, the angle is widened to go around the obstacle. If the edge of the object cannot be found, 
		 * 						  a tempDestination is set the same distance from the object a certain distance away from the current position. 
		 * 							* May need to keep some kind of temporary mapping to keep tanks from getting stuck on this step.
		 * 							* Could use mapping as a kind of memory to make tanks better the more they patrol and "learn their territory". 
		 * 					+ Tank moves towards the tempDestination.
		 * 			- Do not want an omnipotent pathifinding system, it's ok if the tanks get "lost".
		 * 		b. Allow simultaneous turning and movement. 
		 */
	void Patrol () {
		if (players.Count > 0) {
			// TODO Raycast to each Player
		}

		// TODO Figure out how the fuck you're doing this...
		// TODO Check if level is over full
		ArrayList levels = new ArrayList();
		for (int u = 0; u < 5; u++) {
			ArrayList enemiesOnLevel = new ArrayList ();
			for (int i = 0; i < enemies.Count; i++) {
				// TODO omg, this is fucked
			}
			levels.Add (enemiesOnLevel);
		}

	}

	// TODO Raycasts will be continued to be sent out at both players. Enemy will target closes player within LoS
	// TODO Once a raycast has lost contact with a player, the player is "lost" again
	// TODO If no players are located by the Raycast, revert to patrol.
	void Attack () {

	}

	/*
	 void Map () {
		/* 1. Check if current level is fully explored or not.
		 * 		a. This is checked in the char[5][100][100] levelDiscovery
		 * 		b. If fully explored, end.
		 * 		c. If 
		 * 
		 * 
		 * 
		 * 
		 * 
		 * 
		 * 
		 */ /*
	}
	*/
}
