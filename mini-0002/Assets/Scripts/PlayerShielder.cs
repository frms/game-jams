using UnityEngine;
using System.Collections;

public class PlayerShielder : PlayerCharacter
{
    public override bool handleInput()
    {
        bool stillControlling = true;

        if (Input.GetMouseButtonDown(0))
        {
            Slot s = getPlayerSlotAtMouse();

            if (s != null && s.isOpen())
            {
                Debug.Log("Place shield");
            }

            stillControlling = false;
        }

        stillControlling &= handleMove();

        return stillControlling;
    }
}
