using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    public Color[] wallColors;
    public float percentPinkWalls = 0.5f;

	// Use this for initialization
	void Start ()
    {
        randomColor();

        Fader f = GetComponent<Fader>();
        f.setAlpha(0);

        f.targetAlpha(1, 0.5f + 1.7f * Random.value);
        //f.targetAlpha(1, 1.5f);
    }

    private void randomColor()
    {
        int colorIndex = 0;

        if (Random.value > percentPinkWalls)
        {
            colorIndex = Random.Range(1, wallColors.Length);
        }
        GetComponent<Renderer>().material.color = wallColors[colorIndex];
    }

    // Update is called once per frame
    void Update () {
	
	}
}
