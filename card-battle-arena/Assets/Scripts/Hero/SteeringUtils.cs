using UnityEngine;
using System.Collections;

/* A helper class for steering a game object in 2D */
using System.Collections.Generic;


[RequireComponent (typeof (Rigidbody2D))]
public class SteeringUtils : MonoBehaviour {
	
	public float maxVelocity = 3;
	
	/* The maximum acceleration */
	public float maxAcceleration = 4;

	/* The radius from the target that means we are close enough and have arrived */
	public float targetRadius = 0.05f;
	
	/* The radius from the target where we start to slow down  */
	public float slowRadius = 1f;
	
	/* The time in which we want to achieve the targetSpeed */
	public float timeToTarget = 0.1f;

	public float turnSpeed = 20f;

	private Rigidbody2D rb;

	public bool smoothing = true;
	public int numSamplesForSmoothing = 5;
	private Queue<Vector2> velocitySamples;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		velocitySamples = new Queue<Vector2> ();
	}
	
	/* Updates the velocity of the current game object by the given linear acceleration */
	public void steer(Vector2 linearAcceleration) {
		rb.velocity += linearAcceleration * Time.deltaTime;
		
		if (rb.velocity.magnitude > maxVelocity) {
			rb.velocity = rb.velocity.normalized * maxVelocity;
		}
	}

	/* Calls the normal Vector2 linear acceleration */
	public void steer(Vector3 linearAcceleration) {
		this.steer (new Vector2 (linearAcceleration.x, linearAcceleration.y));
	}
	
	/* A seek steering behavior. Will return the steering for the current game object to seek a given position */
	public Vector2 seek(Vector3 targetPosition) {
		//Get the direction
		Vector3 acceleration = targetPosition - transform.position;
		
		//Remove the z coordinate
		acceleration.z = 0;
		
		acceleration.Normalize ();
		
		//Accelerate to the target
		acceleration *= maxAcceleration;
		
		return acceleration;
	}
	
	/* Makes the current game object look where he is going */
	public void lookWhereYoureGoing() {
		Vector2 direction = rb.velocity;

		if (smoothing) {
			if (velocitySamples.Count == numSamplesForSmoothing) {
				velocitySamples.Dequeue ();
			}

			velocitySamples.Enqueue (rb.velocity);

			direction = Vector2.zero;

			foreach (Vector2 v in velocitySamples) {
				direction += v;
			}

			direction /= velocitySamples.Count;
		}

		lookAtDirection (direction);
	}

	public void lookAtDirection(Vector2 direction) {
		direction.Normalize();
		
		// If we have a non-zero direction then look towards that direciton otherwise do nothing
		if (direction.sqrMagnitude > 0.001) {
			float toRotation = (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);
			float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, toRotation, Time.deltaTime*turnSpeed);
			
			transform.rotation = Quaternion.Euler(0, 0, rotation);
		}
	}

	/* Returns the steering for a character so it arrives at the target */
	public Vector2 arrive(Vector3 targetPosition) {
		/* Get the right direction for the linear acceleration */
		Vector3 targetVelocity = targetPosition - transform.position;

		// Remove the z coordinate
		targetVelocity.z = 0;
		
		/* Get the distance to the target */
		float dist = targetVelocity.magnitude;
		
		/* If we are within the stopping radius then stop */
		if(dist < targetRadius) {
			rb.velocity = Vector2.zero;
			return Vector2.zero;
		}
		
		/* Calculate the target speed, full speed at slowRadius distance and 0 speed at 0 distance */
		float targetSpeed;
		if(dist > slowRadius) {
			targetSpeed = maxVelocity;
		} else {
			targetSpeed = maxVelocity * (dist / slowRadius);
		}
		
		/* Give targetVelocity the correct speed */
		targetVelocity.Normalize();
		targetVelocity *= targetSpeed;
		
		/* Calculate the linear acceleration we want */
		Vector3 acceleration = targetVelocity - new Vector3(rb.velocity.x, rb.velocity.y, 0);
		/*
		 Rather than accelerate the character to the correct speed in 1 second, 
		 accelerate so we reach the desired speed in timeToTarget seconds 
		 (if we were to actually accelerate for the full timeToTarget seconds).
		*/
		acceleration *= 1/timeToTarget;
		
		/* Make sure we are accelerating at max acceleration */
		if(acceleration.magnitude > maxAcceleration) {
			acceleration.Normalize();
			acceleration *= maxAcceleration;
		}

		return acceleration;
	}
	
	/* The maximum acceleration for separation */
	public float sepMaxAcceleration = 10;
	public float sepThreshold = 1.6666f;

	public Vector2 separation(HashSet<Transform> targets) {
		Vector2 acceleration = Vector2.zero;
		
		foreach(Transform t in targets) {
			// Skip non heroes 
			if(t.gameObject.layer != GameManager.heroLayer) {
				continue;
			}

			/* Get the direction and distance from the target */
			Vector2 direction = transform.position - t.position;
			float dist = direction.magnitude;

			if(dist < sepThreshold) {
				/* Calculate the separation strength (can be changed to use inverse square law rather than linear) */
				var strength = sepMaxAcceleration * (sepThreshold - direction.magnitude) / sepThreshold;
				
				/* Added separation acceleration to the existing steering */
				direction.Normalize();
				acceleration += direction * strength;
			}
		}

		return acceleration;
	}

	public float collAvoidRadius = 0.5f;

	public Vector2 collisionAvoidance(HashSet<Transform> targets, Transform ignoreUnit) {
		Vector2 acceleration = Vector2.zero;
		
		/* 1. Find the target that the character will collide with first */
		
		/* The first collision time */
		float shortestTime = float.PositiveInfinity;
		
		/* The first target that will collide and other data that
		 * we will need and can avoid recalculating */
		Transform firstTarget = null;
		float firstMinSeparation = 0, firstDistance = 0;
		Vector3 firstRelativePos = Vector3.zero, firstRelativeVel = Vector3.zero;
		
		foreach(Transform t in targets) {
			// Skip non heroes and the given ignore unit
			if(t.gameObject.layer != GameManager.heroLayer || t == ignoreUnit) {
				continue;
			}

			Vector2 targetVel = t.GetComponent<Rigidbody2D>().velocity;
			
			/* Calculate the time to collision */
			Vector3 relativePos = transform.position - t.position;
			Vector3 relativeVel = rb.velocity - targetVel;
			float distance = relativePos.magnitude;
			float relativeSpeed = relativeVel.magnitude;
			
			if(relativeSpeed == 0) {
				continue;
			}
			
			float timeToCollision = -1*Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);
			
			/* Check if they will collide at all */
			Vector3 separation = relativePos + relativeVel*timeToCollision;
			
			float minSeparation = separation.magnitude;
			if(minSeparation > 2*collAvoidRadius) {
				continue;
			}
			
			/* Check if its the shortest */
			if(timeToCollision > 0 && timeToCollision < shortestTime) {
				shortestTime = timeToCollision;
				firstTarget = t;
				firstMinSeparation = minSeparation;
				firstDistance = distance;
				firstRelativePos = relativePos;
				firstRelativeVel = relativeVel;
			}
		}
		
		/* 2. Calculate the steering */
		
		/* If we have no target then exit */
		if(firstTarget == null) {
			return acceleration;
		}
		
		/* If we are going to collide with no separation or if we are already colliding then 
		 * steer based on current position */
		if(firstMinSeparation <= 0 || firstDistance < 2*collAvoidRadius) {
			acceleration = transform.position - firstTarget.position;
		}
		/* Else calculate the future relative position */
		else {
			acceleration = firstRelativePos + firstRelativeVel * shortestTime;
		}
		
		/* Avoid the target */
		acceleration.Normalize();
		acceleration *= sepMaxAcceleration;
		
		return acceleration;
	}
}