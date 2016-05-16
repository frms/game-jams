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

        if(CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            Instantiate(boomerangPrefab, transform.position, Quaternion.identity);
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

    public override void applyDamage(float damage)
    {
        if(hasPartner)
        {
            Vector3 pos = transform.position;
            pos.x += Mathf.Sign(transform.localScale.x) * partnerDropOffset.x;
            pos.y += partnerDropOffset.y;

            GameObject go = Instantiate(partnerPrefab, pos, Quaternion.identity) as GameObject;
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            rb.AddForce(partnerDropImpulse, ForceMode2D.Impulse);

            hasPartner = false;
        }

        base.applyDamage(damage);
    }

    public override void outOfHealth()
    {
        base.outOfHealth();
        Destroy(healthBar.gameObject);
    }
}
