using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerEnemyController : MonoBehaviour {

	private Vector3 destination = new Vector3 (0f, 0f, 0f);
	private Vector3 startPosition;
	private Quaternion startRotation;

	public GameObject tankMesh;
	public GameObject scene;

	public float delayDeath = 2.0f;
	public float respawnTime = 4.0f;
	public float sightDistance = 10.0f;
	public float deathCounter = 0.0f; // WTF is this?

	public bool isAlive = true;

	private MultiplayerTankController tankController;
	private MultiplayerSceneController sceneController;

	private NavMeshAgent navigator;

	public GameObject target;

	public List<GameObject> playersFound = new List<GameObject> ();
	private List<GameObject> playersHidden;

	void Start () {
		this.tankController = gameObject.GetComponent<MultiplayerTankController> ();
		this.sceneController = scene.GetComponent<MultiplayerSceneController> ();
		this.playersHidden = scene.GetComponent<MultiplayerSceneController> ().GetPlayers ();
		this.navigator = gameObject.GetComponent<NavMeshAgent> ();

		this.startPosition = gameObject.transform.position;
		this.startRotation = gameObject.transform.rotation;

		this.destination = transform.position;
	}

	void Update () {
		if (this.isAlive) {
			Respawn ();
		}
	}

	void LateUpdate () {
		if (this.isAlive) {
			LookForPlayers ();
			if (this.playersFound.Count == 0) {
				Patrol ();
			} else {
				Attack ();
			}
		} else {
			navigator.Stop ();
		}
	}

	void Patrol () {
		FindDestination ();
		navigator.SetDestination(destination);
	}
		
	// TODO Once a raycast has lost contact with a player, the player is "lost" again
	// TODO If no players are located by the Raycast, revert to patrol.
	void Attack () {
		Ray checkSight = new Ray (transform.position, target.transform.position - transform.position);
		RaycastHit hitInfo;
		int layerMask = LayerMask.GetMask ("Players", "Structure");
		Physics.Raycast (checkSight, out hitInfo, this.sightDistance, layerMask);

		/*	if (within 10m of target, and there is a clear line of fire) {
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
			navigator.Stop ();
			if (Vector3.Angle (transform.forward, this.target.transform.position - transform.position) < 1.0f && hitInfo.collider.name.Equals(this.target.name)) {
				this.tankController.Fire ();
			} else {
				// TODO turn towards target
				FindRotation ();
			}
		} else {
			this.destination = this.target.transform.position;
			navigator.SetDestination (this.destination);
			navigator.Resume ();
		}
	}

	void FindRotation () {
		float angle = Vector3.Angle (transform.forward, this.target.transform.position - transform.position);

		float rightAngle = Vector3.Angle (transform.right, this.target.transform.position - transform.position);
		float leftAngle = Vector3.Angle (-transform.right, this.target.transform.position - transform.position);

		float turnAmount = 1.0f;

		// Decreases the turn amount when the direction is close to where it's supposed to be
		if (angle < 10.0f) {
			turnAmount /= 10.0f;
		} 
		if (angle < 5.0f) {
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

	void CheckAlive () {
		this.isAlive = this.tankController.isAlive;
	}

	// TODO take another look at this
	void Respawn () {
		this.deathCounter += 1.0f * Time.deltaTime;

		if (deathCounter > delayDeath) {
			this.tankMesh.SetActive (false); // TODO find a better way to do this... maybe
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
			this.destination = transform.position;
			this.destination = this.sceneController.GenerateDestination (gameObject);
		}
	}
		
	// TODO figure out why this isn't working...
	void LookForPlayers () {
		// Look for players not found
		for (int i = 0; i < playersHidden.Count; i++) {
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
		for (int i = 0; i < playersFound.Count; i++) {
			Ray ray = new Ray (transform.position, this.playersFound [i].transform.position - transform.position);
			int layerMask = LayerMask.GetMask ("Players", "Structure");
			RaycastHit hitInfo;

			if (Physics.Raycast (ray, out hitInfo, this.sightDistance, layerMask)) {
				if (!hitInfo.collider.gameObject.Equals (this.playersFound [i])) {
					// add to hidden list
					this.playersHidden.Add (this.playersFound [i]);
					if (this.playersFound [i].Equals(target)) {
						destination = target.transform.position; // may not be necessary, but maybe... 
						target = null;
					}

					// remove from found list
					this.playersFound.RemoveAt (i);
					// Don't wanna accidentally skip one
					i--;
				}
			}
		}

		if (this.playersFound.Count > 0) {
			this.target = FindNearest ();
		}
	}
		
	GameObject FindNearest () {
		GameObject nearest = this.playersFound [0];
		for (int i = 1; i < this.playersFound.Count; i++) {
			if (Vector3.Distance(transform.position, this.playersFound[i].transform.position) < Vector3.Distance(transform.position, nearest.transform.position)) {
				nearest = this.playersFound[i];
			}
		}
		return nearest;
	}

	void FindDestination () {
		if (Vector3.Distance (transform.position, this.destination) < 5.0f) {
			this.destination = sceneController.GenerateDestination (gameObject);
			// print (transform.position + " , " + this.destination);
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Bottom Plane")) {
			this.tankController.Kill();
		} 
	}

	void Reset () {
		transform.position = this.startPosition;
		transform.rotation = this.startRotation;
	}

	public Vector3 GetDestination () {
		return this.destination;
	}

	public void Kill () {
		// Object.Destroy (gameObject);
	}
}
