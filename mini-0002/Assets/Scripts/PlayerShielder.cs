using UnityEngine;
using System.Collections;

public class PlayerShielder : PlayerCharacter
{
    public Transform shieldPrefab;

    public override bool handleInput()
    {
        bool stillControlling = true;

        if (Input.GetMouseButtonDown(0))
        {
            Slot s = getPlayerSlotAtMouse();

            if (s != null && s.isOpen())
            {
                s.set(Instantiate(shieldPrefab));
            }

            stillControlling = false;
        }

        stillControlling &= handleMove();

        return stillControlling;
    }
}
