using UnityEngine;
using System.Collections;

public class PlayerShielder : PlayerCharacter
{
    public Transform shieldPrefab;
    public float rate;

    private float lastTimeUsed = 0;
    private Transform progressBar;

    public override void Start()
    {
        base.Start();

        progressBar = transform.FindChild("ProgressBar");
    }

    public override void Update()
    {
        base.Update();

        Vector3 scale = progressBar.transform.localScale;
        scale.x = Mathf.Min(Time.time - lastTimeUsed, rate) / rate;
        progressBar.transform.localScale = scale;
    }

    public override bool handleInput()
    {
        bool stillControlling = true;

        if (Input.GetMouseButtonDown(0))
        {
            if (lastTimeUsed + rate < Time.time)
            {
                Slot s = getPlayerSlotAtMouse();

                if (s != null && s.isOpen())
                {
                    s.set(Instantiate(shieldPrefab));
                    lastTimeUsed = Time.time;
                }
            }
            stillControlling = false;
        }

        stillControlling &= handleMove();

        return stillControlling;
    }
}
