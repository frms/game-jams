using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour {

    public GameObject[] prefabs;

    public float cellSize = 1f;

    public List<int[,]> levels;

    public Transform ground;

    private int[,] curLevel;
    private int curWidth;
    private int curHeight;

	// Use this for initialization
	void Start () {
        levels = new List<int[,]>();

        levels.Add(new int[,] {
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 }
        });

        loadLevel(0);
    }

    private void loadLevel(int index)
    {
        curLevel = levels[index];
        curWidth = curLevel.GetLength(1);
        curHeight = curLevel.GetLength(0);

        for (int y = 0; y < curHeight; y++)
        {
            for (int x = 0; x < curWidth; x++)
            {
                GameObject go = prefabs[curLevel[curHeight - 1 - y, curWidth - 1 - x]];

                if(go != null)
                {
                    Vector3 pos = new Vector3();
                    pos.x = (cellSize / 2) + cellSize * x;
                    pos.z = (cellSize / 2) + cellSize * y;

                    Instantiate(go, pos, Quaternion.identity);
                }
            }
        }

        float worldWidth = curWidth * cellSize;
        float worldHeight = curHeight * cellSize;

        ground.position = new Vector3(worldWidth / 2f, 0, worldHeight / 2f);

        /* Divide by 10  because the ground plane is assumed to be 10x10 by default */
        ground.localScale = new Vector3(worldWidth, 1f, worldHeight) / 10f;

        centerCamera();
    }

    private void centerCamera()
    {
        float halfMapWidth = ((curWidth) * 0.5f * cellSize);
        float halfMapHeight = ((curHeight) * 0.5f * cellSize);

        float cameraPadding = (1f * cellSize);

        float vertHalfFOV = Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad;

        float z1 = (halfMapWidth + cameraPadding) / (Mathf.Tan(vertHalfFOV) * Camera.main.aspect);
        float z2 = (halfMapHeight + cameraPadding) / Mathf.Tan(vertHalfFOV);

        Vector3 camPos = new Vector3();
        camPos.x = halfMapWidth;
        camPos.y = Mathf.Max(z1, z2);
        camPos.z = halfMapHeight;
        
        Camera.main.transform.position = camPos;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
