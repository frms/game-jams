using UnityEngine;
using System.Collections;

public class PlayerShielder : PlayerCharacter
{
    private Shielder shielder;

    public override void Start()
    {
        base.Start();

        shielder = GetComponent<Shielder>();
    }

    public override bool handleInput()
    {
        bool stillControlling = true;

        if (Input.GetMouseButtonDown(0))
        {
            Slot s = getPlayerSlotAtMouse();
            shielder.tryToUse(s);

            stillControlling = false;
        }

        stillControlling &= handleMove();

        return stillControlling;
    }
}
