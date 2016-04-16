﻿using UnityEngine;

/* A helper class for steering a game object in 2D */
using System.Collections.Generic;

[RequireComponent (typeof (MovementAIRigidbody))]
public class SteeringBasics : MonoBehaviour {
    [Header("General")]

    public float maxVelocity = 3.5f;
	
	public float maxAcceleration = 10f;

    public float turnSpeed = 20f;


    [Header("Arrive")]

    /* The radius from the target that means we are close enough and have arrived */
    public float targetRadius = 0.005f;
	
	/* The radius from the target where we start to slow down  */
	public float slowRadius = 1f;
	
	/* The time in which we want to achieve the targetSpeed */
	public float timeToTarget = 0.1f;


    [Header("Look Direction Smoothing")]

    /* Smoothing controls if the character's look direction should be an average of its previous directions (to smooth out momentary changes in directions) */
    public bool smoothing = true;
	public int numSamplesForSmoothing = 5;
	private Queue<Vector3> velocitySamples = new Queue<Vector3>();


    private MovementAIRigidbody rb;


    void Awake()
    {
        rb = GetComponent<MovementAIRigidbody>();
    }

    /* Updates the velocity of the current game object by the given linear acceleration */
    public void steer(Vector3 linearAcceleration) {
        rb.velocity += linearAcceleration * Time.deltaTime;

        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    /* A seek steering behavior. Will return the steering for the current game object to seek a given position */
    public Vector3 seek(Vector3 targetPosition, float maxSeekAccel) {
		//Get the direction
		Vector3 acceleration = rb.convertVector(targetPosition - transform.position);
		
		acceleration.Normalize ();
		
		//Accelerate to the target
		acceleration *= maxSeekAccel;
		
		return acceleration;
	}

    public Vector3 seek(Vector3 targetPosition)
    {
        return seek(targetPosition, maxAcceleration);
    }

    /* Makes the current game object look where he is going */
    public void lookWhereYoureGoing() {
		Vector3 direction = rb.velocity;

		if (smoothing) {
			if (velocitySamples.Count == numSamplesForSmoothing) {
				velocitySamples.Dequeue ();
			}

			velocitySamples.Enqueue (rb.velocity);

			direction = Vector3.zero;

			foreach (Vector3 v in velocitySamples) {
				direction += v;
			}

			direction /= velocitySamples.Count;
		}

		lookAtDirection (direction);
	}

	public void lookAtDirection(Vector3 direction) {
		direction.Normalize();
		
		// If we have a non-zero direction then look towards that direciton otherwise do nothing
		if (direction.sqrMagnitude > 0.001f) {
            if(rb.is3D)
            {
                // Mulitply by -1 because counter clockwise on the y-axis is in the negative direction
                float toRotation = -1*(Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg);
                float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, toRotation, Time.deltaTime * turnSpeed);

                transform.rotation = Quaternion.Euler(0, rotation, 0);
            } else {
                float toRotation = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
                float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, toRotation, Time.deltaTime * turnSpeed);

                transform.rotation = Quaternion.Euler(0, 0, rotation);
            }
		}
	}

    public void lookAtDirection(Quaternion toRotation)
    {
        if(rb.is3D)
        {
            lookAtDirection(toRotation.eulerAngles.y);
        } else
        {
            lookAtDirection(toRotation.eulerAngles.z);
        }
    }

    /// <summary>
    /// Makes the character's rotation lerp closer to the given target rotation (in degrees).
    /// </summary>
    /// <param name="toRotation">the desired rotation to be looking at in degrees</param>
    public void lookAtDirection(float toRotation)
    {
        if (rb.is3D)
        {
            float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, toRotation, Time.deltaTime * turnSpeed);

            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        else
        {
            float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, toRotation, Time.deltaTime * turnSpeed);

            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
    }

    /* Returns the steering for a character so it arrives at the target */
    public Vector3 arrive(Vector3 targetPosition) {
        Debug.DrawLine(transform.position, targetPosition, Color.cyan, 0f, false);

        targetPosition = rb.convertVector(targetPosition);

        /* Get the right direction for the linear acceleration */
        Vector3 targetVelocity = targetPosition - rb.position;
        //Debug.Log("Displacement " + targetVelocity.ToString("f4"));
		
		/* Get the distance to the target */
		float dist = targetVelocity.magnitude;

        /* If we are within the stopping radius then stop */
        if (dist < targetRadius) {
            rb.velocity = Vector3.zero;
			return Vector3.zero;
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
		Vector3 acceleration = targetVelocity - rb.velocity;
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
        //Debug.Log("Accel " + acceleration.ToString("f4"));
		return acceleration;
	}

    static float quadForm(float a, float b, float c, bool pos)
    {
        float preRoot = b * b - 4 * a * c;
        if (preRoot < 0)
        {
            return float.NaN;
        }
        else
        {
            float sgn = pos ? 1.0f : -1.0f;
            return (sgn * Mathf.Sqrt(preRoot) - b) / (2.0f * a);
        }
    }

    public Vector3 interpose(MovementAIRigidbody target1, MovementAIRigidbody target2)
    {
        Vector3 midPoint = (target1.position + target2.position) / 2;

        float timeToReachMidPoint = Vector3.Distance(midPoint, transform.position) / maxVelocity;

        Vector3 futureTarget1Pos = target1.position + target1.velocity * timeToReachMidPoint;
        Vector3 futureTarget2Pos = target2.position + target2.velocity * timeToReachMidPoint;

        midPoint = (futureTarget1Pos + futureTarget2Pos) / 2;

        return arrive(midPoint);
    }

    /* Checks to see if the target is in front of the character */
    public bool isInFront(Vector3 target)
    {
        return isFacing(target, 0);
    }

    public bool isFacing(Vector3 target, float cosineValue) { 
        Vector3 facing = transform.right.normalized;

        Vector3 directionToTarget = (target - transform.position);
        directionToTarget.Normalize();

        return Vector3.Dot(facing, directionToTarget) >= cosineValue;
    }

    /// <summary>
    /// Returns the given orientation (in radians) as a unit vector
    /// </summary>
    /// <param name="orientation">the orientation in radians</param>
    /// <param name="is3DGameObj">is the orientation for a 3D game object or a 2D game object</param>
    /// <returns></returns>
    public static Vector3 orientationToVector(float orientation, bool is3DGameObj)
    {
        if(is3DGameObj)
        {
            // Mulitply the orientation by -1 because counter clockwise on the y-axis is in the negative
            // direction, but Cos And Sin expect clockwise orientation to be the positive direction
            return new Vector3(Mathf.Cos(-orientation), 0, Mathf.Sin(-orientation));
        } else
        {
            return new Vector3(Mathf.Cos(orientation), Mathf.Sin(orientation), 0);
        }
    }

    /// <summary>
    /// Gets the orientation of a vector as radians. For 3D it gives the orienation around the Y axis.
    /// For 2D it gaves the orienation around the Z axis.
    /// </summary>
    /// <param name="direction">the direction vector</param>
    /// <param name="is3DGameObj">is the direction vector for a 3D game object or a 2D game object</param>
    /// <returns>orientation in radians</returns>
    public static float vectorToOrientation(Vector3 direction, bool is3DGameObj)
    {
        if (is3DGameObj)
        {
            // Mulitply by -1 because counter clockwise on the y-axis is in the negative direction
            return -1*Mathf.Atan2(direction.z, direction.x);
        }
        else
        {
            return Mathf.Atan2(direction.y, direction.x);
        }
    }

    /// <summary>
    /// Creates a debug cross at the given position in the scene view to help with debugging.
    /// </summary>
    public static void debugCross(Vector3 position, float size = 0.5f, Color color = default(Color), float duration = 0f, bool depthTest = true)
    {
        Vector3 xStart = position + Vector3.right * size * 0.5f;
        Vector3 xEnd = position - Vector3.right * size * 0.5f;

        Vector3 yStart = position + Vector3.up * size * 0.5f;
        Vector3 yEnd = position - Vector3.up * size * 0.5f;

        Vector3 zStart = position + Vector3.forward * size * 0.5f;
        Vector3 zEnd = position - Vector3.forward * size * 0.5f;

        Debug.DrawLine(xStart, xEnd, color, duration, depthTest);
        Debug.DrawLine(yStart, yEnd, color, duration, depthTest);
        Debug.DrawLine(zStart, zEnd, color, duration, depthTest);
    }
}