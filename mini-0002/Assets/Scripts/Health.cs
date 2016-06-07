using UnityEngine;
using System.Collections;

public class Health : Hoverable
{

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

    private Transform healthBar;

    public override void Start()
    {
        base.Start();

        healthBar = transform.FindChild("HealthBar");

        if(healthBar != null)
        {
            SpriteRenderer healthSr = healthBar.GetComponent<SpriteRenderer>();
            healthSr.color = hoverColor;
        }
    }

    public override void Update()
    {
        base.Update();

        if(healthBar != null)
        {
            updateHealthBar();
        }
    }

    public void updateHealthBar()
    {
        Vector3 scale = healthBar.transform.localScale;
        scale.x = percentHealth;
        healthBar.transform.localScale = scale;
    }

    public virtual void applyDamage(float damage) {
        if(currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damage;

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
