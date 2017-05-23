using UnityEngine;
using System.Collections;

public class RoadBuilder : MonoBehaviour {

	public GameObject roadSegment;
	public int minWidth;
	public int maxWidth;
	public int length;

	void Start () {
		// Build main road
		for ( int x = 0; x < length; x++ ) {
			for ( int y = 0; y < minWidth; y++ ) {
				GameObject currSegment = (GameObject)GameObject.Instantiate( roadSegment, new Vector3( (x - length / 2 ) * 10, ( y - minWidth / 2 ), 0 ), new Quaternion( 0, 0, 0, 0 ) );
				currSegment.SetActive( true );
				if ( y == 0 ) {
					currSegment.transform.Find( "LeftEORLine" ).gameObject.SetActive( true );
					if ( minWidth > 1 ) {
						currSegment.transform.Find( "LeftLaneLines" ).gameObject.SetActive( true );
					}
				}
				if ( y == minWidth - 1 ) {
					currSegment.transform.Find( "RightEORLine" ).gameObject.SetActive( true );
				}
				if ( y != 0 && y != minWidth - 1 ) {
					currSegment.transform.Find( "LeftLaneLines" ).gameObject.SetActive( true );
				}
				if ( x == 0 ) {
					currSegment.transform.Find( "Spawner" ).gameObject.SetActive( true );
				}
				if ( x == length - 1 ) {
					currSegment.transform.Find( "Terminator" ).gameObject.SetActive( true );
				}
			}
		}
		// TODO Build exits and entrances

	}
	
	void Update () {
	
	}
}
