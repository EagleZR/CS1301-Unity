using System.Collections;
using UnityEngine;

public class SimController : MonoBehaviour {

	public ArrayList spawners;
	public GameObject[,] road;
	public GameObject[] exits;
	public MonoBehaviour roadBuilder;
	public MonoBehaviour menuController;

	// Use this for initialization
	void Start () {
		roadBuilder.enabled = false;
	}

	public void TransmitSpawners ( ArrayList spawners ) {
		this.spawners = spawners;
	}
}
