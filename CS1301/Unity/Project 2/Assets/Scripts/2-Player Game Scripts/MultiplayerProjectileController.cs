using UnityEngine;
using System.Collections;

public class MultiplayerProjectileController : MonoBehaviour {

	public GameObject firingSource;

	public float killTime = 4.0f;

	public bool isAlive = true;

	private float age = 0.0f;


	/* void FixedUpdate () {
		print (Vector3.Distance (gameObject.transform.position, firingSource.transform.position) + " , " + Time.time);
		if (Vector3.Distance (gameObject.transform.position, firingSource.transform.position) > 0.1f) {
			gameObject.GetComponent<Collider> ().enabled = true;
			print ("Collisions on");
		}
	}*/

	void Update () {
		age += 1.0f * Time.deltaTime;
		if (Vector3.Distance (gameObject.transform.position, firingSource.transform.position) > 100.0f || age > killTime) {
			Object.Destroy (gameObject);
		}
	}

	void OnCollisionEnter (Collision other) {
		if (other.gameObject != firingSource) {
			isAlive = false;
		}
		if (other.gameObject.CompareTag("Player")) {
			firingSource.GetComponent<MultiplayerPlayerController> ().DeclareWinner ();
		}
	}
}
