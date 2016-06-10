using UnityEngine;
using System.Collections;
using System;

public class SingleTarget : MonoBehaviour
{
    public Bullet bulletPrefab;
    public float rate;

    public Transform target = null;
    private float timeSinceLastUse = 0;

    public event Action UseEvent;

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
            setSaturation(b);

            timeSinceLastUse = 0;

            if(UseEvent != null)
            {
                UseEvent();
            }
        }
    }

    private void setSaturation(Bullet b)
    {
        SpriteRenderer sr = b.GetComponent<SpriteRenderer>();
        float h, s, v;
        Color.RGBToHSV(sr.color, out h, out s, out v);
        s *= 1 - (rate / 4f);
        sr.color = Color.HSVToRGB(h, s, v);
    }
}
