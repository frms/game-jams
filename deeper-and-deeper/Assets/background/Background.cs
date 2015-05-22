using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Background : MonoBehaviour {
	public Transform backgroundRect;

	public int mapHeight = 40;
	private int mapWidth;
	private float tileSize;

	public bool overlappingRooms = false;
	public int numberOfRooms = 20;
	
	public int[] roomWidthRange = new [] {4, 8};
	
	public int[] roomHeightRange = new [] {4, 8};


	private Queue<List<Room>> sections = new Queue<List<Room>>();

	// Use this for initialization
	void Start () {
		float height = Camera.main.orthographicSize * 2;
		float width = height * Camera.main.aspect;

		tileSize = height / mapHeight;

		mapWidth = Mathf.CeilToInt (width * 1.1f / tileSize);

//		for (int i = 0; i < 3; i++) {
//			buildSection (mapHeight * i);
//		}

		buildSection (0);
		buildSection (mapHeight);
	}

	private int lastSectionBuilt;

	public void buildSection(int startY) {
		List<Room> rooms = buildMapData (startY);
		
		for(int i = 0; i < rooms.Count; i++) {
			Room r = rooms[i];
			
			float x = ( r.centerX - ((float)(mapWidth))/2) * tileSize;
			float y = ( r.centerY - ((float)(mapHeight))/2) * tileSize;
			Vector3 pos = new Vector3(x, y, 0);
			
			Transform clone = Instantiate(backgroundRect, pos, Quaternion.identity) as Transform;
			clone.localScale = new Vector3(r.width * tileSize, r.height * tileSize, 1);
		}

		lastSectionBuilt = startY;
		sections.Enqueue (rooms);
	}

	private List<Room> buildMapData(int startY) {
		List<Room> rooms = new List<Room>();
		
		for (int i = 0; i < numberOfRooms; i++) {
			int roomWidth = Random.Range(roomWidthRange[0], roomWidthRange[1]);
			int roomHeight = Random.Range(roomHeightRange[0], roomHeightRange[1]);

			/* Add 1 because Random.Range() for ints excludes the max value */
			int roomX = Random.Range(0, mapWidth - roomWidth + 1);
			int roomY = Random.Range(startY, startY + mapHeight + 1);
			
			Room r = new Room (roomX, roomY, roomWidth, roomHeight);
			
			if(overlappingRooms || !roomCollides(r, rooms)) {
				rooms.Add (r);
			}
		}

		return rooms;
	}
	
	public bool roomCollides(Room r, List<Room> rooms) {
		foreach (Room r2 in rooms) {
			if(r.collidesWith(r2)) {
				return true;
			}
		}
		
		return false;
	}

}
