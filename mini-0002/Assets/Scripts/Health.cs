using UnityEngine;
using UnityEngine.UI;

public class Health : Hoverable
{
    public Text healthText;
    public float currentHealth = 100;
    public float maxHealth = 100;

    public float percentHealth
    {
        get
        {
            return currentHealth / maxHealth;
        }
    }

    public AudioClip hurtClip;
    public float hurtVolume = 1f;

    public override void Start()
    {
        base.Start();

        if(healthText != null)
        {
            //healthText.color = hoverColor;
        }
    }

    public override void Update()
    {
        base.Update();

        if(healthText != null)
        {
            updateHealth();
        }
    }

    public void updateHealth()
    {
        healthText.text = ((int) currentHealth).ToString();
    }

    public virtual void applyDamage(float damage) {
        if(currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damage;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if(hurtClip != null && damage > 0) {
            AudioSource.PlayClipAtPoint(hurtClip, Camera.main.transform.position, hurtVolume);
        }
        
        // Kill the game obj if it loses all its health
        if(currentHealth <= 0) {
            outOfHealth();
        }
    }

    public virtual void outOfHealth()
    {
        Destroy(gameObject);
    }
}
