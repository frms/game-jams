using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {

    public float time;
    public float alpha;

    private Material mat;

    // Use this for initialization
    void Start () {
        mat = GetComponent<Renderer>().material;
        setAlpha(0f);
    }

    public void setAlpha(float alpha)
    {
        Color c = mat.color;
        c.a = alpha;
        mat.color = c;
    }

    public void flash()
    {
        StopCoroutine("flashCoroutine");
        StartCoroutine("flashCoroutine");
    }

    IEnumerator flashCoroutine()
    {
        setAlpha(alpha);
        yield return new WaitForSeconds(time);
        setAlpha(0f);
    }

}
