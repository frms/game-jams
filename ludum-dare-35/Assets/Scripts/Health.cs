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

	public AudioClip hurtClip;
	public float hurtVolume = 1f;

	public void applyDamage(float damage) {
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
        Destroy(this);
    }
}
