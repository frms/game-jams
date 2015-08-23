using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {
	public int mapWidth = 30;
	public int mapHeight = 30;
	public float tileSize = 1f;

	public int numberOfRooms = 20;
	public bool overlappingRooms = false;
	public bool mirrorMap = false;
	public int[] roomWidthRange = new [] {4, 8};
	public int[] roomHeightRange = new [] {4, 8};
	public int[] innerHallwayWidthRange = new [] {1, 1};

	private List<Room> rooms;

	public Transform[] tiles;

	public Transform player;
	public Transform[] enemyTypes;

	public static MapData map;

	// Use this for initialization
	void Awake () {
		//buildManualMap();

		build();
		placeUnits();

		for(int x = 0; x < map.width; x++) {
			for(int y = 0; y < map.height; y++) {
				int i = map.tiles[x,y];

				if(i != 0) {
					placeThing(x, y, tiles[ i ]);
				}
			}
		}

		centerCamera();
	}

	public void buildManualMap() {
		map = new MapData (mapWidth, mapHeight, tileSize);
		
		for(int x = 0; x < map.width; x++) {
			for(int y = 0; y < map.height; y++) {
				map.tiles[x,y] = 1;
			}
		}
		
		map.tiles[(map.width / 2) - 5, map.height / 2 + 3] = 2;
		
		player.position = map.mapToWorldPoint(map.width / 2, map.height / 2);
		
		Transform enemy1 = placeThing((map.width / 2) + 5, map.height / 2, enemyTypes[0]);
		Enemy1 e1 = enemy1.GetComponent<Enemy1>();
		e1.isLooping = true;
		e1.currentPath = AStar.findPath(map, enemy1.position, enemy1.position + Vector3.up*map.tileSize*7, null, 0);
		
		placeThing((map.width / 2) - 4, map.height / 2 - 1, enemyTypes[0]);
		
		placeThing((map.width / 2) - 6, map.height / 2 + 3, enemyTypes[1]);
	}

	private Transform placeThing (int x, int y, Transform orig) {
		Vector3 pos = map.mapToWorldPoint (x, y);

		Transform t = Instantiate (orig, pos, Quaternion.identity) as Transform;
		t.parent = transform;

		return t;
	}

	private void centerCamera() {
		//float halfMapWidth = (map.width * 0.5f * map.tileSize);
		//float halfMapHeight = (map.height * 0.5f * map.tileSize);
		float halfMapHeight = (6.5f * map.tileSize);
		
		float vertHalfFOV = Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad;
		
		//float z1 = halfMapWidth / (Mathf.Tan(vertHalfFOV) * Camera.main.aspect);
		float z2 = halfMapHeight / Mathf.Tan(vertHalfFOV);
		
		Vector3 camPos = new Vector3();
//		camPos.x = halfMapWidth;
//		camPos.y = halfMapHeight;
//		camPos.z = -1f * Mathf.Max(z1, z2);
		camPos.x = player.transform.position.x;
		camPos.y = player.transform.position.y;
		camPos.z = -z2;
		
		Camera.main.transform.position = camPos;
	}

	public MapData build() {
		map = new MapData (mapWidth, mapHeight, tileSize);
		
		rooms = new List<Room>();
		
		// Randomly create the rooms
		for (int i = 0; i < numberOfRooms; i++) {
			/* Add 1 because Random.Range() for ints excludes the max value */
			int roomWidth = Random.Range(roomWidthRange[0], roomWidthRange[1] + 1);
			int roomHeight = Random.Range(roomHeightRange[0], roomHeightRange[1] + 1);
			
			int roomX;
			if(mirrorMap) {
				roomX = Random.Range(0, (map.width/2) - (roomWidth/2));
			} else {
				roomX = Random.Range(0, map.width - roomWidth + 1);
			}
			
			int roomY = Random.Range(0, map.height - roomHeight + 1);
			
			Room r = new Room (roomX, roomY, roomWidth, roomHeight);
			
			if(overlappingRooms || !roomCollides(r)) {
				createRoom (r);
			}
		}
		
		// Randomly connect the rooms
		for (int i = 0; i < rooms.Count; i++) {
			int j = i + Random.Range(1, rooms.Count);
			j %= rooms.Count;
			
			createHallway (rooms [i], rooms [j]);
		}
		
		// Make sure there are no isolated groups of rooms
		connectAllRooms ();
		
		verifyBorderWalls ();
		
		if (mirrorMap) {
			for (int x = 0; x < map.width/2; x++) {
				for (int y = 0; y < map.height; y++) {
					map.tiles [map.width - 1 - x, y] = map.tiles [x, y];
				}
			}
			
			if(!map.isConnectedTiles(1)) {
				connectMirroredMap ();
			}
		}
		
		return map;
	}
	
	public bool roomCollides(Room r) {
		foreach (Room r2 in rooms) {
			if(r.innerRoomCollidesWith(r2)) {
				return true;
			}
		}
		
		return false;
	}
	
	private void createRoom(Room r) {
		for(int x = 0; x < r.width; x++) {
			for(int y = 0; y < r.height; y++) {
				if(x == 0 || x == r.width-1 || y == 0 || y == r.height-1) {
					map.tiles[x + r.x, y + r.y] = 2;
				} else {
					map.tiles[x + r.x, y + r.y] = 1;
				}
			}
		}
		
		rooms.Add (r);
	}
	
	private void createHallway(Room r1, Room r2) {
		int dx = (r1.centerX < r2.centerX) ? 1 : -1;
		int dy = (r1.centerY < r2.centerY) ? 1 : -1;
		
		int innerWidth = Random.Range (innerHallwayWidthRange [0], innerHallwayWidthRange [1] + 1);
		
		int width1 = -1 * innerWidth / 2;
		int width2 = innerWidth / 2;
		if (innerWidth % 2 == 1) {
			width2 += 1;
		}
		
		for (int y = r1.centerY + width1; y < r1.centerY + width2; y++) {
			for (int x = r1.centerX; x != r2.centerX + (dx * innerWidth / 2); x += dx) {
				setHallwayTile(x, y);
			}
		}
		
		for (int x = r2.centerX + width1; x < r2.centerX + width2; x++) {
			for (int y = r1.centerY; y != r2.centerY + (dy * innerWidth / 2); y += dy) {
				setHallwayTile(x, y);
			}
		}
		
		r1.addConnection (r2);
		r2.addConnection (r1);
	}
	
	private void setHallwayTile(int x, int y) {
		if (x > 0 && x < map.width && y > 0 && y < map.height) {
			map.tiles [x, y] = 1;
			
			if (x > 0 && map.tiles [x - 1, y] == 0) {
				map.tiles [x - 1, y] = 2;
			}
			
			if (x + 1 < map.width && map.tiles [x + 1, y] == 0) {
				map.tiles [x + 1, y] = 2;
			}
			
			if (y > 0 && map.tiles [x, y - 1] == 0) {
				map.tiles [x, y - 1] = 2;
			}
			
			if (y + 1 < map.height && map.tiles [x, y + 1] == 0) {
				map.tiles [x, y + 1] = 2;
			}
			
			if (x > 0 && y > 0 && map.tiles [x - 1, y - 1] == 0) {
				map.tiles [x - 1, y - 1] = 2;
			}
			
			if (x + 1 < map.width && y > 0 && map.tiles [x + 1, y - 1] == 0) {
				map.tiles [x + 1, y - 1] = 2;
			}
			
			if (x > 0 && y + 1 < map.height && map.tiles [x - 1, y + 1] == 0) {
				map.tiles [x - 1, y + 1] = 2;
			}
			
			if (x + 1 < map.width && y + 1 < map.height && map.tiles [x + 1, y + 1] == 0) {
				map.tiles [x + 1, y + 1] = 2;
			}
		}
	}
	
	private void connectAllRooms() {
		List<List<Room>> connectedRooms = new List<List<Room>>();
		
		HashSet<Room> visited = new HashSet<Room> ();
		
		for(int i = 0; i < rooms.Count; i++) {
			if(!visited.Contains(rooms[i])) {
				connectedRooms.Add(getConnectedRooms(rooms[i], visited));
			}
		}
		
		if (connectedRooms.Count > 1) {
			for(int i = 0; i < connectedRooms.Count-1; i++) {
				Room r1 = connectedRooms[i][0];
				Room r2 = connectedRooms[i+1][0];
				
				createHallway(r1, r2);
			}
		}
	}
	
	private List<Room> getConnectedRooms(Room startRoom, HashSet<Room> visited) {
		List<Room> connectedRooms = new List<Room> ();
		
		List<Room> list = new List<Room> ();
		list.Add (startRoom);
		
		while (list.Count > 0) {
			Room r = list[list.Count-1];
			list.RemoveAt(list.Count-1);
			
			if(!visited.Contains(r)) {
				visited.Add(r);
				connectedRooms.Add(r);
				
				foreach(Room c in r.connectedTo) {
					if(!visited.Contains(c)) {
						list.Add(c);
					}
				}
			}
		}
		
		return connectedRooms;
	}
	
	/** 
	 * Make sure any hallways against the borders of the map are fully 
	 * enclosed in wall times (even if that means we have to shave off 
	 * some tiles from the hallway 
	 */
	private void verifyBorderWalls() {
		for (int x = 0; x < map.width; x++) {
			setBorderWall(x, 0);
			setBorderWall(x, map.height-1);
		}
		
		for (int y = 0; y < map.height; y++) {
			setBorderWall(0, y);
			setBorderWall(map.width-1, y);
		}
	}
	
	private void setBorderWall(int x, int y) {
		if (map.tiles [x, y] == 1) {
			map.tiles[x, y] = 2;
		}
	}
	
	private void connectMirroredMap ()
	{
		int currentMaxX = -1;
		Room currentRoom = null;
		for (int i = 0; i < rooms.Count; i++) {
			if (rooms [i].maxX > currentMaxX) {
				currentMaxX = rooms [i].maxX;
				currentRoom = rooms [i];
			}
		}
		int connectionWidth = map.width - 2 * currentRoom.x;
		for (int x = 1; x < connectionWidth - 1; x++) {
			for (int y = 1; y < currentRoom.height - 1; y++) {
				setHallwayTile (x + currentRoom.x, y + currentRoom.y);
			}
		}
	}

	public void placeUnits() {
		int playerRoomIndex = Random.Range(0, rooms.Count);

		Room randomRoom = rooms[playerRoomIndex];
		player.position = map.mapToWorldPoint(randomRoom.centerX, randomRoom.centerY);

		for(int i = 0; i < rooms.Count; i++) {
			if(i != playerRoomIndex) {
				placeThing(rooms[i].centerX, rooms[i].centerY, enemyTypes[1]);
			}
		}
	}
}
