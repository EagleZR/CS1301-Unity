using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

	public Sprite laserOnSprite;
	public Sprite laserOffSprite;

	public float interval = 0.5f;
	public float rotationSpeed = 0.0f;

	private bool isLaserOn = true;

	private float timeUnitlNextToggle;

	public BoxCollider2D thisCollider;

	void Start () {
		this.timeUnitlNextToggle = this.interval;
		this.thisCollider = gameObject.GetComponent<BoxCollider2D>();
	}
	
	void FixedUpdate () {
		this.timeUnitlNextToggle -= Time.fixedDeltaTime;

		if (this.timeUnitlNextToggle <= 0) {
			this.isLaserOn = !this.isLaserOn;

			this.thisCollider.enabled = this.isLaserOn;

			SpriteRenderer spriteRenderer = (SpriteRenderer)gameObject.GetComponent<Renderer>();
			if(this.isLaserOn) {
				spriteRenderer.sprite = laserOnSprite;
			} else {
				spriteRenderer.sprite = laserOffSprite;
			}

			this.timeUnitlNextToggle = this.interval;
		}

		transform.RotateAround( transform.position, Vector3.forward, rotationSpeed * Time.fixedDeltaTime );
	}
}
