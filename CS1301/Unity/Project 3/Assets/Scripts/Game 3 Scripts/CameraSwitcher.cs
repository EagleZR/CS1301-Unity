using UnityEngine;
using System.Collections.Generic;

public class CameraSwitcher : MonoBehaviour {

	public float delaySwitch;
	public List<GameObject> cameras;

	private float currTime; // check for an internal Unity version of this
	private GameObject cameraOn;

	void Start () {
		foreach ( GameObject camera in cameras ) {
			camera.SetActive( false );
		}
		this.currTime = this.delaySwitch;
		this.cameraOn = this.cameras[0];
		PickNewCamera();
	}

	// Update is called once per frame
	void FixedUpdate () {
		this.currTime -= Time.deltaTime;
		if ( this.currTime <= 0 ) {
			PickNewCamera();
			this.currTime = this.delaySwitch;
		}
	}

	private void PickNewCamera () {
		this.cameraOn.SetActive( false );
		int newCamera = (int)Random.Range( 0, this.cameras.Count );
		this.cameraOn = this.cameras[newCamera];
		this.cameraOn.SetActive( true );
		print( newCamera );
	}
}
