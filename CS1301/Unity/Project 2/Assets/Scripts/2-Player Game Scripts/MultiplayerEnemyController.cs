using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerEnemyController : MonoBehaviour {

	public Vector3 destination = new Vector3 (0f, 0f, 0f);
	public Vector3 startPosition;
	public Quaternion startRotation;
	public GameObject tankMesh;
	public GameObject castingSource;

	public float delayDeath = 2.0f;
	public float respawnTime = 4.0f;
	public float sightDistance = 10.0f;
	public float navigationCheckDistance = 10.0f;

	public GameObject scene;

	public bool isAlive = true;

	public int currLevel; // May make this private later

	// public Material tankColor;

	private MultiplayerTankController tankController;
	private MultiplayerSceneController sceneController;

	private Vector3 tempDestination = new Vector3 (0f, 0f, 0f);

	// private Color defaultColor;

	public float deathCounter = 0.0f;

	// private List<GameObject> players = new List <GameObject> (); // TODO Check if this can be held in SceneController
	private List<GameObject> playersFound = new List<GameObject> ();
	// private List<GameObject> enemies = new List<GameObject> (); // TODO Check if this can be held in SceneController

	void Start () {
		// defaultColor = gameObject.GetComponentInChildren<Material> ();
		this.tankController = gameObject.GetComponent<MultiplayerTankController> ();
		this.sceneController = scene.GetComponent<MultiplayerSceneController> ();
		this.startPosition = gameObject.transform.position;
		this.startRotation = gameObject.transform.rotation;
		// players = new List<GameObject>(GameObject.FindGameObjectsWithTag ("Player"));
		// enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag ("Enemy"));
	}

	void Update () {
		if (isAlive) {
			CheckAlive ();
		} else {
			Respawn ();
		}
		// CheckLevel ();
	}

	/*
	void CheckLevel () {
		float yPos = gameObject.transform.position.y;
		if (yPos > 16) {
			this.currLevel = 4;
		} else if (yPos > 12) {
			this.currLevel = 3;
		} else if (yPos > 8) {
			this.currLevel = 2;
		} else if (yPos > 4) {
			this.currLevel = 1;
		} else {
			this.currLevel = 0;
		}
	}
*/
	void LateUpdate () {
		if (this.isAlive) {
			LookForPlayers ();
			if (this.playersFound.Count == 0) {
				Patrol ();
			} else {
				Attack ();
			}
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
		// PickDestination ();
		navigateToDestination ();
		MoveToDestination ();
	}

	// TODO Raycasts will be continued to be se	nt out at both players. Enemy will target closes player within LoS
	// TODO Once a raycast has lost contact with a player, the player is "lost" again
	// TODO If no players are located by the Raycast, revert to patrol.
	void Attack () {
		this.destination = FindNearest ().transform.position;

		navigateToDestination ();

		MoveToDestination ();

		if (Vector3.Angle (transform.forward, this.destination - transform.position) < 0.1f) {
			this.tankController.Fire ();
		}
	}

	bool navigateToDestination () {
		// Check directly ahead
		Vector3 angleToDestination = this.destination - transform.position;
		Ray ray = new Ray (transform.position, angleToDestination);
		// http://answers.unity3d.com/questions/287724/create-ray-with-an-angle.html
		// Vector3 downAngle = Quaternion.AngleAxis (-45, transform.up) * angleToDestination;
		int layerMask = LayerMask.GetMask ("Enemies", "Structure");
		Ray rayDown = new Ray (this.castingSource.transform.position, Vector3.down);
		RaycastHit hitInfo;

		// print (!Physics.Raycast (ray, out hitInfo, navigationCheckDistance, layerMask) + " , " + Physics.Raycast (rayDown, 2.0f));
		if ((!Physics.Raycast (ray, out hitInfo, this.navigationCheckDistance) || hitInfo.collider.gameObject.CompareTag ("Player")) && Physics.Raycast (rayDown, 2.0f)) { // if (nothing in front && there is floor)
			this.tempDestination = this.destination;
			return true; // TODO figure out if this is needed
		} else { // There's an obstacle or hole. Need to navigate around it! :D
			// print (hitInfo.collider.gameObject.name);
			/* int bestAngle = 0;
			float bestDistanceLeft = 0.0f;

			for (int i = 0; i < 180; i++) {
				float checkLeft = 0.0f;
				float checkRight = 0.0f;
			} */
			this.tempDestination = transform.position;
			return true;
		}

	}

	void MoveToDestination () {
		float angle = Vector3.Angle (transform.forward, this.tempDestination - transform.position);
		// print (angle);
		FindRotation (angle);
		if (angle < 1.0f) {
			FindMovement ();
		}
	}

	void FindMovement () {
		if (Vector3.Distance (transform.position, this.tempDestination) > 10) {
			this.tankController.setMovement (1.0f);
		}
	}

	void FindRotation (float angle) {
		
		float rightAngle = Vector3.Angle (transform.right, this.tempDestination - transform.position);
		float leftAngle = Vector3.Angle (-transform.right, this.tempDestination - transform.position);
		float turnAmount = 1.0f;

		// Decreases the turn amount when the direction is close to where it's supposed to be
		if (angle < 1.0f) {
			turnAmount = 0.1f;
		} else if (angle < 0.1f) {
			turnAmount = .01f;
		}

		// Turns left if left is closer, turns right if right is closer
		if (angle > 0.01f) {
			if (rightAngle < leftAngle) {
				this.tankController.setRotation (turnAmount);
			} else {
				this.tankController.setRotation (-turnAmount);
			}
		}
	}

	// http://answers.unity3d.com/questions/568423/how-do-i-check-if-raycast-is-hitting-a-gameobject.html
	void LookForPlayers () {
		for (int i = 0; i < sceneController.players.Count; i++) {
			Ray ray = new Ray (transform.position, this.sceneController.players [i].transform.position - transform.position);
			int layerMask = LayerMask.GetMask ("Players", "Structure");
			RaycastHit hitInfo;
			if (Physics.Raycast (ray, out hitInfo, this.sightDistance, layerMask)) {
				if (hitInfo.collider.gameObject.Equals (this.sceneController.players [i]) && !this.playersFound.Contains(this.sceneController.players[i])) {
					this.playersFound.Add (this.sceneController.players [i]);
				} // else {
					// print ("Players not found " + hitInfo.collider.name);
				// }
			} // else {
				// print ("Players not found");
			// }
		}
			
	}

	GameObject FindNearest () {
		GameObject nearest = this.playersFound [0];
		for (int i = 1; i < this.playersFound.Count; i++) {
			if (Vector3.Distance(transform.position, this.playersFound[i].transform.position) < Vector3.Distance(transform.position, nearest.transform.position)) {
				nearest = this.playersFound[i];
			}
		}
		// print (nearest.name);
		return nearest;
	}

	void CheckAlive () {
		if (!this.tankController.isAlive) {
			this.isAlive = false;
		}
	}

	void Respawn () {
		this.deathCounter += 1.0f * Time.deltaTime;

		if (deathCounter > delayDeath) {
			this.tankMesh.SetActive (false); // TODO find a better way to do this
			gameObject.GetComponent<Collider> ().enabled = false;
		} 

		if (deathCounter > respawnTime) {
			this.tankMesh.SetActive (true);
			gameObject.transform.position = this.startPosition;
			gameObject.transform.rotation = this.startRotation;
			this.deathCounter = 0.0f;
			this.isAlive = true;
			this.tankController.isAlive = true;
			gameObject.GetComponent<Collider> ().enabled = true;
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Bottom Plane")) {
			this.tankController.Kill();
		} 
	}

	void Reset () {
		transform.position = this.startPosition;
		transform.rotation = Quaternion.identity;
	}

	/*
	 * // TODO get this working... 
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
