using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {

	public float speed;
	public float rotateSpeed;

	private Rigidbody rb;
	private Transform target;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
		target = GameObject.Find ("Tank 1 Path 1").transform;
		Move ();
	}

	void Move () {
		// Check rotation
		/* if (rotation is not good) {
		 * 		rotate towards target;
		 * } else { // Rotation is good
		 * 		move towards target;
		 * }
		 * */
		Vector3 rotation = Vector3.RotateTowards (transform.forward, target.position - transform.position, rotateSpeed * Time.deltaTime, 0.0f);
		// Vector3 checkRotation = transform.rotation.eulerAngles;
		// print (rotation.y.ToString() + " , " + checkRotation.y.ToString());
		if (rotation.y <= 0.01f) {
			transform.forward = rotation; 
			print (target.position - transform.position);
		} else {
			print ("Ready to move");
		}
	}
}
// answers.unity3d.com/questions/894796/how-to-make-object-follow-path.html