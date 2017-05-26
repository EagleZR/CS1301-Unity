using UnityEngine;
using System.Collections;
using System;

public class CarController : MonoBehaviour {
	public enum Aggression { EASYGOING, MODERATE, AGGRESSIVE };

	#region Static Variables
	private static float[] accelerations = { 1.5f /*Easygoing*/, 3.0f /*Moderate*/, 6.0f /*Aggressive*/ }; // use Aggression enum
	#endregion

	#region Unity-defined Variables
	// Unity Controlled Variables
	[Tooltip( "in m/s" )]
	public float preferredSpeed;
	[Tooltip( "in m/s" )]
	public float maxPositiveAcceleration;
	[Tooltip( "in m/s" )]
	public float maxBrakingSpeed;
	[Range( 0, 2 )]
	public int aggression; // Use Aggression enum
	[Range( 1, 10 )]
	public int driverComptetency;

	public float carLength = 1.6f;
	#endregion

	#region Colliders 
	// Colliders for swiching lanes.
	/// <summary>
	/// Checks if there is a rear-end type collision that will occur 
	/// if this car switches lanes. This type of collision occurs when 
	/// there is a difference in speed between the cars of the different 
	/// lanes. This is meant to check farther ahead and further behind than
	/// the merge colliders.
	/// </summary>
	public GameObject leftFrontTurnCollider;
	/// <summary>
	/// Checks if there is a rear-end type collision that will occur 
	/// if this car switches lanes. This type of collision occurs when 
	/// there is a difference in speed between the cars of the different 
	/// lanes. This is meant to check farther ahead and further behind than
	/// the merge colliders.
	/// </summary>
	public GameObject rightFrontTurnCollider;

	public GameObject leftRearTurnCollider;

	public GameObject rightRearTurnCollider;
	/// <summary>
	/// Checks if there is a merge-type collision that will occur if 
	/// this car switches lanes. This type of collision occurs when 
	/// the two cars are at similar velocities. The collider is closer
	/// in size to the size of its parent car.
	/// </summary>
	public GameObject leftMergeCollider;
	/// <summary>
	/// <para>Checks if there is a merge-type collision that will occur if 
	/// this car switches lanes.</para><para>This type of collision occurs when 
	/// the two cars are at similar velocities.</para><para>The collider is closer
	/// in size to the size of its parent car.</para>
	/// </summary>
	public GameObject rightMergeCollider;
	/// <summary>
	/// Checks the area (including adjacent lanes) in front of the car. Used for emergency braking.
	/// </summary>
	public GameObject forwardCollider;
	/// <summary>
	/// Checks lane in front for an oncoming collision. 
	/// </summary>
	public GameObject frontCollider;

	/*
	[Space( 5 )]
	[Header( "Colliders" )]
	[Tooltip( "Use the Aggressive/Moderate/EasyGoing Colliders" )]
	public GameObject AggressiveColliders;
	[Tooltip( "Use the Aggressive/Moderate/EasyGoing Colliders" )]
	public GameObject ModerateColliders;
	[Tooltip( "Use the Aggressive/Moderate/EasyGoing Colliders" )]
	public GameObject EasyGoingColliders;
	*/
	#endregion

	#region Private Variables
	private int destination; // Go by exit #, -1 means indefinite 
	public int Destination { get { return this.destination; } }
	private float preferredAcceleration;
	public float PrefferedAcceleration { get { return this.preferredAcceleration; } }
	private Rigidbody rb;
	public Rigidbody RB { get { return this.rb; } }

	private DrivingBehavior currBehavior;
	public DrivingBehavior CurrBehavior {
		set { this.currBehavior = value; }
	}

	private DrivingAttributes currAttribute;
	public DrivingAttributes CurrAttribute {
		get { return this.currAttribute; }
	}
	#endregion

	private void Start () {
		// Convert m/s to u/s
		preferredSpeed = preferredSpeed / 4.5f * 1.6f;
		maxPositiveAcceleration = maxPositiveAcceleration / 4.5f * 1.6f;
		maxBrakingSpeed = maxBrakingSpeed / 4.5f * 1.6f;

		rb = gameObject.GetComponent<Rigidbody>();
		this.preferredAcceleration = accelerations[this.aggression];
		currBehavior = new CruisingBehavior( gameObject );
		currAttribute = ( this.aggression == (int)Aggression.EASYGOING ? (DrivingAttributes)new EasyGoingAttributes() : ( this.aggression == (int)Aggression.MODERATE ? (DrivingAttributes)new ModerateAttributes() : (DrivingAttributes)new AggressiveAttributes() ) );

		leftMergeCollider.transform.localScale = new Vector3( carLength * currAttribute.MergeBufferMultiplier, leftMergeCollider.transform.localScale.y, leftMergeCollider.transform.localScale.z );
		rightMergeCollider.transform.localScale = new Vector3( carLength * currAttribute.MergeBufferMultiplier, rightMergeCollider.transform.localScale.y, rightMergeCollider.transform.localScale.z );
	}

