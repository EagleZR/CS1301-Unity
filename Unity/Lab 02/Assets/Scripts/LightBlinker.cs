using UnityEngine;
using System.Collections;

public class LightBlinker : MonoBehaviour {

	public GameObject light1;
	public GameObject light2;
	public double delay;

	private float count;
	private GameObject on;
	private GameObject off;

	void Start () {
		light1.SetActive (true);
		on = light1;
		light2.SetActive (false);
		off = light2;
		count = 0;
	}

	void Update () {
		count = count + 1 * Time.deltaTime;

		if (count >= delay) {
			switchLights ();
			count = 0;
		}
	}

	void switchLights(){
		// print ("Switch Lights");
		GameObject temp = on;
		on = off;
		on.SetActive (true);
		off = temp;
		off.SetActive (false);
	}
}
