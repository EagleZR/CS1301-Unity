/* Author: Mark Zeagler
 * Class: CS 1301
 * Instructor: Mona Chavoshi
 * Project: Game 3
 *
 * 
 */

// TODO NOTE: May be able to create a generic enemy class with shared behaviors
using UnityEngine;
using System.Collections.Generic;

public class TurretController : MonoBehaviour {

	public float turnSpeed;
	public float projectileSpeed;
	public float fireDelay;
	public float sightDistance;

	public Rigidbody shell;
	public GameObject smoke;
	public GameObject destroyedSmoke;
	public GameObject baseMesh;
	public GameObject turretMesh;
	public Transform shellStartLocation;

	public bool isAlive;

	private float fireCoolDown;

	private GameObject target;

	private List<GameObject> hiddenTanks;
	private List<GameObject> foundTanks;

	private bool lastTurnRight = false;

	void Start () {
		this.foundTanks = new List<GameObject>();
		this.hiddenTanks = new List<GameObject>( GameObject.FindGameObjectsWithTag( "Player" ) ); // TODO populate this
		this.hiddenTanks.AddRange( GameObject.FindGameObjectsWithTag( "Enemy Tank" ) );
	}

	void Update () {
		if ( foundTanks.Count > 0 ) {
			Attack();
		} else {
			Patrol();
		}
	}

	void FixedUpdate () {
		Move();
	}

	/* 
	* Looks for players. If players are found, it sets the nearest one as a target. If players are not found, 
	* but they should be, the players are removed from the list, and 
	*/
	void LookForTanks () {
		// Look for tanks not found
		for ( int i = 0; i < this.hiddenTanks.Count; i++ ) {
			Ray ray = new Ray( transform.position, this.hiddenTanks[i].transform.position - transform.position );
			int layerMask = LayerMask.GetMask( "Tanks", "Structure" ); // TODO Fix layers
			RaycastHit hitInfo;

			if ( Physics.Raycast( ray, out hitInfo, this.sightDistance, layerMask ) ) {
				if ( hitInfo.collider.gameObject.Equals( this.hiddenTanks[i] ) ) {
					// add to found list
					this.foundTanks.Add( this.hiddenTanks[i] );
					// remove from hidden list
					this.hiddenTanks.RemoveAt( i );
					// Don't wanna accidentally skip one
					i--;
				}
			}
		}

		// Look for tanks already found (in case they disappear)
		for ( int i = 0; i < this.foundTanks.Count; i++ ) {
			Ray ray = new Ray( transform.position, this.foundTanks[i].transform.position - transform.position );
			int layerMask = LayerMask.GetMask( "Tanks", "Structure" ); // TODO Fix layers
			RaycastHit hitInfo;

			if ( Physics.Raycast( ray, out hitInfo, this.sightDistance, layerMask ) ) {
				if ( !hitInfo.collider.gameObject.Equals( this.foundTanks[i] ) ) {
					// add to hidden list
					this.hiddenTanks.Add( this.foundTanks[i] );

					// remove from found list
					this.foundTanks.RemoveAt( i );
					// Don't wanna accidentally skip one
					i--;
				}
			}
		}
		this.target = FindNearest();
	}

	/* 
	* Locates the nearest found player. If no players are found, it returns null.
	*/
	GameObject FindNearest () {
		GameObject nearest = null;
#pragma warning disable 0168 // Disable the unused variable warning
		try {
			nearest = this.foundTanks[0];
			for ( int i = 1; i < this.foundTanks.Count; i++ ) {
				if ( Vector3.Distance( transform.position, this.foundTanks[i].transform.position ) < Vector3.Distance( transform.position, nearest.transform.position ) ) {
					nearest = this.foundTanks[i];
				}
			}
		} catch ( System.ArgumentOutOfRangeException e ) {
		}
#pragma warning restore 0168
		return nearest;
	}

	void Patrol () {
		if ( Vector3.Angle( transform.forward, target.transform.position ) < .1 ) {
			GetNewTarget();
		} else {
			FaceTarget();
		}
	}

	// Method 1: Pick a random GameObject as the target and turn towards it

	// Method 2: Turn 60 degrees right, 30 degrees left, and repeat
	Vector3 GetNewTarget () {
		
		if (this.lastTurnRight) {

		}
		// TODO Find a way to generate a new target to look at
		return Vector3.left;
	}

	void FaceTarget () {
		float angle = Vector3.Angle( transform.forward, this.target.transform.position - transform.position );

		float rightAngle = Vector3.Angle( transform.right, this.target.transform.position - transform.position );
		float leftAngle = Vector3.Angle( -transform.right, this.target.transform.position - transform.position );

		float turnAmount = 1.0f;

		// Decreases the turn amount when the direction is close to where it's supposed to be
		if ( angle < 1.0f ) {
			turnAmount /= 10.0f;
		}
		if ( angle < 0.1f ) {
			turnAmount /= 10.0f;
		}

		// Turns left if left is closer, turns right if right is closer
		if ( angle > 0.01f ) {
			if ( rightAngle < leftAngle ) {
				// TODO Turn right
			} else {
				// TODO Turn left
			}
		}
	}

	void Attack () {
		if ( Vector3.Angle( transform.forward, target.transform.position ) < .1 ) {
			Fire();
		} else {
			FaceTarget();
		}
	}

	void Fire () {
		// TODO Fire

		this.fireCoolDown = this.fireDelay;
	}

	void Move () {
		// TODO Move
	}
}
