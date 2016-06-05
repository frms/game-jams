using UnityEngine;
using System.Collections;

public class DmgDealer : Health
{
    public Bullet bulletPrefab;
    public float atkRate;

    public Transform atkTarget = null;
    private float lastAtkTime = -Mathf.Infinity;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        tryToAtk();
    }

    public void tryToAtk()
    {
        if (atkTarget != null && lastAtkTime + atkRate < Time.time)
        {
            Bullet b = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as Bullet;
            b.target = atkTarget;

            lastAtkTime = Time.time;
        }
    }
}

