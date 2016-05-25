using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour {

    public float atkDmg;

    private SteeringBasics steeringBasics;
    private Wander2 wander;
    private CollisionAvoidance colAvoid;

    private NearSensor colAvoidSensor;

    // Use this for initialization
    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        wander = GetComponent<Wander2>();
        colAvoid = GetComponent<CollisionAvoidance>();

        colAvoidSensor = transform.Find("ColAvoidSensor").GetComponent<NearSensor>();
    }

    void FixedUpdate()
    {
        Vector3 accel = colAvoid.getSteering(colAvoidSensor.targets);

        if (accel.magnitude < 0.005f)
        {
            accel = wander.getSteering();
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + accel.normalized, Color.red, 0f, false);
        }

        steeringBasics.steer(accel);
        steeringBasics.lookWhereYoureGoing();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Health h = collision.gameObject.GetComponent<Health>();
            h.applyDamage(atkDmg);
        }
    }
}
