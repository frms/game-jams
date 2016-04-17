using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float barProgress = 100;
	public float barMax = 100;

    public float percentHealth
    {
        get
        {
            return barProgress / barMax;
        }
    }

    public float[] fadeInTime, fadeOutTime;
    internal Fader f;

    // Use this for initialization
    public virtual void Start()
    {
        f = GetComponent<Fader>();
        f.setAlpha(0f);
        f.targetAlpha(1f, Random.Range(fadeInTime[0], fadeOutTime[1]));
    }

    public AudioClip hurtClip;
	public float hurtVolume = 1f;

	public void applyDamage(float damage) {
        if(barProgress <= 0)
        {
            return;
        }

		barProgress -= damage;

		if(hurtClip != null && damage > 0) {
			AudioSource.PlayClipAtPoint(hurtClip, Camera.main.transform.position, hurtVolume);
		}
		
		// Kill the game obj if it loses all its health
		if(barProgress <= 0) {
            outOfHealth();
		}
	}

    public virtual void outOfHealth()
    {
        if(f != null)
            f.targetAlpha(0f, Random.Range(fadeOutTime[0], fadeOutTime[1]));
    }
}
