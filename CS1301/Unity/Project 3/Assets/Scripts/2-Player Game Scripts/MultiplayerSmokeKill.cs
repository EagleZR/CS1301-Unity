using UnityEngine;
using System.Collections;

public class MultiplayerSmokeKill : MonoBehaviour {

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
