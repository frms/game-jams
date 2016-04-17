using UnityEngine;
using System.Collections;

public class Enemy2 : Enemy {
    public Transform bulletPrefab;
    public float spawnOffsetDist = 0.85f;

    public float[] timeBetweenFire;
    private float nextFire;

    private GameObject player;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        nextFire = getNextFire();

        player = GameObject.Find("Player");
    }

    private float getNextFire()
    {
        return Time.time + Random.Range(timeBetweenFire[0], timeBetweenFire[1]);
    }


    void Update()
    {
        if (!f.done || player == null)
        {
            return;
        }

        if (nextFire < Time.time)
        {
            Vector3 startPos = transform.position;
            startPos.y = 0.27f;
            Vector3 startAngles = bulletPrefab.eulerAngles;
            startAngles.y = transform.eulerAngles.y;

            Instantiate(bulletPrefab, startPos + transform.right * spawnOffsetDist, Quaternion.Euler(startAngles));

            startAngles.y += 90;
            Instantiate(bulletPrefab, startPos - transform.forward * spawnOffsetDist, Quaternion.Euler(startAngles));

            startAngles.y += 90;
            Instantiate(bulletPrefab, startPos - transform.right * spawnOffsetDist, Quaternion.Euler(startAngles));

            startAngles.y += 90;
            Instantiate(bulletPrefab, startPos + transform.forward * spawnOffsetDist, Quaternion.Euler(startAngles));

            nextFire = getNextFire();
        }
    }

    public override void outOfHealth()
    {
        Map m = GameObject.Find("Map").GetComponent<Map>();
        m.spawn(2);
        Destroy(gameObject);
    }

}
