using UnityEngine;
using System.Collections;
using System.Collections.ArrayList;

public class MultiplayerEnemyController : MonoBehaviour {

	private ArrayList<GameObject> players = new ArrayList<GameObject> ();
	private ArrayList<GameObject> playersFound = ArrayList<GameObject>();

	// Use this for initialization
	void Start () {
		players =  
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// TODO Send out continuous raycasts at the players. Once the player has been located with a Raycast, switch to attack
	// TODO During patrol, enemy will roam around looking for the player. Create a system to handle tank movement
	// TODO Add a terrain mapping/navigation feature, likely using raycasting
	void Patrol () {

	}

	// TODO Raycasts will be continued to be sent out at both players. Enemy will target closes player within LoS
	// TODO Once a raycast has lost contact with a player, the player is "lost" again
	// TODO If no players are located by the Raycast, revert to patrol.
	void Attack () {

	}
}
