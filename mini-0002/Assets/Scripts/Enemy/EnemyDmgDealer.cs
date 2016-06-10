using UnityEngine;
using System.Collections;

public class EnemyDmgDealer : Health
{
    public NewTargetLogic newTargetLogic = NewTargetLogic.random;

    public bool newTargetEachAtk;

    private SingleTarget singleTarget;

    public override void Start()
    {
        base.Start();

        singleTarget = GetComponent<SingleTarget>();

        if(newTargetEachAtk)
        {
            singleTarget.UseEvent += setNewTarget;
        }
    }

    public override void Update()
    {
        base.Update();

        if(singleTarget.target == null)
        {
            setNewTarget();
        }
    }

    public void setNewTarget()
    {
        if(newTargetLogic == NewTargetLogic.random)
        {
            singleTarget.target = BattleManager.main.playerParty.getRandomChar();
        }
        else if (newTargetLogic == NewTargetLogic.weakest)
        {
            singleTarget.target = BattleManager.main.playerParty.getWeakestChar();
        }
    }
}

public enum NewTargetLogic { random, weakest }