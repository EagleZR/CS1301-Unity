/* Author: Mark Zeagler
 * Class: CS 1301
 * Instructor: Mona Chavoshi
 * Project: Game 3
 *
 * 
 */

using UnityEngine;
using System.Collections.Generic;

public class TerraceSpawner : MonoBehaviour {

	public List<GameObject> terraces;
	public List<GameObject> cornerTerraces;
	public List<GameObject> terraceLocations;
	public List<GameObject> cornerTerraceLocations;

	void Start () {
		foreach (GameObject currLocation in this.terraceLocations) {
			Instantiate( this.terraces[Random.Range( 0, this.terraces.Count )], currLocation.transform.position, currLocation.transform.rotation );
			currLocation.SetActive( false );
		}

		foreach( GameObject currLocation in this.cornerTerraceLocations ) {
			Instantiate( this.cornerTerraces[Random.Range( 0, this.cornerTerraces.Count )], currLocation.transform.position, currLocation.transform.rotation );
			currLocation.SetActive( false );
		}
	}
}
