using UnityEngine;
using System.Collections;

public class SingleTarget : MonoBehaviour
{
    public Bullet bulletPrefab;
    public float rate;

    public Transform target = null;
    private float lastTimeUsed = -Mathf.Infinity;

    // Update is called once per frame
    public void Update()
    {
        tryToAtk();
    }

    public void tryToAtk()
    {
        if (target != null && lastTimeUsed + rate < Time.time)
        {
            Bullet b = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as Bullet;
            b.target = target;

            lastTimeUsed = Time.time;
        }
    }
}
