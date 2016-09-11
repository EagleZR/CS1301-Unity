/* Author: Mark Zeagler
 * Class: CS 1301
 * Insructor: Mona Chavoshi
 * Project: Game 1
 * 
 * Project Description: This is a game where a single player attempts to navigate a simple 3D maze while avoiding enemies
 * travelling back and forth between waypoints. 
 * 
 * Class Description: This class handles the Enemy tank movements. Most notably, it takes the tank from its given waypointA
 * to its waypointB, and back again. It also resets the tanks should they come in contact with the Bottom Plane. I needed 
 * some help with getting the tanks to go where I wanted them to, and I documented the sources that influenced my design. 
 * 
 * Development Process: Initially, I was going to have the tanks spawned by the PlayerController.cs at the start of the game,
 * but it proved too troublesome. On a whim, I decided to just have the tanks go back-and-forth between 2 waypoints, and their
 * chaotic collisions were hilarious enough that I decided to keep it. 
 * 
 * I had a lot of issues getting the tank to follow the waypoint list. Initially, I decided to allow the tanks to rotate while
 * moving, but I had to disable it due to some issues with the ramps. It, too, was temporary, but the secondary effects following
 * the tank-on-tank collisions also made it worth leaving as is. 
 * 
 * */

using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {

	public float speed;
	public float rotateSpeed;

	// The two waypoints that the tank will go between. They have to be typed in the Unity Inspector. 
	public string waypointA;
	public string waypointB; 

	private string otherWaypoint; // A third string used for target switching. 
	private Transform target; 
	private bool rotateGood; // This variable is used on the initial rotation. The tank is not allowed to move until it's pointed at its target. 

	void Start () {
		rotateGood = false; // Tank cannot rotate until it is pointed at its target.
		otherWaypoint = waypointA; // Once it reaches the target, the new target will be selected by GameObject.Find(otherWaypoint).transform. 
		target = GameObject.Find (waypointB).transform;
	}

	void FixedUpdate () {
		if (!rotateGood) {
			Rotate (); // Cannot drive while it is rotating.
		} else {
			Move (); // Cannot rotate while it is driving. 
		}
	}

	void OnTriggerEnter (Collider other) {
		if(other.name.Equals(target.name)) { // Trigger for Waypoint arrival.
			// New waypoint selected. 
			rotateGood = false;
			string temp = target.name;
			target = GameObject.Find (otherWaypoint).transform;
			otherWaypoint = temp;
		} 
		if(other.gameObject.CompareTag("Bottom Plane")){ // Trigger for falling off the edge. 
			// Tank is reset. 
			rotateGood = false;
			target = GameObject.Find (waypointA).transform;
			otherWaypoint = waypointB;
			transform.position = target.position;
			transform.rotation = Quaternion.identity;
		}
	}

	// Tank only moves forward. 
	void Move () {
		transform.Translate (Vector3.forward * Time.deltaTime * speed);
	}
		
	// Due to the inclines and changes in height, the Vector3.RotateTowards() function had to be tricked into thinking that both the Tank and the 
	// target were the same height. This method was originally run while the tank was moving, but it led to some issues when the tank would go
	// up/down a ramp while pointed directly ahead. This way is funnier, anyways. 
	void Rotate () { // answers.unity3d.com/questions/894796/how-to-make-object-follow-path.html
		Vector3 targetPosition = target.transform.position; // answers.unity3d.com/questions/54973/rotate-an-object-to-look-at-another-object-on-one.html
		// Trick target height to 0
		targetPosition.y = 0;
		// Trick this object height to 0
		Vector3 transformPlaceholder = new Vector3 (transform.position.x, 0, transform.position.z); // use instead of transform.position
		// Trick forward height to 0
		Vector3 placeHolder2 = new Vector3 (transform.forward.x, 0, transform.forward.z);

		// Rotate imaginary-high tank to imaginary-high target
		Vector3 rotation = Vector3.RotateTowards (transform.forward, targetPosition - transformPlaceholder, rotateSpeed * Time.deltaTime, 0.0f);
		transform.forward = rotation; 

		if (!rotateGood) {
			if (Vector3.Angle (placeHolder2, transformPlaceholder - targetPosition) == 180f) {
				rotateGood = true;
			}
		}
	}
} 