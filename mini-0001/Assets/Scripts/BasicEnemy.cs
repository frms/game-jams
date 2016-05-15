using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class BasicEnemy : MonoBehaviour {

    public float atkDist;

    private PlatformerCharacter2D character;
    private Transform player;

    void Awake()
    {
        character = GetComponent<PlatformerCharacter2D>();
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        bool crouch = false;
        bool jump = false;

        float deltaX = player.position.x - transform.position.x;

        float h = sign(deltaX);

        if(Mathf.Abs(deltaX) <= atkDist)
        {
            h = 0;
        }

        character.Move(h, crouch, jump);
    }

    private static int sign(float number)
    {
        return number < 0 ? -1 : (number > 0 ? 1 : 0);
    }
}
