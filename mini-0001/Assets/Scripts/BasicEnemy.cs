using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class BasicEnemy : MonoBehaviour {

    private PlatformerCharacter2D m_Character;
    private bool m_Jump;

    private Transform player;


    void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        player = GameObject.Find("Player").transform;
    }


    void Update()
    {

    }


    void FixedUpdate()
    {
        // Read the inputs.
        bool crouch = false;
        float h = sign(player.position.x - transform.position.x);

        // Pass all parameters to the character control script.
        m_Character.Move(h, crouch, m_Jump);
        m_Jump = false;
    }

    private static int sign(float number)
    {
        return number < 0 ? -1 : (number > 0 ? 1 : 0);
    }
}
