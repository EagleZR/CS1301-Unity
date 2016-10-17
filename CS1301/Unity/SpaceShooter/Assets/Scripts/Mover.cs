using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
	public float speed;

	private Rigidbody rb;

	void Start ()
	{
		rb = gameObject.GetComponent<Rigidbody> ();
		rb.velocity = transform.forward * speed;
	}
}