	void LateUpdate () {
		AdjustColliders();
		currBehavior.ExecuteAction();
		LateralDrag();
	}

	void AdjustColliders () {
		frontCollider.transform.localScale = new Vector3( GetBrakingDistance( GetAverageBrakingSpeed(), rb.velocity.x, 0 ) * currAttribute.FrontBufferMultiplier, frontCollider.transform.localScale.y, frontCollider.transform.localScale.z );
		frontCollider.transform.localPosition = new Vector3( ( frontCollider.transform.localScale.x + carLength ) / 2 + 0.1f, 0, 0 );
		forwardCollider.transform.localScale = new Vector3( GetBrakingDistance( GetStrongBrakingSpeed(), rb.velocity.x, 0 ) * currAttribute.ForwardBufferMultiplier, forwardCollider.transform.localScale.y, forwardCollider.transform.localScale.z );
		forwardCollider.transform.localPosition = new Vector3( ( forwardCollider.transform.localScale.x + carLength ) / 2 + 0.1f, 0, 0 );

		leftFrontTurnCollider.transform.localScale = new Vector3( frontCollider.transform.localScale.x * currAttribute.LaneChangeBufferMultiplier, leftFrontTurnCollider.transform.localScale.y, leftFrontTurnCollider.transform.localScale.z );
		leftFrontTurnCollider.transform.localPosition = new Vector3( leftFrontTurnCollider.transform.localScale.x / 2, leftFrontTurnCollider.transform.localPosition.y, leftFrontTurnCollider.transform.localPosition.z );
		rightFrontTurnCollider.transform.localScale = new Vector3( frontCollider.transform.localScale.x * currAttribute.LaneChangeBufferMultiplier, rightFrontTurnCollider.transform.localScale.y, rightFrontTurnCollider.transform.localScale.z );
		rightFrontTurnCollider.transform.localPosition = new Vector3( rightFrontTurnCollider.transform.localScale.x / 2, rightFrontTurnCollider.transform.localPosition.y, rightFrontTurnCollider.transform.localPosition.z );
	}

	/// <summary>
	/// Eliminates velocity that isn't in the direction of travel (car is turning).
	/// </summary>
	void LateralDrag () {
		float localYVelocity = transform.InverseTransformDirection( rb.velocity ).y;
		if ( localYVelocity > 1 || localYVelocity < -1 ) {
			Vector3 localVelocity = transform.InverseTransformDirection( rb.velocity );
			localVelocity = new Vector3( localVelocity.x, 0, 0 );
			rb.velocity = transform.TransformDirection( localVelocity );
		} else if ( localYVelocity != 0 ) {
			int mod = ( localYVelocity > 0 ? -1 : 0 );
			rb.AddRelativeForce( 0, mod * maxBrakingSpeed, 0 );
		}

		if ( rb.velocity.z != 0 ) {
			Debug.Log( "z velocity exists" );
		}
	}

	/// <summary>
	/// Brakes fully and comes to a complete stop. 
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter ( Collision collision ) {
		if ( collision.gameObject.CompareTag( "Car" ) ) {
			currBehavior = new EmergencyBrakingBehavior( gameObject, null, null );
		} else {
			Debug.Log( "Collision with " + collision.gameObject.name );
		}
	}

	#region Get Braking Speeds and Distance
	private float GetLightBrakingSpeed () {
		return this.maxBrakingSpeed / 10.0f;
	}

	private float GetAverageBrakingSpeed () {
		return this.maxBrakingSpeed / 5.0f;
	}

	private float GetStrongBrakingSpeed () {
		return this.maxBrakingSpeed / 2.0f;
	}

	private static float GetBrakingDistance ( float brakingSpeed, float initialSpeed, float finalSpeed ) {
		/*
		float brakeTime = ( initialSpeed - finalSpeed ) / brakingSpeed;
		return brakingSpeed * brakeTime * brakeTime + ( initialSpeed - finalSpeed ) * brakeTime;
		*/
		// http://www.webpages.uidaho.edu/niatt_labmanual/Chapters/geometricdesign/theoryandconcepts/BrakingDistance.htm
		return ( initialSpeed * initialSpeed - finalSpeed * finalSpeed ) / ( 2 * brakingSpeed );
	}
	#endregion

