using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float fadeInTime = 2.5f;
    private Fader f;

    //public LinePath path;

    private SteeringBasics steeringBasics;
    private WallAvoidance wallAvoidance;
    //private FollowPath followPath;

    private Transform player;

    // Use this for initialization
    void Start()
    {
        //path.calcDistances();

        steeringBasics = GetComponent<SteeringBasics>();
        wallAvoidance = GetComponent<WallAvoidance>();
        //followPath = GetComponent<FollowPath>();

        player = GameObject.Find("Player").transform;

        f = GetComponent<Fader>();
        f.setAlpha(0f);
        f.targetAlpha(1f, fadeInTime);
    }

    void FixedUpdate()
    {
        if(!f.done)
        {
            return;
        }

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

        //path.draw();
    }
}
