using UnityEngine;
using System.Collections;

public class RoadBuilder_v2_0 : MonoBehaviour {

	public SimController controller;

	#region Road Characteristics
	[Range( 0, 100 )]
	public int numExits;
	[Range( 50, 1000 )]
	public int roadLength;
	[Range( 1, 10 )]
	public int mostLanes;
	[Range( 1, 10 )]
	public int fewestLanes;
	#endregion

	#region Prefabs
	public GameObject roadSegment;
	public GameObject entranceRamp;
	public GameObject exitRamp;
	#endregion

	void Start () {
		int targNumSegments = Random.Range( ( mostLanes - fewestLanes ) * 2, roadLength / 10 );
		int prevSegmentWidth = 0;
		bool roadNarrowing = ( Random.Range( 0, 100 ) > 50 );
		GameObject[,] road = new GameObject[roadLength, mostLanes + 1];
		ArrayList spawners = new ArrayList();
		// Iterates through whole road (x = 0 --> x = roadLength)
		for ( int x = 0; x < this.roadLength; x++ ) {
			int segmentLength = Random.Range( 10, this.roadLength / targNumSegments * 2 );
			int segmentWidth = ( prevSegmentWidth == 0 ? Random.Range( fewestLanes, mostLanes ) : ( roadNarrowing ? --prevSegmentWidth : ++prevSegmentWidth ) );
			int segStartX = x;
			// Iterates through segment (x = startX --> x= startX + segmentLength)
			for ( ; x <= segStartX + segmentLength && x < this.roadLength; x++ ) {
				// Iterates through lanes of segment (y = 0 --> y = segmentWidth)
				for ( int y = 0; y < segmentWidth; y++ ) {
					GameObject currSegment = (GameObject)GameObject.Instantiate( roadSegment, new Vector3( x * 10, y, 0 ), new Quaternion( 0, 0, 0, 0 ) );
					currSegment.name = "RoadSegment (" + x + "," + y + ")";
					road[x, y+1] = currSegment;
					currSegment.SetActive( true );
					if ( y == 0 ) {
						currSegment.transform.Find( "RightEORLine" ).gameObject.SetActive( true );
						if ( segmentWidth > 1 ) {
							currSegment.transform.Find( "LeftLaneLines" ).gameObject.SetActive( true );
						}
					}
					if ( y == segmentWidth - 1 ) {
						currSegment.transform.Find( "LeftEORLine" ).gameObject.SetActive( true );
					}
					if ( y != 0 && y != segmentWidth - 1 ) {
						currSegment.transform.Find( "LeftLaneLines" ).gameObject.SetActive( true );
					}
					if ( x == 0 ) {
						GameObject spawner = currSegment.transform.Find( "Spawner" ).gameObject;
						spawner.gameObject.SetActive( true );
						spawners.Add( spawner );
					}
					if ( x == roadLength - 1 ) {
						currSegment.transform.Find( "Terminator" ).gameObject.SetActive( true );
					}
				}
			}
			x--;
			prevSegmentWidth = segmentWidth;
			if ( segmentWidth >= mostLanes || segmentWidth <= fewestLanes || Random.Range( 0, 100 ) > 50 ) {
				roadNarrowing = !roadNarrowing;
			}
		}

		// Pass back through lanes and add the EndOfRoad object to lanes that end
		for ( int x = 10; x < roadLength; x++ ) {
			for ( int y = 1; y < mostLanes+1; y++ ) {
				if ( road[x, y] == null && road[x - 1, y - 1] != null ) {
					road[x - 1, y - 1].transform.Find( "EndOfRoad" ).gameObject.SetActive( true );
				}
			}
		}

		// Add entrances and exits
		GameObject currRamp = ( Random.Range( 0, 100 ) > 50 ? entranceRamp : exitRamp );
		GameObject[] exits = new GameObject[numExits];
		int lastRampPos = 0;
		int rampLength = 7;
		for ( int rn = 0; rn < numExits * 2; rn++ ) {
			for ( int x = lastRampPos; x < roadLength - rampLength; x++ ) {
				for ( int y = 1; y < mostLanes+1; y++ ) {
					if ( road[x, y] != null ) {
						bool validPos = true;
						for ( int i = 1; i < rampLength + 1; i++ ) {
							if ( road[x + i, y] != null && road[x + i, y - 1] == null ) {
								// do nothing
							} else {
								validPos = false;
								break;
							}
						}
						if ( validPos ) {
							GameObject newRamp = (GameObject)GameObject.Instantiate( currRamp, new Vector3( x * 10, y-1 , 0 ), Quaternion.identity );
							if ( currRamp.Equals( exitRamp ) ) {
								exits[rn / 2] = newRamp;
							}
							GameObject spawner = newRamp.transform.Find( "Spawner" ).gameObject;
							spawners.Add( spawner );
							for (int i = 0; i < rampLength; i += (currRamp.Equals(exitRamp) ? i : -1)) {
								road[x + i, y - 1] = newRamp;
							}
							newRamp.SetActive( true );
							y = mostLanes;
							lastRampPos = x;
							x = roadLength;
							currRamp = ( currRamp.Equals( exitRamp ) ? entranceRamp : exitRamp );
						}
					} else {
						y = mostLanes; // Finish segment (no use checking interior lanes)
					}
				}
				if (x == roadLength - rampLength) {
					print( "Unable to find location for ramp" );
				}
			}
		}
		controller.spawners = spawners;
		controller.roadBuilder = this;
		controller.road = road;
		controller.enabled = true;
	}
}
