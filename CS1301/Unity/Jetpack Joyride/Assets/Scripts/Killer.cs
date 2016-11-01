using UnityEngine;
using System.Collections;

public class Killer : MonoBehaviour {

	float lifetime = 0.2f;
	float age = 0.0f;
	void Start () {
	
	}
	
	void Update () {
		this.age += 1.0f * Time.deltaTime;
		if (this.age >= this.lifetime) {
			GameObject.Destroy( gameObject );
		}
	}
}
