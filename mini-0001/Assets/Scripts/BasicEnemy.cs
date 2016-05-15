using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class BasicEnemy : MonoBehaviour {

    public float atkDist;
    public float atkCooldown;
    public float atkDmg;

    private float lastAtk;

    private PlatformerCharacter2D character;
    private Health player;

    void Awake()
    {
        lastAtk = Time.time;

        character = GetComponent<PlatformerCharacter2D>();
        player = GameObject.Find("Player").GetComponent<Health>();
    }

    void FixedUpdate()
    {
        if(player == null)
        {
            return;
        }

        bool crouch = false;
        bool jump = false;

        float deltaX = player.transform.position.x - transform.position.x;

        float h = sign(deltaX);

        if(Mathf.Abs(deltaX) <= atkDist)
        {
            h = 0;

            tryToAtk();
        }

        character.Move(h, crouch, jump);
    }

    private void tryToAtk()
    {
        if (lastAtk + atkCooldown <= Time.time)
        {
            player.applyDamage(atkDmg);
            lastAtk = Time.time;
        }
    }

    private static int sign(float number)
    {
        return number < 0 ? -1 : (number > 0 ? 1 : 0);
    }
}
