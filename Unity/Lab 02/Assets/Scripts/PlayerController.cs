using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public Text countText;
	public Text winText;
	public Text moveText;

	GameObject ball;
	private Rigidbody rb;
	private int count;
	private int moveCount;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		count = 0;
		setCountText ();
		winText.text = "";
		moveCount = 0;
		moveText.text = "";
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		moveCount += System.Math.Abs((int)System.Math.Round (moveHorizontal, 0)) + System.Math.Abs((int)System.Math.Round (moveVertical, 0));

		// print (moveCount);
		moveText.text = "Movement: " + moveCount.ToString();

		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Pick Up")) {
			other.gameObject.SetActive (false);
			count = count + 1; 
			setCountText ();
		}
	}

	void setCountText(){
		countText.text = "Count: " + count.ToString ();
		if (count >= 12) {
			winText.text = "You Win!";
		}
	}
}
