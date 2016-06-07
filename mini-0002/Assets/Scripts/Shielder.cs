using UnityEngine;
using System.Collections;

public class Shielder : MonoBehaviour
{
    public Transform shieldPrefab;
    public float rate;

    private float lastTimeUsed = 0;
    private Transform progressBar;

    void Start()
    {
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
        if (s != null && s.isOpen() && lastTimeUsed + rate < Time.time)
        {
            s.set(Instantiate(shieldPrefab));
            lastTimeUsed = Time.time;
        }
    }
}