	/// <summary>
	/// Brakes the car at the specified acceleration. 
	/// </summary>
	/// <param name="brakingSpeed">The magnitude of the desired deceleration (works against any x velocity)</param>
	public void Brake ( float brakingSpeed ) {
		if ( rb.velocity.magnitude < 2 ) {
			rb.velocity = new Vector3( 0, 0, 0 );
		} else {
			int mod = ( rb.velocity.x > 0 ? -1 : ( rb.velocity.x < 0 ? 1 : 0 ) );
			rb.AddRelativeForce( mod * brakingSpeed / ( rb.velocity.x < brakingSpeed ? ( rb.velocity.x < brakingSpeed / 10 ? 5 : 2 ) : 1 ), 0, 0, ForceMode.Acceleration );
		}
	}

	/// <summary>
	/// Accelerates the Car. Not sure if I should allow for varying accelerations....
	/// </summary>
	public void Accelerate () {
		rb.AddRelativeForce( ( preferredAcceleration < maxPositiveAcceleration ? preferredAcceleration : maxPositiveAcceleration ), 0, 0, ForceMode.Acceleration );
	}

	// Use these to set the speed/acceleration/buffers of the different aggressions
	#region Driving Attributes
	/// <summary>
	/// I might try to find a better way to do this. For now, it's what I'm using.
	/// </summary>
	public abstract class DrivingAttributes {
		public abstract float FrontBufferMultiplier {
			get;
		}

		public abstract float ForwardBufferMultiplier {
			get;
		}

		public abstract float MergeBufferMultiplier {
			get;
		}

		public abstract float LaneChangeBufferMultiplier {
			get;
		}

		public abstract float SpeedBuffer {
			get;
		}
	}

	public class EasyGoingAttributes : DrivingAttributes {
		public override float FrontBufferMultiplier {
			get {
				return 2.0f;
			}
		}

		public override float ForwardBufferMultiplier {
			get {
				return 2.0f;
			}
		}

		public override float MergeBufferMultiplier {
			get {
				return 2.0f;
			}
		}

		public override float LaneChangeBufferMultiplier {
			get {
				return 2.0f;
			}
		}

		public override float SpeedBuffer {
			get {
				return 5.0f;
			}
		}
	}

	public class ModerateAttributes : DrivingAttributes {
		public override float FrontBufferMultiplier {
			get {
				return 1.5f;
			}
		}

		public override float ForwardBufferMultiplier {
			get {
				return 1.5f;
			}
		}

		public override float MergeBufferMultiplier {
			get {
				return 1.5f;
			}
		}

		public override float LaneChangeBufferMultiplier {
			get {
				return 1.5f;
			}
		}

		public override float SpeedBuffer {
			get {
				return 2.5f;
			}
		}
	}

	public class AggressiveAttributes : DrivingAttributes {
		public override float FrontBufferMultiplier {
			get {
				return 1.1f;
			}
		}

		public override float ForwardBufferMultiplier {
			get {
				return 1.1f;
			}
		}

		public override float MergeBufferMultiplier {
			get {
				return 1.1f;
			}
		}

		public override float LaneChangeBufferMultiplier {
			get {
				return 1.1f;
			}
		}

		public override float SpeedBuffer {
			get {
				return 1.0f;
			}
		}
	}

	#endregion

	#region Driving Behaviors 
	// Complete
	/// <summary>
	/// <para>Abstract class for the various driving behaviors.</para>
	/// <para>The CarController script will simply call ExecuteAction() on the currently active behavior. </para>
	/// </summary>
	public abstract class DrivingBehavior {
		public GameObject parent;
		public CarController parentScript;

		public DrivingBehavior ( GameObject parent ) {
			this.parent = parent;
			this.parentScript = parent.GetComponent<CarController>();
		}

		/// <summary>
		/// Overridden by each DrivingBehavior extension to perform its individual action. 
		/// </summary>
		public abstract void ExecuteAction ();
	}

	// Unfinished
	/// <summary>
	/// <para>Controls the Car's behavior while it is driving unobstructed.</para>
	/// <para></para>
	/// </summary>
	public class CruisingBehavior : DrivingBehavior {

		public CruisingBehavior ( GameObject parent ) : base( parent ) {

		}

