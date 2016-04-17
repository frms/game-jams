﻿using UnityEngine;
using System.Collections;

public class Enemy : Health
{
    public float fadeInTime = 2.5f;
    private Fader f;

    private SteeringBasics steeringBasics;
    private WallAvoidance wallAvoidance;

    private Transform player;

    public float explodeDist = 0.85f;
    public float explodeDmg = 15f;

    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        wallAvoidance = GetComponent<WallAvoidance>();

        player = GameObject.Find("Player").transform;

        f = GetComponent<Fader>();
        f.setAlpha(0f);
        f.targetAlpha(1f, fadeInTime);

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(!f.done || player == null)
        {
            rb.velocity = Vector3.zero;   
            return;
        }

        if(Vector3.Distance(player.position, transform.position) <= explodeDist)
        {
            player.GetComponent<Health>().applyDamage(explodeDmg);
            Destroy(gameObject);
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