using UnityEngine;
using System.Collections;

public class Shielder : MonoBehaviour
{
    public Transform shieldPrefab;
    public float rate;

    private float timeSinceLastUse = 0;
    private Transform progressBar;

    void Start()
    {
        progressBar = transform.FindChild("ProgressBar");
        updateProgressBar();
    }

    void Update()
    {
        if(BattleManager.main.isPaused)
        {
            return;
        }

        timeSinceLastUse += Time.deltaTime;

        updateProgressBar();
    }

    private void updateProgressBar()
    {
        Vector3 scale = progressBar.transform.localScale;
        scale.x = Mathf.Min(timeSinceLastUse / rate, 1);
        progressBar.transform.localScale = scale;
    }

    public void tryToUse(Slot s)
    {
        if (s != null && s.isOpen() && isReady())
        {
            s.set(Instantiate(shieldPrefab));
            timeSinceLastUse = 0;
            updateProgressBar();
        }
    }

    public bool isReady()
    {
        return timeSinceLastUse >= rate;
    }
}
