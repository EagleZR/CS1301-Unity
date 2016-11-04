/* Author: Mark Zeagler
 * Class: CS 1301
 * Instructor: Mona Chavoshi
 * Project: Game 2
 * 
 * This is the enemy controller, responsible for making the enemies move around in patrol,
 * look for players, and attack the players. Originally, it was going to have its own 
 * navigation system, in which case it would use the MultiplayerTankController.cs script
 * to actually handle the movement, but it was easier to just go with Unity's built-in 
 * navigation system, though it results in some non-tank-like movement (translating left
 * and right). 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {

	public Vector3 destination = new Vector3 (0f, 0f, 0f);
	public string status = "";

	public GameObject tankMesh;
	public GameObject scene;

	public float respawnTime = 4.0f;
	public float sightDistance = 10.0f;
	public float deathCounter = 0.0f; // WTF is this?

	public bool isAlive = true;

	private TankController tankController;
	private SceneController sceneController;

	private NavMeshAgent navigator;

	public GameObject target;

	public List<GameObject> playersFound = new List<GameObject> ();
	private List<GameObject> playersHidden;

	void Start () {
		this.tankController = gameObject.GetComponent<TankController> ();
		this.tankController.movementHandledOutside = true;
		this.sceneController = this.scene.GetComponent<SceneController> ();
		this.playersHidden = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Player"));
		this.navigator = gameObject.GetComponent<NavMeshAgent> ();

		this.navigator.Stop ();
		this.navigator.Resume ();

		this.destination = transform.position;
	}

	void Update () {
		if (this.sceneController.isAlive) {
			if (!this.isAlive) {
				Respawn ();
			}
		}
	}

	void LateUpdate () {
		if (this.sceneController.isAlive) {
			if (this.isAlive) {
				LookForPlayers ();
				if (this.playersFound.Count == 0) { // If no players are found
					this.status = "Patrol";
					Patrol ();
				} else {
					this.status = "Attack";
					Attack ();
				}
			} else { // If dead
				this.navigator.Stop ();
				this.tankController.isMoving = false;
			}
		} else {
			this.navigator.Stop ();
		}
	}

	/* 
	 * When there are no players found, this just moves the tank.
	 */
	void Patrol () {
		this.tankController.movementHandledOutside = true;
		FindDestination ();
		this.navigator.SetDestination(this.destination);
	}
		
	/* 
	 * Moves the tank towards the target. Once within range, it stops moving, rotates toward the target, and fires.
	 */
	void Attack () {
		Ray checkSight = new Ray (transform.position, this.target.transform.position - transform.position);
		RaycastHit hitInfo;
		int layerMask = LayerMask.GetMask ("Players", "Structure");
		Physics.Raycast (checkSight, out hitInfo, this.sightDistance, layerMask);

		/*	if (within 10m of target, and there is a clear line of fire) {
		 * 		stop moving
		 * 		if (the angle is pointing at the target) {
		 * 			fire
		 * 		} else {
		 * 			point towards target
		 * 		}
		 * 	} else {
		 * 		navigate to target
		 * 	}
		 */
		if (Vector3.Distance (transform.position, target.transform.position) <= 10.0f && hitInfo.collider.name.Equals(this.target.name)) {
			this.navigator.Stop ();
			this.tankController.movementHandledOutside = false;
			if (Vector3.Angle (transform.forward, this.target.transform.position - transform.position) < 1.0f && hitInfo.collider.name.Equals(this.target.name)) {
				this.tankController.Fire ();
			} else {
				FindRotation ();
			}
		} else {
			this.destination = this.target.transform.position;
			this.navigator.SetDestination (this.destination);
			this.navigator.Resume ();
			this.tankController.isMoving = true;
		}
	}

	/* 
	 * Turns the tank towards the player when within range.
	 */
	void FindRotation () {
		float angle = Vector3.Angle (transform.forward, this.target.transform.position - transform.position);

		float rightAngle = Vector3.Angle (transform.right, this.target.transform.position - transform.position);
		float leftAngle = Vector3.Angle (-transform.right, this.target.transform.position - transform.position);

		float turnAmount = 1.0f;

		// Decreases the turn amount when the direction is close to where it's supposed to be
		if (angle < 1.0f) {
			turnAmount /= 10.0f;
		} 
		if (angle < 0.1f) {
			turnAmount /= 10.0f;
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

	/* 
	 * Respawns the enemy tank if it has been killed. 
	 */
	void Respawn () {
		this.deathCounter += 1.0f * Time.deltaTime;

		if (this.deathCounter > this.respawnTime) {
			this.tankController.Spawn ();
			this.navigator.Resume ();
			this.deathCounter = 0.0f;
			this.isAlive = true;
			this.destination = transform.position;
			this.destination = this.sceneController.GenerateDestination (gameObject);
		}
	}

	/* 
	 * Looks for players. If players are found, it sets the nearest one as a target. If players are not found, 
	 * but they should be, the players are removed from the list, and 
	 */
	void LookForPlayers () {
		// Look for players not found
		for (int i = 0; i < this.playersHidden.Count; i++) {
			Ray ray = new Ray (transform.position, this.playersHidden [i].transform.position - transform.position);
			int layerMask = LayerMask.GetMask ("Players", "Structure");
			RaycastHit hitInfo;

			if (Physics.Raycast (ray, out hitInfo, this.sightDistance, layerMask)) {
				if (hitInfo.collider.gameObject.Equals (this.playersHidden [i])) {
					// add to found list
					this.playersFound.Add (this.playersHidden [i]);
					// remove from hidden list
					this.playersHidden.RemoveAt (i);
					// Don't wanna accidentally skip one
					i--;
				} 
			}
		}

		// Look for players already found (in case they disappear)
		for (int i = 0; i < this.playersFound.Count; i++) {
			Ray ray = new Ray (transform.position, this.playersFound [i].transform.position - transform.position);
			int layerMask = LayerMask.GetMask ("Players", "Structure");
			RaycastHit hitInfo;

			if (Physics.Raycast (ray, out hitInfo, this.sightDistance, layerMask)) {
				if (!hitInfo.collider.gameObject.Equals (this.playersFound [i])) {
					// add to hidden list
					this.playersHidden.Add (this.playersFound [i]);

					/* If the player that is now hidden was the target, the destination is set
					 * as the last known position of the target. After the enemy is transferred
					 * back to patrol mode, it will move to that location, at which point, it 
					 * may find the target again. This makes it harder to lose an enemy than
					 * simply going around a wall, or down a level.
					 */
					if (this.playersFound [i].Equals(target)) {
						this.navigator.Resume ();
						this.destination = target.transform.position; // may not be necessary, but maybe... 
						this.target = null;
					}

					// remove from found list
					this.playersFound.RemoveAt (i);
					// Don't wanna accidentally skip one
					i--;
				}
			}
		}
		this.target = FindNearest ();
	}
		
	/* 
	 * Locates the nearest found player. If no players are found, it returns null.
	 */
	GameObject FindNearest () {
		GameObject nearest = null;
		#pragma warning disable 0168 // Disable the unused variable warning
		try { 
			nearest = this.playersFound [0];
			for (int i = 1; i < this.playersFound.Count; i++) {
				if (Vector3.Distance(transform.position, this.playersFound[i].transform.position) < Vector3.Distance(transform.position, nearest.transform.position)) {
					nearest = this.playersFound[i];
				}
			}
		} catch (System.ArgumentOutOfRangeException e) {
		}
		#pragma warning restore 0168
		return nearest;
	}

	/* 
	 * Retrieves a new destination from the MultiplayerSceneController.cs script.
	 * If the tank is still traveling towards a destination, this method does nothing.
	 * Once the tank is within a certain distance of the destination, it'll ask for a 
	 * new one. 
	 */
	void FindDestination () {
		if (Vector3.Distance (transform.position, this.destination) < 5.0f) {
			this.destination = this.sceneController.GenerateDestination (gameObject);
		}
	}

	/* 
	 * Returns this object's destination for use by an outside script.
	 */
	public Vector3 GetDestination () {
		return this.destination;
	}

	/* 
	 * Called by the MultiplayerTankController.cs script when the shared object is killed.
	 */
	public void Kill () {
		this.isAlive = false;
	}
}
