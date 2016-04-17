using UnityEngine;
using System.Collections;

public class Enemy : Health
{
    public float fadeInTime = 2.5f;
    internal Fader f;

    // Use this for initialization
    public virtual void Start()
    {
        f = GetComponent<Fader>();
        f.setAlpha(0f);
        f.targetAlpha(1f, fadeInTime);
    }
}
