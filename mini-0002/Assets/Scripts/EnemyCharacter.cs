using UnityEngine;
using System.Collections;

public class EnemyCharacter : DmgDealer {
    public override void Update()
    {
        base.Update();

        if(atkTarget == null)
        {
            atkTarget = BattleManager.main.playerParty.getRandomChar();
        }
    }
}
