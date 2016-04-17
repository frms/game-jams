using UnityEngine;
using System.Collections;

public class PlayerBullet : Bullet {
    private float stopTime;

    public override void Start()
    {
        base.Start();

        stopTime = Time.time + (GameManager.playerBulletDist / speed);
    }

    void Update()
    {
        if(stopTime < Time.time)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ( (other.tag == "Enemy") || ((other.tag != "Player") && other.gameObject.layer != GameManager.dontSense) ) {
            Health h = other.GetComponent<Health>();
            if (h != null)
            {
                h.applyDamage(dmg);
            }
            Destroy(gameObject);
        }
    }
}
