﻿using UnityEngine;
using System.Collections;

public class PlayerBullet : Bullet {
    void OnTriggerEnter(Collider other)
    {
        if ((other.tag != "Player") && other.gameObject.layer != GameManager.dontSense) {
            Health h = other.GetComponent<Health>();
            if (h != null)
            {
                h.applyDamage(dmg);
            }
            Destroy(gameObject);
        }
    }
}
