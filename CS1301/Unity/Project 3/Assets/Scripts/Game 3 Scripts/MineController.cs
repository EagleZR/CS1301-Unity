﻿/* Author: Mark Zeagler
 * Class: CS 1301
 * Instructor: Mona Chavoshi
 * Project: Game 3
 *
 * 
 */

using UnityEngine;
using System.Collections;

public class MineController : MonoBehaviour {

	public GameObject explosion;

	public float lifeAfterExplosion = 3.0f;

	private float ageAfterExplosion = 0.0f;

	private bool hasExploded = false;

	private MeshRenderer mineMesh;

	void Start () {
		mineMesh = gameObject.GetComponent<MeshRenderer>();
	}

	void Update () {
		if (this.hasExploded) {
			this.ageAfterExplosion += Time.deltaTime;
			if (this.ageAfterExplosion >= this.lifeAfterExplosion) {
				GameObject.Destroy( gameObject );
			}
		}
	}
	
	public void Explode () {
		this.hasExploded = true;
		mineMesh.enabled = false;
		Instantiate( this.explosion, transform.position, Quaternion.identity, transform );
	}

	void OnCollisionEnter (Collision other) {
		if (other.gameObject.CompareTag ("Player") || other.gameObject.CompareTag( "Enemy" )) {
			other.gameObject.GetComponent<TankController>().Kill();
			Explode();
		}
	}
}
