/* *** UNUSED ***
 * 
 * Author: Mark Zeagler
 * Class: CS 1301
 * Insructor: Mona Chavoshi
 * Project: Game 1
 * 
 * Project Description: This is a game where a single player attempts to navigate a simple 3D maze while avoiding enemies
 * travelling back and forth between waypoints. 
 * 
 * Class Description: This class is unused. It was originally intended to control the camera while allowing RigidBody.AddForce
 * to roll the ball around instead of the weightless transform.Translate. However, that build had too many issues and was 
 * abandoned. 
 * 
 * The idea was to have the camera simply modify it's (x,z) position around the sphere, keeping its height static, while pointing
 * at the player sphere. However, the sphere would not rotate
 * 
 * */

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// https://www.youtube.com/watch?v=Ta7v27yySKs

	public GameObject player;
	public float rotSpeed;

	//private Vector3 offset;
	private float rotationX = 0.0f;
	private float distance = Mathf.Sqrt(20);
	private float setYHeight;

	void Start () {
		setYHeight = transform.position.y;
		// offset = transform.position - player.transform.position;
	}

	void Update () {
		if (Input.GetKey (KeyCode.D)) {
			rotationX += 1 * rotSpeed * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.A)) {
			rotationX += -1 * rotSpeed * Time.deltaTime;
		}
	}

	void LateUpdate () {
		// transform.position = player.transform.position + offset;
		Vector3 dir = new Vector3 (0, 1.5f, -distance); 
		Quaternion rotation = Quaternion.Euler (0, rotationX, 0);
		transform.position = player.transform.position + rotation * dir;
		transform.LookAt (player.transform.position);
	}
}