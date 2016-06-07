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

    private float readyToTryTime = -1;

    public override void Update()
    {
        base.Update();

        if (shielder.isReady())
        {
            Slot s = BattleManager.main.enemyParty.getRandomEmptySlot();

            if (s == null)
            {
                readyToTryTime = -1;
            }
            else if (readyToTryTime == -1)
            {
                readyToTryTime = Time.time + Random.Range(decisionTime[0], decisionTime[1]);
            }
            else if(readyToTryTime <= Time.time)
            {
                shielder.tryToUse(s);
            }
        }
        else
        {
            readyToTryTime = -1;
        }
    }
}
