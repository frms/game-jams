using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Player : Health {

    public Transform healthBar;

    private PlatformerCharacter2D character;
    private bool jump;

    private void Awake()
    {
        character = GetComponent<PlatformerCharacter2D>();
    }

    private void Update()
    {
        if (!jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            jump = CrossPlatformInputManager.GetButtonDown("Jump");
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

    public override void outOfHealth()
    {
        base.outOfHealth();
        Destroy(healthBar.gameObject);
    }
}
