using UnityEngine;
using System.Collections;

public class EnemyShielder : Health
{
    public float[] decisionTime;

    private Shielder shielder;

    public override void Start()
    {
        base.Start();

        shielder = GetComponent<Shielder>();
    }

    private float thinkingTimeLeft = Mathf.Infinity;

    public override void Update()
    {
        base.Update();

        if(BattleManager.main.isPaused)
        {
            return;
        }

        if (shielder.isReady())
        {
            Slot s = BattleManager.main.enemyParty.getRandomEmptySlot();

            if (s == null)
            {
                thinkingTimeLeft = Mathf.Infinity;
            }
            else if (thinkingTimeLeft == Mathf.Infinity)
            {
                thinkingTimeLeft = Random.Range(decisionTime[0], decisionTime[1]);
            }
            else if (thinkingTimeLeft > 0)
            {
                thinkingTimeLeft -= Time.deltaTime;
            }
            else
            {
                shielder.tryToUse(s);
            }
        }
        else
        {
            thinkingTimeLeft = Mathf.Infinity;
        }
    }
}
