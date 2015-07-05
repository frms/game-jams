using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapData {
	public int width;
	public int height;
	public float tileSize;

	private int[,] map;

	// Indexer declaration.
	public int this[int x, int y]
	{
		get
		{
			return map[x, y];
		}
		
		set
		{
			map[x, y] = value;
		}
	}
	
	public MapData(int width, int height, float tileSize) {
		this.width = width;
		this.height = height;
		this.tileSize = tileSize;
		
		map = new int[width,height];
	}


	public bool isConnectedMap (int tileType) {
		bool result = true;

		int startX = -1;
		int startY = -1;
		
		int count = 0;
		
		for(int x = 0; x < width; x++) {
			for(int y = 0; y < height; y++) {
				if(map[x, y] == tileType) {
					count++;
					
					if(count == 1) {
						startX = x;
						startY = y;
					}
				}
			}
		}

		if (count > 0) {
			int connectedCount = numberOfConnectedTiles (startX, startY);

			result = (count == connectedCount);
		}

		return result;
	}

	/* Searches and counts the number of connected tiles of the same type at the given starting point.
	 * Only searches the standard 4 directions and not all 8.
	 */
	public int numberOfConnectedTiles(int startX, int startY) {
		int tileType = map [startX, startY];

		bool[,] visited = new bool[width, height];

		List<int> list = new List<int> ();
		list.Add (startX * height + startY);

		int count = 0;

		while (list.Count > 0) {
			int index = list[list.Count-1];
			list.RemoveAt(list.Count-1);

			int x = index / height;
			int y = index % height;

			if(!visited[x, y]) {
				visited[x, y] = true;

				if(map[x,y] == tileType) {
					count++;

					if (x > 0 && !visited[x - 1, y]) {
						list.Add ((x - 1) * height + y);
					}

					if (x < this.height - 1 && !visited[x + 1, y]) {
						list.Add ((x + 1) * height + y);
					}

					if (y > 0 && !visited[x, y - 1]) {
						list.Add (x * height + (y - 1));
					}
					
					if (y < this.height - 1 && !visited[x, y + 1]) {
						list.Add (x * height + (y + 1));
					}
				}
			}
		}

		return count;
	}

	public List<Connection> getConnectedNodes(int[] node) {
		List<Connection> ret = new List<Connection> ();
		
		int x = node [0];
		int y = node [1];
		
		bool left, right, down, up;
		
		if (left = (x > 0 && map [x - 1, y] == 1)) {
			ret.Add(new Connection( x - 1, y, Connection.DEFAULT_COST ));
		}
		
		if (right = (x + 1 < width && map [x + 1, y] == 1)) {
			ret.Add(new Connection( x + 1, y, Connection.DEFAULT_COST ));
		}
		
		if (down = (y > 0 && map [x, y - 1] == 1)) {
			ret.Add(new Connection( x, y - 1, Connection.DEFAULT_COST ));
		}
		
		if (up = (y + 1 < height && map [x, y + 1] == 1)) {
			ret.Add(new Connection( x, y + 1, Connection.DEFAULT_COST ));
		}
		
		if (left && down && map [x - 1, y - 1] == 1) {
			ret.Add(new Connection( x - 1, y - 1, Connection.DIAGONAL_COST ));
		}
		
		if (right && down && map [x + 1, y - 1] == 1) {
			ret.Add(new Connection( x + 1, y - 1, Connection.DIAGONAL_COST ));
		}
		
		if (left && up && map [x - 1, y + 1] == 1) {
			ret.Add(new Connection( x - 1, y + 1, Connection.DIAGONAL_COST ));
		}
		
		if (right && up && map [x + 1, y + 1] == 1) {
			ret.Add(new Connection( x + 1, y + 1, Connection.DIAGONAL_COST ));
		}
		
		return ret;
	}

	public Vector3 mapToWorldPoint(int x, int y) {
		Vector3 pos = new Vector3();
		pos.x = (tileSize/2) + tileSize*x; 
		pos.y = (tileSize/2) + tileSize*y;

		return pos;
	}

	public int[] worldToMapPoint(Vector2 pos) {
		int[] coords = new int[2];
		coords [0] = (int) (pos.x / tileSize);
		coords [1] = (int) (pos.y / tileSize);

		return coords;
	}
}

public class Connection {
	public static float DEFAULT_COST = 1;
	public static float DIAGONAL_COST = Mathf.Sqrt (2) * DEFAULT_COST;
	
	public int[] toNode;
	public float cost;
	
	public Connection(int x, int y, float cost) {
		this.toNode = new int[] { x, y };
		this.cost = cost;
	}
}
