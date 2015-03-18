using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap : MonoBehaviour {
	public Transform floor;
	public Transform wall;
	public Transform cardPickUp;

	public int mapWidth = 3;
	public int mapHeight = 2;
	public float tileSize = 1.0f;

	public float percentCardsOnGround = 0.2f;
	public List<Card> cards;
	private float cardWeightSum = 0;

	public bool overlappingRooms = false;
	public int numberOfRooms = 20;
	[HideInInspector]
	public int[] roomWidthRange = new [] {4, 8};
	[HideInInspector]
	public int[] roomHeightRange = new [] {4, 8};

	private MapData map;
	private List<Room> rooms;
	private List<int[]> floorTiles;

	private Transform player;

	// Use this for initialization
	void Awake () {
		cardWeightSum = 0;

		cards = new List<Card> ();
		cards.Add (createCard (Color.black));
		cards.Add (createCard (Color.blue));
		cards.Add (createCard (Color.cyan));
		cards.Add (createCard (Color.gray));

		player = GameObject.Find ("Player").transform;

		buildMap ();
	}

	private Card createCard(Color color) {
		Card c = new Card ();
		c.color = color;
		c.dropWeight = 1;
		cardWeightSum += c.dropWeight;
		return c;
	}

	public void buildMap() {
		buildMapData ();
		buildMapTerrain ();
		placeGameBits ();
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

		for (int i = 0; i < rooms.Count; i++) {
			if(!rooms[i].isConnected) {
				int j = i + Random.Range(1, rooms.Count);
				j %= rooms.Count;

				createHallway (rooms [i], rooms [j]);
			}
		}
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
					map[x + r.x, y + r.y] = 2;
				} else {
					map[x + r.x, y + r.y] = 1;
				}
			}
		}
		
		rooms.Add (r);
	}

	private void createHallway(Room r1, Room r2) {
		int x = r1.centerX;
		int y = r1.centerY;

		int dx = (x < r2.centerX) ? 1 : -1;
		int dy = (y < r2.centerY) ? 1 : -1;

		while (x != r2.centerX) {
			setHallwayTile(x, y);
			x += dx;
		}

		while (y != r2.centerY) {
			setHallwayTile(x, y);
			y += dy;
		}
	}

	private void setHallwayTile(int x, int y) {
		map[x, y] = 1;

		if (x > 0 && map [x - 1, y] == 0) {
			map [x - 1, y] = 2;
		}

		if (x + 1 < map.width && map [x + 1, y] == 0) {
			map [x + 1, y] = 2;
		}

		if (y > 0 && map [x, y - 1] == 0) {
			map [x, y - 1] = 2;
		}
		
		if (y + 1 < map.height && map [x, y + 1] == 0) {
			map [x, y + 1] = 2;
		}

		if (x > 0 && y > 0 && map [x - 1, y - 1] == 0) {
			map [x - 1, y - 1] = 2;
		}

		if (x + 1 < map.width && y > 0 && map [x + 1, y - 1] == 0) {
			map [x + 1, y - 1] = 2;
		}

		if (x > 0 && y + 1 < map.height && map [x - 1, y + 1] == 0) {
			map [x - 1, y + 1] = 2;
		}

		if (x + 1 < map.width && y + 1 < map.height && map [x + 1, y + 1] == 0) {
			map [x + 1, y + 1] = 2;
		}
	}

	private void buildMapTerrain() {
		// Destory all current wall game objects
		while(transform.childCount > 0) {
			Transform child = transform.GetChild(0);
			DestroyImmediate(child.gameObject);
		}

		floorTiles = new List<int[]> ();
		
		for (int z = 0; z < map.height; z++) {
			for(int x = 0; x < map.width; x++) {
				if(map[x, z] == 1) {
					createGameObj(x, z, floor);

					int[] loc = new [] { x, z };
					floorTiles.Add(loc);
				} else if(map[x, z] == 2) {
					createGameObj(x, z, wall);
				}
			}
		}
	}

	private Transform createGameObj(int x, int z, Transform t) {
		Vector3 pos = getPosition (x, z);
		
		Transform child = Instantiate(t, pos, t.rotation) as Transform;
		child.parent = transform;

		return child;
	}

	private Vector3 getPosition(int x, int z) {
		Vector3 pos = new Vector3();
		pos.x = (tileSize/2) + tileSize*x; 
		pos.z = (tileSize/2) + tileSize*z;

		return pos;
	}

	private void placeGameBits() {
		int i = Random.Range (0, floorTiles.Count);
		int[] mapPos = floorTiles [i];
		player.position = getPosition (mapPos[0], mapPos[1]);

		for(int x = 0; x < floorTiles.Count; x++) {
			if(Random.value < percentCardsOnGround) {
				mapPos = floorTiles [x];
				Transform child = createGameObj(mapPos[0], mapPos[1], cardPickUp);

				CardPickUp cpu = child.GetComponent<CardPickUp>();
				cpu.setUp(randomCard());
			}
		}
	}

	private Card randomCard() {
		float r = Random.Range (0, cardWeightSum);
		float sum = 0;

		for (int i = 0; i < cards.Count; i++) {
			sum += cards[i].dropWeight;

			if(r < sum) {
				return cards[i];
			}
		}

		return null;	//This should not be reached
	}
}
