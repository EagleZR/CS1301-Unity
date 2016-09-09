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