		/// <summary>
		/// Accelerates to desired speed and maintain lane. 
		/// </summary>
		public override void ExecuteAction () {
			if ( base.parentScript.frontCollider.GetComponent<ProximityZoneController>().containedCars.Count > 0 ) {
				GameObject object1 = (GameObject)base.parentScript.frontCollider.GetComponent<ProximityZoneController>().containedCars[0];
				HandleObstruction();
				return;
			}
			Drive();
		}

		/// <summary>
		/// Determines how to avoid an obstacle when it is present.
		/// <para>If collision is imminent, max brakes.</para> 
		/// <para>If it's a car moving slowly, and passing is available, pass.</para> (Unfinished)
		/// <para>If it's a car moving slowly, and passing is unavailable, slow down.</para> (Unfinished)
		/// <para>If it's a car moving slightly slower, match speed.</para> (Unfinished)
		/// <para>If it's a car moving faster, do nothing.</para> (Unfinished)
		/// <para>If it's the end of the exit, and car is exiting, do nothing.</para> (Unfinished)
		/// <para>If it's the end of the lane, change lanes.</para> (Unfinished)
		/// </summary>
		void HandleObstruction () {
			Ray ray = new Ray( new Vector3( parent.transform.position.x + 1, parent.transform.position.y, parent.transform.position.z ), parent.transform.right );
			RaycastHit forwardContact;
			if ( Physics.Raycast( ray, out forwardContact, LayerMask.GetMask( "Cars", "Road Indicators" ) ) ) {

				if ( forwardContact.transform.gameObject.CompareTag( "Car" ) ) {
					float distance = forwardContact.distance;
					GameObject obstacle = forwardContact.transform.gameObject;
					float obstacleVelocity = obstacle.GetComponent<CarController>().rb.velocity.magnitude;
					if ( obstacleVelocity > parentScript.preferredSpeed ) {
						if ( !parentScript.forwardCollider.GetComponent<ProximityZoneController>().containedCars.Contains( obstacle ) ) {
							Drive();
						} else {
							// Coast
						}
					} else if (obstacleVelocity > parentScript.preferredSpeed - parentScript.currAttribute.SpeedBuffer) {
						parentScript.currBehavior = new PacingBehavior( parent, this, obstacle );
					} else {
						parentScript.currBehavior = new PassingBehavior( parent, this, obstacle );
					}
					/*
					if ( CarController.GetBrakingDistance( parentScript.GetStrongBrakingSpeed(), parentScript.rb.velocity.magnitude, forwardContact.rigidbody.velocity.magnitude ) > distance + parentScript.forwardCollider.transform.localScale.x ) {
						parentScript.currBehavior = new EmergencyBrakingBehavior( parent, new PassingBehavior( parent, this, obstacle ), obstacle );
					} else if ( CarController.GetBrakingDistance( parentScript.GetAverageBrakingSpeed(), parentScript.rb.velocity.magnitude, forwardContact.rigidbody.velocity.magnitude ) > distance + parentScript.forwardCollider.transform.localScale.x ) {
						parentScript.currBehavior = new PassingBehavior( parent, this, obstacle );
					} else if ( CarController.GetBrakingDistance( parentScript.GetLightBrakingSpeed(), parentScript.rb.velocity.magnitude, forwardContact.rigidbody.velocity.magnitude ) > distance + parentScript.forwardCollider.transform.localScale.x ) {
						parentScript.currBehavior = new PacingBehavior( parent, new PassingBehavior( parent, this, obstacle ), obstacle );
					} else {
						parentScript.currBehavior = new PacingBehavior( parent, this, obstacle );
					}
					*/
				} else {
					// Road obstacles
				}
			}
		}

		void Drive () {
			if ( parentScript.rb.velocity.x < parentScript.preferredSpeed ) {
				parentScript.Accelerate();
			} else {
				// Maintain speed
			}
		}
	}

	// Not started
	/// <summary>
	/// Maintains the speed of the car in front. Assumes that the preceding car is moving slower than the desired speed of this car.
	/// </summary>
	public class PacingBehavior : DrivingBehavior {
		private GameObject target;
		private DrivingBehavior parentBehavior;
		ProximityZoneController frontColliderController;

		public PacingBehavior ( GameObject parent, DrivingBehavior parentBehavior, GameObject target ) : base( parent ) {
			this.target = target;
			this.parentBehavior = parentBehavior;
			this.frontColliderController = parentScript.frontCollider.GetComponent<ProximityZoneController>();
		}

