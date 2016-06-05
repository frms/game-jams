using UnityEngine;
using System.Collections;

public class EnemyCharacter : Health
{
    private SingleTarget singleTarget;

    public override void Start()
    {
        base.Start();

        singleTarget = GetComponent<SingleTarget>();
    }

    public override void Update()
    {
        base.Update();

        if(singleTarget.target == null)
        {
            singleTarget.target = BattleManager.main.playerParty.getRandomChar();
        }
    }
}
