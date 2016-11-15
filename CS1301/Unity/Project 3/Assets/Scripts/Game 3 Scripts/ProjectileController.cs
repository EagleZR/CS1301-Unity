/* Author: Mark Zeagler
 * Class: CS 1301
 * Instructor: Mona Chavoshi
 * Project: Game 3
 * 
 * 
 */

using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public GameObject firingSource;
	public GameObject firingSmoke;
	public GameObject explosion;

	public float killTime = 4.0f;

	private float age = 0.0f;

	void Start () {
		Instantiate( this.firingSmoke, transform.position, transform.rotation );
	}

	/* 
	 * Kills the porjectile after it's traveled a certain distance away from its firing source. This prevents the build-up of unimportant objects.
	 */
	void Update () {
		age += 1.0f * Time.deltaTime;
		if (Vector3.Distance (gameObject.transform.position, firingSource.transform.position) > 100.0f || age > killTime) {
			Object.Destroy (gameObject);
		}
	}

	/* 
	 * Handles different collisions in different ways, passing the correct message to the correct 
	 */ 
	void OnCollisionEnter (Collision other) {
		if (other.gameObject != firingSource) { // Had some issues where it would hit the collider on its way out.
			if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy") { 
				other.gameObject.GetComponent <TankController> ().Kill (); 
			} // TODO Add something for the turrets
			Instantiate( this.explosion, transform.position, transform.rotation );
			GameObject.Destroy( gameObject ); // Regardless of what it hits (other than it's firer's collider), the projectile is destroyed on impact.
		}
	}
}
