using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    public int width = 80;
    public int height = 45;

    public int smoothRuns = 1;

    [Range(0, 1)]
    public float percentWall = 0.45f;

    public GameObject wallPrefab;

    public float centerEmptyDist = 1f;

    public int numEnemies = 40;

    public Transform[] enemyPrefabs;
    private Transform enemies;

    int[,] map;

    private Transform _player;

    private Transform player
    {
        get
        {
            if (_player == null)
            {
                GameObject go = GameObject.Find("Player");

                if (go != null)
                    _player = go.transform;
            }

            return _player;
        }
    }

    void Start()
    {
        buildMap();
    }

    public void buildMap()
    {
        map = new int[width, height];

        randomFillMap();

        for (int i = 0; i < smoothRuns; i++)
        {
            smoothMap();
        }

        GameObject go = GameObject.Find("Enemies");
        if(go == null)
        {
            go = new GameObject();
            go.name = "Enemies";
        }
        enemies = go.transform;

        createGameObjects();
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
                else if (Vector2.Distance(player.position, pos(x, y, 0)) < centerEmptyDist)
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
                    go.transform.position = pos(x, y, 0f);
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
        // Remove old enemies objects
        while (enemies.childCount > 0)
        {
            GameObject.DestroyImmediate(enemies.GetChild(0).gameObject);
        }

        if(enemyPrefabs.Length > 0)
        {
            for (int i = 0; i < numEnemies; i++)
            {
                int randEnemyIndex = Random.Range(0, enemyPrefabs.Length);
                placeEnemy(randEnemyIndex);
            }
        }
    }

    private void placeEnemy(int enemyIndex)
    {
        int randIndex = Random.Range(0, openSpots.Count);
        Vector3 p = pos(openSpots[randIndex][0], openSpots[randIndex][1], 0);

        Collider2D[] cols = Physics2D.OverlapAreaAll(p, p + new Vector3(0.45f, 0.45f, 0.45f));

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].tag == "Player" || cols[i].tag == "Enemy")
            {
                return;
            }
        }

        Quaternion orientation = Quaternion.Euler(0, Random.value * 360f, 0);

        Transform t = Instantiate(enemyPrefabs[enemyIndex], p, orientation) as Transform;
        t.parent = enemies;
    }

    private Vector3 pos(int x, int y, float aboveGround)
    {
        return new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, aboveGround);
    }

    public void spawn(int i)
    {
        if (player == null)
        {
            return;
        }

        if (i == 0)
        {
            placeRandomWall();
        }
        else
        {
            placeEnemy(i - 1);
        }
    }

    private void placeRandomWall()
    {
        int randIndex = Random.Range(0, openSpots.Count);
        Vector3 p = pos(openSpots[randIndex][0], openSpots[randIndex][1], 0f);

        Collider2D[] cols = Physics2D.OverlapAreaAll(p, p + new Vector3(0.45f, 0.45f, 0.45f));

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].tag == "Player" || cols[i].tag == "Enemy")
            {
                return;
            }
        }

        openSpots.RemoveAt(randIndex);

        GameObject go = Instantiate(wallPrefab);
        go.transform.position = p;
        go.transform.parent = transform;
    }

}
