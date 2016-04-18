using UnityEngine;
using System.Collections;

public class Wall : Health {

    public Color[] wallColors;
    public float percentPinkWalls = 0.5f;

    private Map m;
    private bool canBeDestroyed;
    private int x, y;

	// Use this for initialization
	public override void Start ()
    {
        randomColor();

        base.Start();

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

    void Update()
    {
        if (canBeDestroyed && barProgress <= 0 && f.done)
        {
            m.openSpots.Add(new int[] { x, y });
            m.spawn(0);
            Destroy(gameObject);
        }
    }

    public override void outOfHealth()
    {
        if(canBeDestroyed)
        {
            base.outOfHealth();

            GameManager.bubbleDestroyed++;
        }
    }
}
