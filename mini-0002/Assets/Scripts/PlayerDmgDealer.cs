﻿using UnityEngine;
using System.Collections;

public class PlayerDmgDealer : PlayerCharacter {
    private SingleTarget singleTarget;

    public override void Start()
    {
        base.Start();

        singleTarget = GetComponent<SingleTarget>();
    }

    public override bool handleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = PlayerCharacter.raycastAtMouse();

            if (hit.collider != null && hit.collider.tag == "EnemyChar")
            {
                singleTarget.target = hit.transform;
            }

            return false;
        }

        return true;
    }
}
