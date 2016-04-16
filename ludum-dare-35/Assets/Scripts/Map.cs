using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour
{
    public int width = 80;
    public int height = 45;

    public int smoothRuns;

    [Range(0, 1)]
    public float percentWall = 0.4f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    int[,] map;

    public void buildMap()
    {
        map = new int[width, height];

        randomFillMap();

        for (int i = 0; i < smoothRuns; i++)
        {
            smoothMap();
        }

        createGameObjects();

        Debug.Log(((float)transform.childCount) / ((width) * (height)));
    }

    private void randomFillMap()
    {
        System.Random rand = new System.Random();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // If position is on the edge of the map then make sure its a wall
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                // Else randomize the tile
                else
                {
                    map[x, y] = (rand.NextDouble() < percentWall) ? 1 : 0;
                }
            }
        }
    }

    private void smoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int count = getWallCount(x, y);

                if (count > 4)
                {
                    map[x, y] = 1;
                }
                else if (count < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    private int getWallCount(int x, int y)
    {
        int count = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < width && j >= 0 && j < height)
                {
                    if (i != x || j != y)
                    {
                        count += map[i, j];
                    }
                }
                else
                {
                    count++;
                }
            }
        }

        return count;
    }

    private void createGameObjects()
    {
        // Remove old map objects
        while (transform.childCount > 0)
        {
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
        }

        // Create the new map objects
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.position = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
                    go.transform.parent = transform;
                }
            }
        }
    }

}
