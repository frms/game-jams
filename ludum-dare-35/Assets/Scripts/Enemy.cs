using UnityEngine;
using System.Collections;

public class Enemy : Health
{
    public AudioClip explosionClip;
    public float explosionVolume = 1f;

    public override void outOfHealth()
    {
        if (f != null)
        {
            f.targetAlpha(0f, Random.Range(fadeOutTime[0], fadeOutTime[1]));

            if (explosionClip != null)
            {
                AudioSource.PlayClipAtPoint(explosionClip, Camera.main.transform.position, explosionVolume);
            }

            GameManager.enemyKillCount++;
        }
    }
}
