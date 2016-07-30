using UnityEngine;

public class Enemy1 : MonoBehaviour
{

    private MovementAIRigidbody player;
    private SteeringBasics steeringBasics;
    private Pursue pursue;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<MovementAIRigidbody>();
        steeringBasics = GetComponent<SteeringBasics>();
        pursue = GetComponent<Pursue>();
    }

    void FixedUpdate()
    {
        if(player != null)
        {
            Vector3 accel = pursue.getSteering(player);

            steeringBasics.steer(accel);
            steeringBasics.lookWhereYoureGoing();
        }
    }
}
