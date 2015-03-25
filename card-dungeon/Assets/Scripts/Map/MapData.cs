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

	public Vector3 getPosition(int x, int y) {
		Vector3 pos = new Vector3();
		pos.x = (tileSize/2) + tileSize*x; 
		pos.y = (tileSize/2) + tileSize*y;
		
		return pos;
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
