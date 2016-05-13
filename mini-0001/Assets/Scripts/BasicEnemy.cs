using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class BasicEnemy : MonoBehaviour {

    private PlatformerCharacter2D m_Character;
    private bool m_Jump;


    void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
    }


    void Update()
    {

    }


    void FixedUpdate()
    {
        // Read the inputs.
        bool crouch = false;
        float h = 0;

        // Pass all parameters to the character control script.
        m_Character.Move(h, crouch, m_Jump);
        m_Jump = false;
    }
}
