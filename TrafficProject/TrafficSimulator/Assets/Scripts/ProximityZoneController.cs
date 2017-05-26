using UnityEngine;
using System.Collections;

public class ProximityZoneController : MonoBehaviour {

	public ArrayList containedCars;

	void Start () {
		containedCars = new ArrayList();
	}

	private void OnTriggerEnter ( Collider other ) {
		if ( ( other.CompareTag( "Car" ) && !other.Equals( gameObject ) ) || other.CompareTag( "Road Indicator" ) ) {
			containedCars.Add( other.gameObject );
		}
	}

	private void OnTriggerExit ( Collider other ) {
		if ( containedCars.Contains( other.gameObject ) ) {
			containedCars.Remove( other.gameObject );
		}
	}
}
