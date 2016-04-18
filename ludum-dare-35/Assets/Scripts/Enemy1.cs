using UnityEngine;
using System.Collections;

public class Enemy1 : Enemy {

    private SteeringBasics steeringBasics;
    private WallAvoidance wallAvoidance;

    private Transform player;

    public float explodeDist = 0.85f;
    public float explodeDmg = 15f;

    private float awakeDist;

    private Rigidbody rb;

    public GameObject explosionPrefab;

    // Use this for initialization
    public override void Start () {
        base.Start();

        steeringBasics = GetComponent<SteeringBasics>();
        wallAvoidance = GetComponent<WallAvoidance>();

        player = GameObject.Find("Player").transform;

        rb = GetComponent<Rigidbody>();

        Vector3 screenDiag = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10)) - Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10));
        awakeDist = 0.95f * Mathf.Max(screenDiag.x, screenDiag.z);
    }

    void FixedUpdate()
    {
        if(barProgress <= 0 && f.done)
        {
            Map m = GameObject.Find("Map").GetComponent<Map>();
            m.spawn(1);
            Destroy(gameObject);
        }

        if(player == null)
        {
            return;
        }

        float distToPlayer = Vector3.Distance(player.position, transform.position);

        if (!f.done || distToPlayer > awakeDist)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        if (Vector3.Distance(player.position, transform.position) <= explodeDist)
        {
            player.GetComponent<Health>().applyDamage(explodeDmg);
            applyDamage(10000000f);

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            rb.velocity = Vector3.zero;

            Camera.main.GetComponent<CameraMovement>().shake = 0.05f;
        }

        Vector3 accel = wallAvoidance.getSteering();

        if (accel.magnitude < 0.005f)
        {
            accel = steeringBasics.arrive(player.position);
        }

        steeringBasics.steer(accel);
        steeringBasics.lookWhereYoureGoing();
    }
}