		public override void ExecuteAction () {
			if ( !frontColliderController.containedCars.Contains( target ) ) {
				// If the target car is no longer in the front collider
				ObstructionCleared();
				return;
			}
			float targetVelocity = target.GetComponent<CarController>().rb.velocity.x;
			if ( targetVelocity < parentScript.preferredSpeed - parentScript.currAttribute.SpeedBuffer ) {
				// Target is going too slow, pass
				parentScript.currBehavior = new PassingBehavior( parent, parentBehavior, target );
			} else if ( targetVelocity > parentScript.preferredSpeed ) {
				// Target is going too fast, revert to cruising
				ObstructionCleared();
			} else {
				// Pace target
				if ( parentScript.rb.velocity.x < targetVelocity ) {
					parentScript.Accelerate();
				} else {
					// Maintain speed
				}
			}
		}

		private void ObstructionCleared () {
			parentScript.currBehavior = parentBehavior;
		}
	}

	// Not started
	/// <summary>
	/// Passes the target car. When first called, it will determine if the car should be 
	/// passed on the left or the right. After the other car has been passed (current car 
	/// is ahead of target car), the car will return to cruising.
	/// </summary>
	public class PassingBehavior : DrivingBehavior {
		private GameObject target;
		private DrivingBehavior parentBehavior;

		public PassingBehavior ( GameObject parent, DrivingBehavior parentBehavior, GameObject target ) : base( parent ) {
			this.target = target;
			this.parentBehavior = parentBehavior;
		}

		public override void ExecuteAction () {
			print( "passing" );
		}

		private void ObstructionCleared () {

		}
	}

	// Not started
	/// <summary>
	/// Changes lanes more casually than the passing behavior does. 
	/// </summary>
	public class LaneChangeBehavior : DrivingBehavior {

		public LaneChangeBehavior ( GameObject parent ) : base( parent ) {

		}

		public override void ExecuteAction () {
			print( "Changing Lanes" );
		}
	}

	// Not started
	/// <summary>
	/// Merges between two cars/ ahead of a car/ behind a car of similar speeds
	/// </summary>
	public class MergingBehavior : DrivingBehavior {

		private GameObject mergeAhead;
		private GameObject mergeBehind;

		/// <summary>
		/// mergeBehind and mergeAhead can be null, and will be handled that way.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="mergeBehind"></param>
		/// <param name="mergeAhead"></param>
		public MergingBehavior ( GameObject parent, GameObject mergeBehind, GameObject mergeAhead ) : base( parent ) {

		}

		public override void ExecuteAction () {

		}
	}

	// Not started
	/// <summary>
	/// Navigates the car to the appropriate exit lane and maintains speed to exit. 
	/// </summary>
	public class ExitingBehavior : DrivingBehavior {
		private GameObject destination;
		private DrivingBehavior parentBehavior;

		public ExitingBehavior ( GameObject parent, DrivingBehavior parentBehavior, GameObject destination ) : base( parent ) {
			this.destination = destination;
			this.parentBehavior = parentBehavior;
		}

		public override void ExecuteAction () {
			Ray ray = new Ray( parent.transform.position, parent.transform.right );
			LayerMask layerMask = LayerMask.GetMask( "RoadIndicators" );
			RaycastHit hitInfo;
			if ( parent.transform.rotation == Quaternion.identity && Physics.Raycast( ray, out hitInfo, layerMask ) && hitInfo.transform.gameObject.Equals( destination ) ) {
				parentScript.currBehavior = parentBehavior;
			} else {
				// Navigate to destination 
			}
		}
	}

	// Complete
	/// <summary>
	/// Brakes the maximum amount to attempt to avoid an obstacle. Will look for a lane change if available. 
	/// </summary>
	public class EmergencyBrakingBehavior : DrivingBehavior {
		private GameObject obstacle;
		private DrivingBehavior parentBehavior;

		public EmergencyBrakingBehavior ( GameObject parent ) : base( parent ) {

		}

		public EmergencyBrakingBehavior ( GameObject parent, DrivingBehavior parentBehavior, GameObject obstacle ) : base( parent ) {
			this.obstacle = obstacle;
			this.parentBehavior = parentBehavior;
		}

		public override void ExecuteAction () {
			if ( obstacle != null && parentBehavior != null ) {
				if ( CarController.GetBrakingDistance( parentScript.GetStrongBrakingSpeed(), parentScript.rb.velocity.x, ( obstacle.CompareTag( "Car" ) ? obstacle.GetComponent<CarController>().rb.velocity.x : 0 ) ) < Vector3.Distance( base.parent.transform.position, obstacle.transform.position ) + parentScript.forwardCollider.transform.localScale.x ) {
					parentScript.currBehavior = parentBehavior;
					return;
				}
			}
			parentScript.Brake( parentScript.maxBrakingSpeed );
		}
	}
	#endregion
}
