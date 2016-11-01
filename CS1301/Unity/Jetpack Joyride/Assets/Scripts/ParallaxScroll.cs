using UnityEngine;
using System.Collections;

public class ParallaxScroll : MonoBehaviour {

	public Renderer background;
	public Renderer foreground;

	public float backgroundSpeed = 0.02f;
	public float foregroundSpeed = 0.06f;
	public float offset = 0;

	void Start () {
	
	}
	
	void Update () {
		float backgroundOffset = this.offset * this.backgroundSpeed;
		float foregroundOffset = this.offset * this.foregroundSpeed;

		this.background.material.mainTextureOffset = new Vector2( backgroundOffset, 0 );
		this.foreground.material.mainTextureOffset = new Vector2( foregroundOffset, 0 );
	}
}
