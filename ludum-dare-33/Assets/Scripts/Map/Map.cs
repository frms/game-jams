using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {
	public int mapWidth = 30;
	public int mapHeight = 30;
	public float tileSize = 1f;

	public Transform[] tiles;

	public Transform player;
	public Transform enemy;

	public static MapData map;

	// Use this for initialization
	void Awake () {
		map = new MapData (mapWidth, mapHeight, tileSize);

		for(int x = 0; x < map.width; x++) {
			for(int y = 0; y < map.height; y++) {
				placeThing(x, y, tiles[ map.tiles[x,y] ]);
			}
		}

		player.position = map.mapToWorldPoint(map.width / 2, map.height / 2);

		Transform enemy1 = placeThing((map.width / 2) + 5, map.height / 2, enemy);
		Enemy1 e1 = enemy1.GetComponent<Enemy1>();
		e1.isLooping = true;
		e1.currentPath = AStar.findPath(map, enemy1.position, enemy1.position + Vector3.up*map.tileSize*7, null, 0);

		placeThing((map.width / 2) - 4, map.height / 2 - 1, enemy);

		centerCamera();
	}

	private Transform placeThing (int x, int y, Transform orig) {
		Vector3 pos = map.mapToWorldPoint (x, y);

		Transform t = Instantiate (orig, pos, Quaternion.identity) as Transform;
		t.parent = transform;

		return t;
	}

	private void centerCamera() {
		float halfMapWidth = (map.width * 0.5f * map.tileSize);
		float halfMapHeight = (map.height * 0.5f * map.tileSize);
		
		float vertHalfFOV = Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad;
		
		float z1 = halfMapWidth / (Mathf.Tan(vertHalfFOV) * Camera.main.aspect);
		float z2 = halfMapHeight / Mathf.Tan(vertHalfFOV);
		
		Vector3 camPos = new Vector3();
		camPos.x = halfMapWidth;
		camPos.y = halfMapHeight;
		camPos.z = -1f * Mathf.Max(z1, z2);
		
		Camera.main.transform.position = camPos;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
