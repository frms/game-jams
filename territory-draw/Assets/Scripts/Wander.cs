using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour {
		
	/* The forward offset of the wander square */
	public float wanderOffset = 4;
	
	/* The radius of the wander square */
	public float wanderRadius = 4;
	
	/* The rate at which the wander orientation can change */
	public float wanderRate = 0.4f;

	private float wanderOrientation = 0;

	private GameObject debugRing;

	private SteeringUtils steeringUtils;


	public float rayDistance = 1.5f;
	public float sameDecisionDuration = 3;
	
	private float lastDecisionTime = Mathf.NegativeInfinity;
	private int decision;

	void Start() {
		DebugDraw debugDraw = gameObject.GetComponent<DebugDraw> ();
		debugRing = debugDraw.createRing (Vector3.zero, wanderRadius);

		steeringUtils = gameObject.GetComponent<SteeringUtils> ();
	}

	void FixedUpdate () {

		float characterOrientation = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;


		/* Update the wander orientation */
		Vector3 dir = orientationToVector (characterOrientation);
		
		Vector3 end = transform.position + dir * rayDistance;
		Debug.DrawLine (transform.position, end, Color.green);
		
		RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayDistance);
		/* If we collide with the wall then wander away from it */
		if (hit.collider != null){

			if(lastDecisionTime + sameDecisionDuration < Time.time) {
				int decision = (int)(Random.value * 2) - 1;
				wanderOrientation += decision * wanderRate * 2;
				lastDecisionTime = Time.time;
			}
		} else {
			wanderOrientation += randomBinomial() * wanderRate;
		}

		
		
		/* Calculate the combined target orientation */
		float targetOrientation = wanderOrientation + characterOrientation;
		
		/* Calculate the center of the wander circle */
		Vector3 targetPosition = transform.position + (orientationToVector (characterOrientation) * wanderOffset);

		debugRing.transform.position = targetPosition;
		
		/* Calculate the target position */
		targetPosition = targetPosition + (orientationToVector(targetOrientation) * wanderRadius);

		Debug.DrawLine (transform.position, targetPosition);


		Vector2 acceleration = steeringUtils.seek (targetPosition);

		steeringUtils.steer (acceleration);
	
		steeringUtils.lookWhereYoureGoing ();

	}

	/* Returns a random number between -1 and 1. Values around zero are more likely. */
	float randomBinomial() {
		return Random.value - Random.value;
	}
	
	/* Returns the orientation as a unit vector */
	Vector3 orientationToVector(float orientation) {
		return new Vector3(Mathf.Cos(orientation), Mathf.Sin(orientation), 0);
	}
}
