using UnityEngine;
using System.Collections;

public class Expand : MonoBehaviour {
    public float[] sizes;
    public float[] alphas;
    public float time;

    private Material mat;

    private float startTime;

	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;

        setAlpha(alphas[0]);
        setSize(sizes[0]);

        startTime = Time.time;
    }

    public void setSize(float size)
    {
        transform.localScale = new Vector3(size, size, size);
    }

    public void setAlpha(float alpha)
    {
        Color c = mat.color;
        c.a = alpha;
        mat.color = c;
    }

    // Update is called once per frame
    void Update () {
        float t = (Time.time - startTime) / time;
        setAlpha(Mathf.Lerp(alphas[0], alphas[1], t));
        setSize(Mathf.Lerp(sizes[0], sizes[1], t));
	}
}
