using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    public LinePath path;

    private SteeringBasics steeringBasics;
    private WallAvoidance wallAvoidance;
    private FollowPath followPath;

    private Transform player;

    // Use this for initialization
    void Start()
    {
        path.calcDistances();

        steeringBasics = GetComponent<SteeringBasics>();
        wallAvoidance = GetComponent<WallAvoidance>();
        followPath = GetComponent<FollowPath>();

        player = GameObject.Find("Player").transform;
    }

    void FixedUpdate()
    {
        //if (followPath.isAtEndOfPath(path))
        //{
        //    path.reversePath();
        //}

        Vector3 accel = wallAvoidance.getSteering();

        if (accel.magnitude < 0.005f)
        {
            //accel = followPath.getSteering(path);
            accel = steeringBasics.arrive(player.position);
        }

        steeringBasics.steer(accel);
        steeringBasics.lookWhereYoureGoing();

        path.draw();
    }
}
