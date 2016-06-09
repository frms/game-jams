using UnityEngine;
using System.Collections;

public class SingleTarget : MonoBehaviour
{
    public Bullet bulletPrefab;
    public float rate;

    public Transform target = null;
    private float timeSinceLastUse = 0;

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
        }
    }
}
