/* Author: Mark Zeagler
 * Class: CS 1301
 * Instructor: Mona Chavoshi
 * Project: Game 3
 *
 * 
 */

using UnityEngine;
using System.Collections;

public class SmokeKill : MonoBehaviour {

	public float smokeLifetime;

	private float currLife;

	void Start () {
		currLife = 0.0f;
	}

	void Update () {
		currLife += 1.0f * Time.deltaTime;
		if (currLife >= smokeLifetime) {
			Object.Destroy (gameObject);
		}
	}
}
