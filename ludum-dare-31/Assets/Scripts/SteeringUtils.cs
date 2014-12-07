using UnityEngine;
using System.Collections;

/* A helper class for steering a game object in 2D */
public class SteeringUtils : MonoBehaviour {
	
	public float maxVelocity = 3;
	
	/* The maximum acceleration */
	public float maxAcceleration = 4;

	/* The radius from the target that means we are close enough and have arrived */
	public float targetRadius = 0.005f;
	
	/* The radius from the target where we start to slow down  */
	public float slowRadius = 1f;
	
	/* The time in which we want to achieve the targetSpeed */
	public float timeToTarget = 0.1f;
	
	/* Updates the velocity of the current game object by the given linear acceleration */
	public void steer(Vector2 linearAcceleration) {
		rigidbody2D.velocity += linearAcceleration * Time.fixedDeltaTime;
		
		if (rigidbody2D.velocity.magnitude > maxVelocity) {
			rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxVelocity;
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
		Vector2 direction = rigidbody2D.velocity.normalized;
		
		// If we have a non-zero velocity then look towards where we are moving otherwise do nothing
		if (rigidbody2D.velocity.sqrMagnitude > 0.001) {
			float toRotation = (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);
			float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, toRotation, Time.fixedDeltaTime*5);
			
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
			rigidbody2D.velocity = Vector2.zero;
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
		Vector3 acceleration = targetVelocity - new Vector3(rigidbody2D.velocity.x, rigidbody2D.velocity.y, 0);
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
}