using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class MapBuilder : MonoBehaviour{
	public int mapWidth = 30;
	public int mapHeight = 30;
	public int numberOfRooms = 20;
	public bool overlappingRooms = false;

	public Transform hero;
	public Transform baseBuilding;

	public int[] baseRoomSize = new [] {9, 9};
	public int[] roomWidthRange = new [] {4, 8};
	public int[] roomHeightRange = new [] {4, 8};
	public int[] innerHallwayWidthRange = new [] {1, 1};

	[System.NonSerialized]
	public float tileSize = 1f;

	public MapData map;
	public List<Room> rooms;

	private GameManager gm;

	// Use this for initialization
	void Start () {
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

	public MapData build() {
		map = new MapData (mapWidth, mapHeight, tileSize);
		
		rooms = new List<Room>();

		createBaseRoom ();
		
		// Randomly create the rooms
		for (int i = 0; i < numberOfRooms; i++) {
			/* Add 1 because Random.Range() for ints excludes the max value */
			int roomWidth = Random.Range(roomWidthRange[0], roomWidthRange[1] + 1);
			int roomHeight = Random.Range(roomHeightRange[0], roomHeightRange[1] + 1);

			int roomX = Random.Range(0, (map.width/2) - (roomWidth/2));
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

		mirrorMap ();

		placeGameObjs ();

		createMapColliders ();

		return map;
	}

	private void createBaseRoom() {
		int roomWidth = baseRoomSize [0];
		int roomHeight = baseRoomSize [1];

		int roomX = 0;
		//int roomX = Random.Range(0, (map.width/4) - (roomWidth/2));
		
		int roomY = Random.Range(0, map.height - roomHeight + 1);
		
		Room r = new Room (roomX, roomY, roomWidth, roomHeight);

		createRoom (r);
	}
	
	private bool roomCollides(Room r) {
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

	private void mirrorMap () {
		for (int x = 0; x < map.width / 2; x++) {
			for (int y = 0; y < map.height; y++) {
				map.tiles [map.width - 1 - x, y] = map.tiles [x, y];
			}
		}

		connectMirroredMap ();
	}

	private void connectMirroredMap ()
	{
		List<Room> sortedRooms = rooms.OrderByDescending(r=>r.maxX).ToList();

		for (int i = 0; i < sortedRooms.Count; i++) {
			int middleColumn = map.width / 2;
			int walkableCount = 0;

			for (int j = 0; j < map.height; j++) {
				if(map.tiles[middleColumn, j] == 1) {
					walkableCount++;
				}
			}

			if(walkableCount < innerHallwayWidthRange[1] * 2) {
				connectMirroredRooms (sortedRooms[i]);
			} else {
				break;
			}
		}
	}

	void connectMirroredRooms (Room r)
	{
		int connectionWidth = map.width - 2 * r.x;
		for (int x = 1; x < connectionWidth - 1; x++) {
			for (int y = 1; y < r.height - 1; y++) {
				setHallwayTile (x + r.x, y + r.y);
			}
		}
	}

	private GameObject mapObjs;

	private void placeGameObjs() {
		mapObjs = GameObject.Find ("MapObjects");
		if (mapObjs != null) {
			DestroyImmediate(mapObjs);
		}
		mapObjs = new GameObject ("MapObjects");

		placeBaseAndHero ();
	}

	private void placeBaseAndHero() {
		Room r = rooms [0];

		//Place player objects
		Vector3 pos = placeBase (r.centerX - 1, r.centerY, TeamMember.TEAM_1);

		pos.x += 3*tileSize;
		Hero startHero = placeHero (pos, TeamMember.TEAM_1);
		gm.selectHero (startHero);
		placeHero (pos + Vector3.down*tileSize, TeamMember.TEAM_1);
		placeHero (pos + (0.5f*Vector3.down*tileSize) + (Vector3.right*tileSize), TeamMember.TEAM_1);

		//Place enemy objects
		placeBase (map.width - 1 - (r.centerX - 1), r.centerY, TeamMember.TEAM_2);

		pos.x = (map.width)*tileSize - pos.x;
		Hero enemyHero = placeHero (pos, TeamMember.TEAM_2);

		Vector3[] patrolNodes = new Vector3[2];
		patrolNodes [0] = new Vector3 (pos.x, pos.y + 3 * tileSize, pos.z);
		patrolNodes [1] = new Vector3 (pos.x + 6 * tileSize, pos.y + 3 * tileSize, pos.z);
//		patrolNodes [2] = new Vector3 (pos.x + 6 * tileSize, pos.y - 3 * tileSize, pos.z);
//		patrolNodes [3] = new Vector3 (pos.x, pos.y - 3 * tileSize, pos.z);

		LinePath patrolPath = new LinePath (patrolNodes);
		patrolPath.draw ();

		enemyHero.patrolPath = patrolPath;
		HealthBar hb = enemyHero.GetComponent<HealthBar> ();
		hb.barMax = 10000f;
		hb.barProgress = 10000f;

		enemyHero = placeHero (pos + Vector3.down*tileSize, TeamMember.TEAM_2);
		hb = enemyHero.GetComponent<HealthBar> ();
		hb.barMax = 10000f;
		hb.barProgress = 10000f;
	}

	private Vector3 placeBase (int x, int y, Color teamColor) {
		Vector3 pos = map.mapToWorldPoint (x, y);
		Transform t = Instantiate (baseBuilding, pos, Quaternion.identity) as Transform;
		t.parent = mapObjs.transform;

		t.GetComponent<Base> ().teamId = teamColor;

		map.placeBuilding (t.GetComponent<Base> (), x, y);

		return pos;
	}

	Hero placeHero (Vector3 pos, Color teamColor)
	{
		Transform t = Instantiate (hero, pos, Quaternion.identity) as Transform;
		t.parent = mapObjs.transform;

		Hero h = t.GetComponent<Hero> ();
		h.teamId = teamColor;

		gm.addHero (h);

		return h;
	}

	private GameObject mapColliders;
	
	private void createMapColliders() {
		mapColliders = GameObject.Find ("MapColliders");
		if (mapColliders != null) {
			DestroyImmediate(mapColliders);
		}
		mapColliders = new GameObject ("MapColliders");
		
		bool[,] visited = new bool[map.width, map.height];

		for(int y = 0; y < map.height; y++) {
			int[] startPos = null;

			for(int x = 0; x < map.width; x++) {
				if(visited[x, y] || map.tiles [x, y] != 2) {
					if(startPos != null && (x-startPos[0]) > 1) {
						createCollider(startPos, new int[] {x-1, y});
						visited[x-1, y] = true;
					}

					startPos = null;
				} else {
					if(startPos == null) {
						startPos = new int[] {x, y};
					} else {
						visited[x-1, y] = true;
					}
				}
			}

			if(startPos != null && (map.width-startPos[0]) > 1) {
				createCollider(startPos, new int[] {map.width-1, y});
				visited[map.width-1, y] = true;
			}
		}

		for(int x = 0; x < map.width; x++) {
			int[] startPos = null;

			for(int y = 0; y < map.height; y++) {
				if(visited[x, y] || map.tiles [x, y] != 2) {
					if(startPos != null && (y-startPos[1]) > 1) {
						createCollider(startPos, new int[] {x, y-1});
						visited[x, y-1] = true;
					}
					
					startPos = null;
				} else {
					if(startPos == null) {
						startPos = new int[] {x, y};
					} else {
						visited[x, y-1] = true;
					}
				}
			}
			
			if(startPos != null && (map.height-startPos[1]) > 1) {
				createCollider(startPos, new int[] {x, map.height-1});
				visited[x, map.height-1] = true;
			}
		}

		for (int x = 0; x < map.width; x++) {
			for (int y = 0; y < map.height; y++) {
				if(!visited[x, y] && map.tiles [x, y] == 2) {
					createCollider(new int[] {x, y}, new int[] {x, y});
				}
			}
		}
	}

	void createCollider (int[] startPos, int[] endPos)
	{
		GameObject go = new GameObject("MapCol");
		BoxCollider2D col = go.AddComponent<BoxCollider2D>();

		Vector2 size = new Vector2 ();
		size.x = (endPos [0] - startPos [0] + 1) * tileSize;
		size.y = (endPos [1] - startPos [1] + 1) * tileSize;
		col.size = size;

		Vector3 position = new Vector3 ();
		position.x = startPos [0] * tileSize + size.x / 2f;
		position.y = startPos [1] * tileSize + size.y / 2f;
		go.transform.position = position;

		go.transform.parent = mapColliders.transform;
	}
}
