using UnityEngine;
using System.Collections;

public class TerminatorController : MonoBehaviour {

	private void OnTriggerEnter (Collider collider ) {
		if ( collider.CompareTag( "Car" ) ) {
			GameObject.Destroy( collider.gameObject );
		}
	}
}
