using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

    public bool done = true;

    private Material mat;
    private float desiredAlpha = 1f;
    private float speed;

	// Use this for initialization
	void Start () {
        
	}

    private void checkMat()
    {
        if (mat == null)
        {
            mat = GetComponent<Renderer>().material;
        }
    }

    public void setAlpha(float alpha)
    {
        checkMat();

        Color c = mat.color;
        c.a = alpha;
        mat.color = c;
    }

    public void targetAlpha(float targetAlpha, float time)
    {
        checkMat();

        desiredAlpha = targetAlpha;
        speed = Mathf.Abs(desiredAlpha - mat.color.a) / time;

        done = false;

        StopCoroutine("fade");
        StartCoroutine("fade");
    }

    IEnumerator fade()
    {
        while (mat.color.a != desiredAlpha)
        {
            float difference = desiredAlpha - mat.color.a;
            Color c = mat.color;
            c.a += Mathf.Sign(difference) * Mathf.Min(Mathf.Abs(difference), speed * Time.deltaTime);
            mat.color = c;

            yield return null;
        }

        done = true;
    }
}
