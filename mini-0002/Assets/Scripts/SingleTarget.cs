﻿using UnityEngine;
using System;
using UnityEngine.UI;

public class SingleTarget : MonoBehaviour
{
    public Text dpsText;
    public Color positiveColor;
    public Color negativeColor;

    public Bullet bulletPrefab;
    public float rate;

    public Transform target = null;
    private float timeSinceLastUse = 0;

    public event Action UseEvent;

    void Start()
    {
        if (dpsText != null)
        {
            int dps = (int)(bulletPrefab.dmg / rate);
            dpsText.text = Mathf.Abs(dps).ToString();

            if(dps >= 0)
            {
                dpsText.color = positiveColor;
            }
            else
            {
                dpsText.color = negativeColor;
            }
        }
    }

    // Update is called once per frame
    public void Update()
    {
        tryToUse();
    }

    public void tryToUse()
    {
        if(BattleManager.main.isPaused)
        {
            return;
        }

        timeSinceLastUse += Time.deltaTime;

        if (target != null && timeSinceLastUse >= rate)
        {
            Bullet b = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as Bullet;
            b.target = target;

            timeSinceLastUse = 0;

            if(UseEvent != null)
            {
                UseEvent();
            }
        }
    }
}
