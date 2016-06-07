using UnityEngine;
using System.Collections;

public class Shielder : MonoBehaviour
{
    public Transform shieldPrefab;
    public float rate;

    private float lastTimeUsed;
    private Transform progressBar;

    void Start()
    {
        lastTimeUsed = Time.time;
        progressBar = transform.FindChild("ProgressBar");
    }

    void Update()
    {
        Vector3 scale = progressBar.transform.localScale;
        scale.x = Mathf.Min(Time.time - lastTimeUsed, rate) / rate;
        progressBar.transform.localScale = scale;
    }

    public void tryToUse(Slot s)
    {
        if (s != null && s.isOpen() && isReady())
        {
            s.set(Instantiate(shieldPrefab));
            lastTimeUsed = Time.time;
        }
    }

    public bool isReady()
    {
        return lastTimeUsed + rate < Time.time;
    }
}
