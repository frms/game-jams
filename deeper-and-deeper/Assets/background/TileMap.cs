using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap : MonoBehaviour {
	public Transform backgroundRect;

	public int mapHeight = 40;
	private int mapWidth;

	public bool overlappingRooms = false;
	public int numberOfRooms = 20;
	
	public int[] roomWidthRange = new [] {4, 8};
	
	public int[] roomHeightRange = new [] {4, 8};

	private MapData map;
	private List<Room> rooms;

	// Use this for initialization
	void Start () {
		float height = Camera.main.orthographicSize * 2;
		float width = height * Camera.main.aspect;

		float tileSize = height / mapHeight;

		mapWidth = Mathf.CeilToInt (width * 1.1f / tileSize);

		buildMapData ();

		for(int i = 0; i < rooms.Count; i++) {
			Room r = rooms[i];

			float x = ( r.centerX - ((float)(mapWidth))/2) * tileSize;
			float y = ( r.centerY - ((float)(mapHeight))/2) * tileSize;
			Vector3 pos = new Vector3(x, y, 0);

			Transform clone = Instantiate(backgroundRect, pos, Quaternion.identity) as Transform;
			clone.localScale = new Vector3(r.width * tileSize, r.height * tileSize, 1);
		}
	}

	private void buildMapData() {
		map = new MapData (mapWidth, mapHeight);

		rooms = new List<Room>();
		
		for (int i = 0; i < numberOfRooms; i++) {
			int roomWidth = Random.Range(roomWidthRange[0], roomWidthRange[1]);
			int roomHeight = Random.Range(roomHeightRange[0], roomHeightRange[1]);

			/* Add 1 because Random.Range() for ints excludes the max value */
			int roomX = Random.Range(0, map.width - roomWidth + 1);
			int roomY = Random.Range(0, map.height - roomHeight + 1);
			
			Room r = new Room (roomX, roomY, roomWidth, roomHeight);
			
			if(overlappingRooms || !roomCollides(r)) {
				createRoom (r);
			}
		}

	}
	
	public bool roomCollides(Room r) {
		foreach (Room r2 in rooms) {
			if(r.collidesWith(r2)) {
				return true;
			}
		}
		
		return false;
	}
	
	private void createRoom(Room r) {
		for(int x = 0; x < r.width; x++) {
			for(int y = 0; y < r.height; y++) {
				if(x == 0 || x == r.width-1 || y == 0 || y == r.height-1) {
					map[x + r.x, y + r.y] = 2;
				} else {
					map[x + r.x, y + r.y] = 1;
				}
			}
		}
		
		rooms.Add (r);
	}
}
