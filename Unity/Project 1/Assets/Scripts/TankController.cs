using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {

	public float speed;
	public float rotateSpeed;
	// public TextAsset waypointText;
	// public string waypointFileName;

	// private Rigidbody rb;
	private Transform target;
	private bool rotateGood;
	// private bool isGrounded;
	private string waypointString;
	// private TextAsset waypointText;
	private ArrayList waypoints; 
	private int currWaypoint;

	void Start () {
		// rb = GetComponent<Rigidbody> ();
		rotateGood = false;
		// isGrounded = true;
		currWaypoint = 1;

		TextAsset waypointText = Resources.Load("TextFiles/Tank 1 Path a") as TextAsset;
		waypointString = waypointText.ToString();
		CreateWaypointsObject ();
		string currWaypointText = (string)waypoints [currWaypoint];
		// print (currWaypointText.Length);
		target = GameObject.Find (currWaypointText).transform;
		// print (waypoints[currWaypoint] + " , " + target.name);
	}

	void FixedUpdate () {
		// GravityPull ();
		if (!rotateGood) {
			Rotate ();
		} else {
			Move ();
		}
	}

	void OnTriggerEnter (Collider other) {
		if (target.name.Contains("End")) {
			// keep driving
		} else if(other.name.Equals(target.name)) {
			rotateGood = false;
			currWaypoint++;
			string waypoint = (string) waypoints [currWaypoint];
			target = GameObject.Find (waypoint).transform;
			print (target.name);
		} 
		if(other.gameObject.CompareTag("Bottom Plane")){
			rotateGood = false;
			currWaypoint = 0;
			string waypoint = (string)waypoints [currWaypoint];
			target = GameObject.Find (waypoint).transform;
			transform.position = target.position;
			transform.rotation = Quaternion.identity;
		}
	}

	void Move () {
		// Rotate ();
		transform.Translate (Vector3.forward * Time.deltaTime * speed);
	}

	void Rotate () {
		Vector3 targetPosition = target.transform.position; // answers.unity3d.com/questions/54973/rotate-an-object-to-look-at-another-object-on-one.html
		// Trick target height to 0
		targetPosition.y = 0;
		// Trick this object height to 0
		Vector3 transformPlaceholder = new Vector3 (transform.position.x, 0, transform.position.z); // use instead of transform.position
		// Trick forward height to 0
		Vector3 placeHolder2 = new Vector3 (transform.forward.x, 0, transform.forward.z);

		// Rotate imaginary-high tank to imaginary-high target
		// Vector3 rotation = Vector3.RotateTowards (transform.forward, targetPosition - transformPlaceholder, rotateSpeed * Time.deltaTime, 0.0f);
		// print (rotation);
		// transform.forward = rotation; 
		transform.LookAt(target);

		if (!rotateGood) {
			if (Vector3.Angle (placeHolder2, transformPlaceholder - targetPosition) == 180f) {
				rotateGood = true;
				print ("G2G");
			}
		}
	}

	/*
	// http://answers.unity3d.com/questions/585997/monodevelop-401-copy-paste-issue.html
	void GravityPull () {
		float snapDistance = 1.1f; //Adjust this based on the CharacterController's height and the slopes you want to handle - my CharacterController's height is 1.8 
		RaycastHit hitInfo = new RaycastHit();
		if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hitInfo, snapDistance)) {
			isGrounded = true;
			transform.position = hitInfo.point;
			transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
		} else {
			isGrounded = false;
		}
	}
	*/

	// int exit = 10; 
	// int i = 0;
	void CreateWaypointsObject () {
		waypoints = new ArrayList();
		while (waypointString.Length != 0 /*&& i < exit*/) {
			int index = waypointString.IndexOf ("\n");
			string waypoint = waypointString.Substring (0, index);
			waypoint = waypoint.Trim ();
			waypoints.Add (waypoint);
			waypointString = waypointString.Substring (index + 1);
			// print (waypoint + " , " + waypointString);
			// i++;
		}
	}
} 
// answers.unity3d.com/questions/894796/how-to-make-object-follow-path.html
