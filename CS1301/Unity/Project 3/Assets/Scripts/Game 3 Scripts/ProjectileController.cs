/* Author: Mark Zeagler
 * Class: CS 1301
 * Instructor: Mona Chavoshi
 * Project: Game 2
 * 
 * This is a simple script that handles the collision and death behaviors of projectiles. 
 * The projectiles are either destroyed on collision, or after they've travelled a certain
 * distance from the object that fired them. If they collide with a player or enemy, they
 * will inform that object that it is dead, using the MultiplayerTankController.cs script. 
 */

using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public GameObject firingSource;
	public GameObject explosion;

	public float killTime = 4.0f;

	private float age = 0.0f;

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
				Instantiate (this.explosion, other.transform.position, other.transform.rotation);

				if (other.gameObject.CompareTag ("Player") && other.gameObject != firingSource && firingSource.name.Contains ("Player")) { // This triggers the end-game stuff
					firingSource.GetComponent<PlayerController> ().DeclareWinner ();
				}

				Object.Destroy (gameObject); // Regardless of what it hits (other than it's firer's collider), the projectile is destroyed on impact.
			}
		}
	}
}
