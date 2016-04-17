using UnityEngine;
using System.Collections;

public class Wall : Health {

    public Color[] wallColors;
    public float percentPinkWalls = 0.5f;

    private Map m;
    private bool canBeDestroyed;
    private int x, y;

	// Use this for initialization
	void Start ()
    {
        randomColor();

        Fader f = GetComponent<Fader>();
        f.setAlpha(0);

        f.targetAlpha(1, 0.5f + 1.7f * Random.value);
        //f.targetAlpha(1, 1.5f);


        m = GameObject.Find("Map").GetComponent<Map>();
        x = (int)(transform.position.x - 0.5f + (m.width / 2));
        y = (int)(transform.position.z - 0.5f + (m.height / 2));
        //(-width / 2 + x + .5f, aboveGround, -height / 2 + y + .5f);
        canBeDestroyed = !(x == 0 || x == m.width - 1 || y == 0 || y == m.height - 1);
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

    public override void outOfHealth()
    {
        if(canBeDestroyed)
        {
            m.openSpots.Add(new int[] { x, y });
            m.spawn(0);
            Destroy(gameObject);
        }
    }
}
