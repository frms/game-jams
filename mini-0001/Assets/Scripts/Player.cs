using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Player : Health {

    public Transform healthBar;

    public GameObject boomerangPrefab;

    public GameObject partnerPrefab;
    public Vector2 partnerDropOffset;
    public Vector2 partnerDropImpulse;
    public float partnerDropHealthDrain;

    private bool hasPartner;

    private PlatformerCharacter2D character;
    private bool jump;

    private void Awake()
    {
        hasPartner = true;

        character = GetComponent<PlatformerCharacter2D>();
        jump = false;
    }

    private void Update()
    {
        if (!jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if(hasPartner && CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            Instantiate(boomerangPrefab, transform.position, Quaternion.identity);
        }

        if(!hasPartner)
        {
            applyDamage(transform, partnerDropHealthDrain * Time.deltaTime);
        }
        else if(currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + partnerDropHealthDrain * Time.deltaTime, maxHealth);
        }

        healthBar.localScale = new Vector3(percentHealth, 1f, 1f);
    }

    private void FixedUpdate()
    {
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        float h = CrossPlatformInputManager.GetAxis("Horizontal");

        character.Move(h, crouch, jump);
        jump = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.layer == LayerMask.NameToLayer("Partner"))
        {
            Destroy(coll.gameObject);
            hasPartner = true;
        }
    }

    public override void applyDamage(Transform other, float damage)
    {
        if(hasPartner)
        {
            float dir = Mathf.Sign(transform.position.x - other.position.x);

            Vector3 pos = transform.position;
            pos.x += dir * partnerDropOffset.x;
            pos.y += partnerDropOffset.y;

            GameObject go = Instantiate(partnerPrefab, pos, Quaternion.identity) as GameObject;

            Vector3 impulse = partnerDropImpulse;
            impulse.x *= dir;
            go.GetComponent<Rigidbody2D>().AddForce(impulse, ForceMode2D.Impulse);

            hasPartner = false;
        }

        base.applyDamage(other, damage);
    }

    public override void outOfHealth()
    {
        base.outOfHealth();
        Destroy(healthBar.gameObject);
    }
}
