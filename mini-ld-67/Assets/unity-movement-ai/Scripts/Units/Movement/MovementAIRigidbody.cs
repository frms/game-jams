﻿using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// This is a wrapper class for either a Rigidbody or Rigidbody2D, so that either can be used with the Unity Movement AI code. 
/// </summary>
public class MovementAIRigidbody : MonoBehaviour
{
    [Header("3D Settings")]

    /* Determines if the character should follow the ground or can fly any where in 3D space */
    public bool canFly = false;

    [Header("3D Grounded Settings")]

    /* If the character should try to stay grounded */
    public bool stayGrounded = true;

    /* How far the character should look below him for ground to stay grounded to */
    public float fooGroundFollowDistance = 0.1f;

    /* The sphere cast mask that determines what layers should be consider the ground */
    public LayerMask groundCheckMask = Physics.DefaultRaycastLayers;

    /* The maximum slope the character can climb in degrees */
    public float slopeLimit = 80f;

    private SphereCollider col3D;
    private CircleCollider2D col2D;

    /// <summary>
    /// The radius for the current game object (either the radius of a sphere or circle
    /// collider). If the game object does not have a sphere or circle collider this 
    /// will return -1.
    /// </summary>
    public float radius
    {
        get
        {
            if(col3D != null)
            {
                return Mathf.Max(rb3D.transform.localScale.x, rb3D.transform.localScale.y, rb3D.transform.localScale.z) * col3D.radius;
            }
            else if(col2D != null)
            {
                return Mathf.Max(rb2D.transform.localScale.x, rb2D.transform.localScale.y) * col2D.radius;
            }
            else
            {
                return -1;
            }
        }
    }

    [System.NonSerialized]
    public bool is3D;

    /// <summary>
    /// The current ground normal for this character. This value is only used by 3D 
    /// characters who cannot fly.
    /// </summary>
    [System.NonSerialized]
    public Vector3 wallNormal = Vector3.zero;

    /// <summary>
    /// The current movement plane normal for this character. This value is only
    /// used by 3D characters who cannot fly.
    /// </summary>
    [System.NonSerialized]
    public Vector3 movementNormal = Vector3.up;

    private Rigidbody rb3D;
    private Rigidbody2D rb2D;

    void Awake()
    {
        setUp();
    }

    /// <summary>
    /// Sets up the MovementAIRigidbody so it knows about its underlying collider and rigidbody.
    /// </summary>
    public void setUp()
    {
        setUpRigidbody();
        setUpCollider();
    }

