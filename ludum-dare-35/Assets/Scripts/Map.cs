using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    public int width = 80;
    public int height = 45;

    public int smoothRuns;

    [Range(0, 1)]
    public float percentWall = 0.4f;

    public GameObject wallPrefab;

    public Transform[] enemyPrefabs;

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

        Transform ground = GameObject.Find("Ground").transform;
        ground.localScale = new Vector3(width / 10f, 1f, height / 10f);
    }

    private void randomFillMap()
    {
        Vector2 center = new Vector2(width / 2f, height / 2f);

        System.Random rand = new System.Random();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // If position is on the edge of the map then make sure its a wall
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                } else if(Vector2.Distance(center, new Vector2(x, y)) < 3)
                {
                    map[x, y] = 0;
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

    public List<int[]> openSpots;

    private void createGameObjects()
    {
        // Remove old map objects
        while (transform.childCount > 0)
        {
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
        }

        openSpots = new List<int[]>();

        // Create the new map objects
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    GameObject go = Instantiate(wallPrefab);
                    go.transform.position = pos(x, y, 0.5f);
                    go.transform.parent = transform;
                }
                else
                {
                    openSpots.Add(new int[] { x, y });
                }
            }
        }

        createEnemies();
    }

    private void createEnemies()
    {
        Transform enemies = GameObject.Find("Enemies").transform;

        // Remove old enemies objects
        while (enemies.childCount > 0)
        {
            GameObject.DestroyImmediate(enemies.GetChild(0).gameObject);
        }

        for (int i = 0; i < 10; i++)
        {
            placeEnemy(enemies);
        }
    }

    private void placeEnemy(Transform parent)
    {
        int randIndex = Random.Range(0, openSpots.Count);
        int randEnemyIndex = Random.Range(0, enemyPrefabs.Length);

        Transform t = Instantiate(enemyPrefabs[randEnemyIndex], pos(openSpots[randIndex][0], openSpots[randIndex][1], 0), Quaternion.identity) as Transform;
        t.parent = parent;
    }

    private Vector3 pos(int x, int y, float aboveGround)
    {
        return new Vector3(-width / 2 + x + .5f, aboveGround, -height / 2 + y + .5f);
    }

}
