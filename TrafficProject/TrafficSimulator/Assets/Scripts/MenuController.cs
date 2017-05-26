using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Simulation will start with this menu. Once the user has pressed 
/// "Begin", this script will start the RoadBuilder script, and will 
/// display a loading screen. Once the RoadBuilder script is finished,
/// it will initiate the SimController script, which will disable this
/// script and the RoadBuilder script, and will take over the application. 
/// </summary>
public class MenuController : MonoBehaviour {

	public MonoBehaviour roadBuilder;

	void Start () {
		roadBuilder.enabled = true;
	}
}
