using UnityEngine;
using System.Collections;

/* A helper class for steering a game object in 2D */
public class SteeringUtils : MonoBehaviour {

	public float maxVelocity = 3;

	/* The maximum acceleration */
	public float maxAcceleration = 4;

	/* Updates the velocity of the current game object by the given linear acceleration */
	public void steer(Vector2 linearAcceleration) {
		GetComponent<Rigidbody2D>().velocity += linearAcceleration * Time.fixedDeltaTime;
		
		if (GetComponent<Rigidbody2D>().velocity.magnitude > maxVelocity) {
			GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * maxVelocity;
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
		Vector2 direction = GetComponent<Rigidbody2D>().velocity.normalized;
		
		// If we have a non-zero velocity then look towards where we are moving otherwise do nothing
		if (GetComponent<Rigidbody2D>().velocity.sqrMagnitude > 0.001) {
			float toRotation = (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);
			float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, toRotation, Time.fixedDeltaTime*5);
			
			transform.rotation = Quaternion.Euler(0, 0, rotation);
		}
	}
}