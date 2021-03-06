﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class BasicEnemy : Health {

    public float idleDist;

    public float atkDist;
    public float atkCooldown;
    public float atkDmg;

    private float lastAtk;

    private PlatformerCharacter2D character;
    private Health player;

    void Awake()
    {
        lastAtk = Time.time - atkCooldown;

        character = GetComponent<PlatformerCharacter2D>();
        player = GameObject.Find("Player").GetComponent<Health>();
    }

    void FixedUpdate()
    {
        if(player == null)
        {
            return;
        }

        bool crouch = false;
        bool jump = false;

        float deltaX = player.transform.position.x - transform.position.x;
        float dist = Mathf.Abs(deltaX);

        float h = sign(deltaX);

        if(dist >= idleDist)
        {
            h = 0;
        }
        else if (dist <= atkDist)
        {
            h = 0;
            tryToAtk();
        }

        character.Move(h, crouch, jump);
    }

    private void tryToAtk()
    {
        if (Mathf.Abs(player.transform.position.y - transform.position.y) < 0.5f && lastAtk + atkCooldown <= Time.time)
        {
            player.applyDamage(transform, atkDmg);
            lastAtk = Time.time;
        }
    }

    private static int sign(float number)
    {
        return number < 0 ? -1 : (number > 0 ? 1 : 0);
    }
}