    private void setUpRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            this.rb3D = rb;
            is3D = true;
        }
        else
        {
            this.rb2D = GetComponent<Rigidbody2D>();
            is3D = false;
        }
    }

    private void setUpCollider()
    {
        if (is3D)
        {
            SphereCollider col = rb3D.GetComponent<SphereCollider>();

            if (col != null)
            {
                col3D = col;
            }
        }
        else
        {
            CircleCollider2D col = rb2D.GetComponent<CircleCollider2D>();

            if (col != null)
            {
                col2D = col;
            }
        }
    }

    void Start()
    {
        StartCoroutine(debugDraw());

        /* Call fixed update for 3D grounded characters to make sure they get proper 
         * ground / movement normals before their velocity is set */
        FixedUpdate();

        Debug.Log(Vector3.Angle(Vector3.up, Vector3.zero));
    }

    private int countDebug = 0;

    private IEnumerator debugDraw()
    {
        yield return new WaitForFixedUpdate();

        Vector3 origin = colliderPosition;
        Debug.DrawLine(origin, origin + (velocity.normalized), Color.red, 0f, false);
        if(is3D)
        {
            Debug.DrawLine(origin, origin + (Vector3.ProjectOnPlane(velocity, movementNormal).normalized), Color.magenta, 0f, false);
            Debug.DrawLine(origin, origin + (realVelocity.normalized), Color.green, 0f, false);
            Debug.DrawLine(origin, origin + (wallNormal), Color.yellow, 0f, false);
            Debug.DrawLine(origin, origin + (movementNormal), Color.yellow, 0f, false);
        }

        //Debug.Log("waitforfixedupdate " + transform.position.ToString("f4"));
        //Debug.Log(rb3D.velocity.magnitude);
        //Debug.Log(movementNormal.ToString("f4"));
        //Debug.Log("--------------------------------------------------------------------------------");
        //SteeringBasics.debugCross(colliderPosition, 0.5f, Color.red, 0, false);

        countDebug = 0;
        StartCoroutine(debugDraw());
    }

    void FixedUpdate()
    {
        //Debug.Log("fixed " + count);
        /* If the character can't fly then find the current the ground normal */
        if (is3D && !canFly)
        {
            /* Reset to default values */
            wallNormal = Vector3.zero;
            movementNormal = Vector3.up;
            rb3D.useGravity = true;

            RaycastHit downHit;

            /* 
            Start the ray with a small offset of 0.1f from inside the character. The
            transform.position of the characer is assumed to be at the base of the character.
             */
            if (sphereCast(Vector3.down, out downHit, fooGroundFollowDistance, groundCheckMask.value))
            {
                if (isWall(downHit.normal))
                {
                    /* Get vector pointing down the wall */
                    Vector3 rightSlope = Vector3.Cross(downHit.normal, Vector3.down);
                    Vector3 downSlope = Vector3.Cross(rightSlope, downHit.normal);

                    float remainingDist = fooGroundFollowDistance - downHit.distance;

                    RaycastHit downWallHit;

                    /* If we found ground that we would have hit if not for the wall then follow it */
                    if (remainingDist > 0 && sphereCast(downSlope, out downWallHit, remainingDist, groundCheckMask.value, downHit.normal) && !isWall(downWallHit.normal))
                    {
                        Vector3 newPos = rb3D.position + (downSlope.normalized * downWallHit.distance);
                        foundGround(downWallHit.normal, newPos);
                    }

                    /* If we are close enough to the hit to be touching it then we are on the wall */
                    if (downHit.distance <= 0.01f)
                    {
                        wallNormal = downHit.normal;
                        //Debug.DrawRay(hitInfo.point, hitInfo.normal, new Color(1f, 0.38823f, 0.27843f), 1f, false);
                    }
                }
                /* Else we've found walkable ground */
                else
                {
                    Vector3 newPos = rb3D.position + (Vector3.down * downHit.distance);
                    foundGround(downHit.normal, newPos);
                    //SteeringBasics.debugCross(hitInfo.point + Vector3.up * (hitInfo.distance - 0.1f), 0.5f, Color.red, 0, false);
                }
            }

            limitMovementOnSteepSlopes();
        }
    }

    /* Make the spherecast offset slightly bigger than the max allowed collider overlap. This was
     * known as Physics.minPenetrationForPenalty and had a default value of 0.05f, but has since
     * been removed and supposedly replaced by Physics.defaultContactOffset/Collider.contactOffset.
     * My tests show that as of Unity 5.3.0f4 this is not %100 true and Unity still seems to be 
     * allowing overlaps of 0.05f somewhere internally. So I'm setting my spherecast offset to be
     * slightly bigger than 0.05f */
    private float spherecastOffset = 0.051f;

    private bool sphereCast(Vector3 dir, out RaycastHit hitInfo, float dist, int layerMask, Vector3 planeNormal = default(Vector3))
    {
        /* Make sure we use the collider's origin for our cast (which can be different
         * then the transform.position).
         *
         * Also if we are given a planeNormal then raise the origin a tiny amount away
         * from the plane to avoid problems when the given dir is just barely moving  
         * into the plane (this can occur due to floating point inaccuracies when the 
         * dir is calculated with cross products) */
        Vector3 origin = colliderPosition + (planeNormal * 0.001f);

        /* Start the ray with a small offset from inside the character, so it will
         * hit any colliders that the character is already touching. */
        origin += -spherecastOffset * dir;

        float maxDist = (spherecastOffset + dist);

        if (Physics.SphereCast(origin, radius, dir, out hitInfo, maxDist, layerMask))
        {
            /* Remove the small offset from the distance before returning*/
            hitInfo.distance -= spherecastOffset;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void foundGround(Vector3 normal, Vector3 newPos)
    {
        movementNormal = normal;
        rb3D.useGravity = false;
        rb3D.MovePosition(newPos);
        /* Reproject the velocity onto the ground plane in case the ground plane has changed this frame.
         * Make sure to multiple by the movement velocity's magnitude, rather than the actual velocity
         * since we could have been falling and now found ground so all the downward y velocity is not
         * part of our movement speed. Technically I am projecting the actual velocity onto the ground
         * plane rather than finding the real movement velocity's speed.*/
        rb3D.velocity = dirOnPlane(rb3D.velocity, movementNormal) * velocity.magnitude;
    }

    private bool isWall(Vector3 surfNormal)
    {
        /* If the normal of the surface is greater then our slope limit then its a wall */
        return Vector3.Angle(Vector3.up, surfNormal) > slopeLimit;
    }

    private void limitMovementOnSteepSlopes()
    {
        Vector3 startVelocity = rb3D.velocity;

        /* If we are currently on a wall then limit our movement */
        if (wallNormal != Vector3.zero && isMovingInto(rb3D.velocity, wallNormal))
        {
            rb3D.velocity = limitVelocityOnWall(rb3D.velocity, wallNormal);
        }
        /* Else we have no wall or we are moving away from the wall so we will no longer be touching it */
        else
        {
            wallNormal = Vector3.zero;
        }

        /* Check if we are moving into a wall */
        for (int i = 0; i < 2; i++)
        {
            Vector3 direction = rb3D.velocity.normalized;
            float dist = rb3D.velocity.magnitude * Time.deltaTime;

            Vector3 origin = colliderPosition;
            countDebug++;

            if (i == 0)
            {
                Debug.DrawRay(origin + Vector3.up * 0.05f * countDebug, direction, new Color(0.953f, 0.898f, 0.961f), 0f, false);
            }
            else if (i == 1)
            {
                Debug.DrawRay(origin + Vector3.up * 0.05f * countDebug, direction, new Color(0.612f, 0.153f, 0.69f), 0f, false);
            }
            else
            {
                Debug.DrawRay(origin + Vector3.up * 0.05f * countDebug, direction, new Color(0.29f, 0.078f, 0.549f), 0f, false);
            }

            RaycastHit hitInfo;

            /* Spherecast in the direction we are moving and check if we will hit a wall. Also check that we are
             * in fact moving into the wall (it seems that it is possible to clip the corner of a wall even 
             * though the char/spherecast is moving away from the wall) */
            if (sphereCast(direction, out hitInfo, dist, groundCheckMask.value) && isWall(hitInfo.normal)
                && isMovingInto(direction, hitInfo.normal))
            {
                Vector3 projectedVel = limitVelocityOnWall(rb3D.velocity, hitInfo.normal);
                Vector3 projectedStartVel = limitVelocityOnWall(startVelocity, hitInfo.normal);

                /* If we have a previous wall. And if the latest velocity is moving into the previous wall or if 
                 * our starting velocity projected onto this new wall is moving into the previous wall then stop
                 * movement */
                if (wallNormal != Vector3.zero && (isMovingInto(projectedVel, wallNormal) || isMovingInto(projectedStartVel, wallNormal)))
                {
                    Vector3 vel = Vector3.zero;
                    if (rb3D.useGravity)
                    {
                        vel.y = rb3D.velocity.y;
                    }
                    rb3D.velocity = vel;

                    break;
                }
                /* Else move along the wall */
                else
                {
                    /* Move up to the on coming wall */
                    float moveUpDist = Mathf.Max(0, hitInfo.distance);
                    rb3D.MovePosition(rb3D.position + (direction * moveUpDist));

                    rb3D.velocity = projectedVel;

                    /* Make this wall the previous wall */
                    wallNormal = hitInfo.normal;
                    //Debug.DrawRay(hitInfo.point, hitInfo.normal, new Color(1f, 0.6471f, 0), 1, false);
                }
            }
            else
            {
                break;
            }
        }
    }

    private bool isMovingInto(Vector3 dir, Vector3 normal)
    {
        return Vector3.Angle(dir, normal) > 90f;
    }

    private Vector3 limitVelocityOnWall(Vector3 velocity, Vector3 planeNormal)
    {
        if (!rb3D.useGravity)
        {
            Vector3 groundPlaneIntersection = Vector3.Cross(movementNormal, planeNormal);

            velocity = Vector3.Project(velocity, groundPlaneIntersection);

            /* Don't move up the intersecting line if it is greater than our slope limit */
            if (Vector3.Angle(velocity, Vector3.up) < 90f - slopeLimit)
            {
                velocity = Vector3.zero;
            }
        }
        else
        {
            /* Get vector pointing down the slope) */
            Vector3 rightSlope = Vector3.Cross(planeNormal, Vector3.down);
            Vector3 downSlope = Vector3.Cross(rightSlope, planeNormal);

            /* Keep any downward movement (like gravity) */
            float yComponent = Mathf.Min(0f, rb3D.velocity.y);

            /* Project the remaining movement on to the wall */
            Vector3 newVel = rb3D.velocity;
            newVel.y = 0;
            newVel = Vector3.ProjectOnPlane(newVel, planeNormal);

            /* If the remaining movement is moving up the wall then make it only go left/right.
             * I believe this will be true for all  ramp walls but false for all ceiling walls */
            if (Vector3.Angle(downSlope, newVel) > 90f)
            {
                newVel = Vector3.Project(newVel, rightSlope);
            }

            /* Add the downward movement back in and make sure we are still moving along the wall
             * so future sphere casts won't hit this wall */
            newVel.y = yComponent;
            newVel = Vector3.ProjectOnPlane(newVel, planeNormal);

            velocity = newVel;

            //Debug.DrawLine(colliderPosition, colliderPosition + (Vector3.up), Color.blue);
            //Debug.DrawLine(colliderPosition, colliderPosition + (planeMovement.normalized), Color.magenta);
        }

        return velocity;
    }

    /// <summary>
    /// The position that should be used for most movement AI code. For 2D chars the position will 
    /// be on the X/Y plane. For 3D grounded characters the position is on the X/Z plane. For 3D
    /// flying characters the position is in full 3D (X/Y/Z).
    /// </summary>
    public Vector3 position
    {
        get
        {
            if (is3D)
            {
                if (canFly)
                {
                    return rb3D.position;
                }
                else
                {
                    return new Vector3(rb3D.position.x, 0, rb3D.position.z);
                }
            }
            else
            {
                return rb2D.position;
            }
        }
    }

    /// <summary>
    /// Gets the position of the collider (which can be offset from the transform position).
    /// </summary>
    public Vector3 colliderPosition
    {
        get
        {
            if (is3D)
            {
                return transform.TransformPoint(col3D.center);
            }
            else
            {
                return transform.TransformPoint(col2D.offset);
            }
        }
    }

    private int count = 0;

    /// <summary>
    /// The velocity that should be used for movement AI code. For 2D chars this velocity will be on 
    /// the X/Y plane. For 3D grounded characters this velocity will be on the X/Z plane but will be
    /// applied on whatever plane the character is currently moving on. For 3D flying characters the
    /// velocity will be in full 3D (X/Y/Z).
    /// </summary>
    public Vector3 velocity
    {
        get
        {
            if (is3D)
            {
                if (canFly)
                {
                    return rb3D.velocity;
                }
                else
                {
                    Vector3 dir = rb3D.velocity;
                    dir.y = 0;

                    float mag = Vector3.ProjectOnPlane(rb3D.velocity, movementNormal).magnitude;

                    return dir.normalized * mag;
                }
            }
            else
            {
                return rb2D.velocity;
            }
        }

        set
        {
            if (is3D)
            {
                if (canFly)
                {
                    rb3D.velocity = value;
                }
                /* Assume the value is given as a vector on the x/z plane for grounded chars*/
                else
                {
                    //Debug.Log("setvelocity " + transform.position.ToString("f4"));
                    count++;
                    //Debug.Log(count + " " + rb3D.velocity.ToString("f4"));

                    /* If the char is not on the ground then then we will move along the x/z
                     * plane and keep any y movement we already have */
                    if (rb3D.useGravity)
                    {
                        value.y = rb3D.velocity.y;
                        rb3D.velocity = value;
                    }
                    /* Else only move along the ground plane */
                    else
                    {
                        rb3D.velocity = dirOnPlane(value, movementNormal) * value.magnitude;
                    }

                    //Debug.Log("Value Vel " + value.ToString("f4"));

                    limitMovementOnSteepSlopes();
                }
            }
            else
            {
                rb2D.velocity = value;
            }
        }
    }

    /// <summary>
    /// The actual velocity of the underlying unity rigidbody.
    /// </summary>
    public Vector3 realVelocity
    {
        get
        {
            return (is3D) ? rb3D.velocity : (Vector3) rb2D.velocity;
        }
        set
        {
            if(is3D)
            {
                rb3D.velocity = value;
            }
            else
            {
                rb2D.velocity = value;
            }
        }
    }

    /// <summary>
    /// Creates a vector that maintains x/z direction but lies on the plane.
    /// </summary>
    private Vector3 dirOnPlane(Vector3 vector, Vector3 planeNormal)
    {
        Vector3 newVel = vector;
        newVel.y = (-planeNormal.x * vector.x - planeNormal.z * vector.z) / planeNormal.y;
        return newVel.normalized;
    }

    public new Transform transform
    {
        get
        {
            if (is3D)
            {
                return rb3D.transform;
            }
            else
            {
                return rb2D.transform;
            }
        }
    }

    public Quaternion rotation
    {
        get
        {
            if (is3D)
            {
                return rb3D.rotation;
            }
            else
            {
                Quaternion r = Quaternion.identity;
                r.eulerAngles = new Vector3(0, 0, rb2D.rotation);
                return r;
            }
        }

        set
        {
            if(is3D)
            {
                rb3D.rotation = value;
            } else
            {
                rb2D.rotation = value.eulerAngles.z;
            }
        }
    }

    /// <summary>
    /// The angularVelocity for the rigidbody. If its a 3D rigidbody underneath then the angularVelocity is for the y axis only (setting the angular velocity will clear out the x/z angular velocities).
    /// </summary>
    public float angularVelocity
    {
        get
        {
            if (is3D)
            {
                return rb3D.angularVelocity.y;
            }
            else
            {
                return rb2D.angularVelocity;
            }
        }

        set
        {
            if (is3D)
            {
                rb3D.angularVelocity = new Vector3(0, value, 0);
            }
            else
            {
                rb2D.angularVelocity = value;
            }
        }
    }

    /// <summary>
    /// Rotates the rigidbody to angle (given in degrees)
    /// </summary>
    /// <param name="angle"></param>
    public void MoveRotation(float angle)
    {
        if (is3D)
        {
            Quaternion rot = Quaternion.Euler((new Vector3(0f, angle, 0f)));
            rb3D.MoveRotation(rot);
        }
        else
        {
            rb2D.MoveRotation(angle);
        }
    }

    public float rotationInRadians
    {
        get
        {
            if (is3D)
            {
                return rb3D.rotation.eulerAngles.y * Mathf.Deg2Rad;
            }
            else
            {
                return rb2D.rotation * Mathf.Deg2Rad;
            }
        }
    }

    public Vector3 rotationAsVector
    {
        get
        {
            return SteeringBasics.orientationToVector(rotationInRadians, is3D);
        }
    }

    /// <summary>
    /// Converts the vector based what kind of character the rigidbody is on. 
    /// If it is a 2D character then the Z component will be zeroed out. If it
    /// is a grounded 3D character then the Y component will be zeroed out. 
    /// And if it is flying 3D character no changes will be made to the vector.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public Vector3 convertVector(Vector3 v)
    {
        /* If the character is a 2D character then ignore the z component */
        if (!is3D)
        {
            v.z = 0;
        }
        /* Else if the charater is a 3D character who can't fly then ignore the y component */
        else if (!canFly)
        {
            v.y = 0;
        }

        return v;
    }

    /* This function is here to ensure we have a rigidbody (2D or 3D). */
    #if UNITY_EDITOR
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void Reset()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();

        if (rb == null && rb2D == null)
        {
            if (UnityEditor.EditorUtility.DisplayDialog("Choose a Component", "You are missing one of the required componets. Please choose one to add", "Rigidbody", "Rigidbody2D"))
            {
                gameObject.AddComponent<Rigidbody>();
            }
            else
            {
                gameObject.AddComponent<Rigidbody2D>();
            }
        }
    }
    #endif
}
