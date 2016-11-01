using UnityEngine;

public class MouseController : MonoBehaviour {

	public float jetpackForce = 75.0f;
	public float forwardMovementSpeed = 3.0f;

	public ParticleSystem jetpack;

	public Transform groundCheckTransform;

	public LayerMask groundCheckLayerMask;

	public Texture2D coinIconTexture;

	public AudioSource jetpackAudio;
	public AudioSource footstepsAudio;

	public ParallaxScroll parallax;

	public GameObject coinCollectSound;

	private bool isGrounded; 
	private bool dead = false;

	private Rigidbody2D rb;

	private Animator animator;

	private uint coins = 0;

	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();
		this.animator = gameObject.GetComponent<Animator>();
	}
	
	void FixedUpdate () {
		bool jetpackActive = Input.GetButton( "Fire1" );

		jetpackActive = jetpackActive && !this.dead;

		if (jetpackActive) {
			rb.AddForce( new Vector2( 0, jetpackForce ) );
		}

		if ( !this.dead ) {
			Vector2 newVelocity = rb.velocity;
			newVelocity.x = forwardMovementSpeed;
			rb.velocity = newVelocity;
		}

		UpdateGroundedStatus();
		AdjustJetpack( jetpackActive );
		AdjustFootstepsAndJetpackSound( jetpackActive );

		parallax.offset = transform.position.x;
	}

	void UpdateGroundedStatus () {
		//1 
		this.isGrounded = Physics2D.OverlapCircle( groundCheckTransform.position, 0.1f, groundCheckLayerMask );

		//2
		this.animator.SetBool( "grounded", this.isGrounded );
	}

	void AdjustJetpack ( bool jetpackActive ) {
		jetpack.enableEmission = !this.isGrounded;
		jetpack.emissionRate = jetpackActive ? 300.0f : 75.0f;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if ( collider.gameObject.CompareTag( "Coins" ) ) {
			CollectCoin( collider );
		} else {
			HitByLaser( collider );
		}
	}

	void HitByLaser (Collider2D laserCollider) {
		if (!this.dead) {
			laserCollider.gameObject.GetComponent<AudioSource>().Play();
		}
		this.dead = true;
		this.animator.SetBool( "dead", true );
	}

	void CollectCoin(Collider2D coinCollider) {
		this.coins++;
		Instantiate( this.coinCollectSound );
		GameObject.Destroy( coinCollider.gameObject );
	}

	void OnGUI () {
		DisplayCoinsCount();
		DisplayRestartButton();
	}

	void DisplayCoinsCount () {
		Rect coinIconRect = new Rect( 10, 10, 32, 32 );
		GUI.DrawTexture( coinIconRect, coinIconTexture );

		GUIStyle style = new GUIStyle();
		style.fontSize = 30;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.yellow;

		Rect labelRect = new Rect( coinIconRect.xMax, coinIconRect.y, 60, 32 );
		GUI.Label( labelRect, this.coins.ToString(), style );
	}

	void DisplayRestartButton () {
		if (this.dead && this.isGrounded) {
			Rect buttonRect = new Rect( Screen.width * 0.35f, Screen.height * .45f, Screen.width * 0.30f, Screen.height * 0.1f );
			if (GUI.Button(buttonRect, "Tap to restart!")) {
				UnityEngine.SceneManagement.SceneManager.LoadScene( "MainScene", UnityEngine.SceneManagement.LoadSceneMode.Single );
			}
		}
	}

	void AdjustFootstepsAndJetpackSound(bool jetpackActive) {
		this.footstepsAudio.enabled = !this.dead && this.isGrounded;

		this.jetpackAudio.enabled = !this.dead && !this.isGrounded;
		this.jetpackAudio.volume = jetpackActive ? 1.0f : 0.5f;
	}
}
