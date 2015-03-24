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

	public List<int[]> getConnectedNodes(int[] node) {
		List<int[]> ret = new List<int[]> ();

		int x = node [0];
		int y = node [1];

		if (x > 0 && map [x - 1, y] == 1) {
			ret.Add(new [] { x - 1, y });
		}
		
		if (x + 1 < width && map [x + 1, y] == 1) {
			ret.Add(new [] { x + 1, y });
		}
		
		if (y > 0 && map [x, y - 1] == 1) {
			ret.Add(new [] { x, y - 1 });
		}
		
		if (y + 1 < height && map [x, y + 1] == 1) {
			ret.Add(new [] { x, y + 1 });
		}
		
//		if (x > 0 && y > 0 && map [x - 1, y - 1] == 1) {
//			ret.Add(new [] { x - 1, y - 1 });
//		}
//		
//		if (x + 1 < width && y > 0 && map [x + 1, y - 1] == 1) {
//			ret.Add(new [] { x + 1, y - 1 });
//		}
//		
//		if (x > 0 && y + 1 < height && map [x - 1, y + 1] == 1) {
//			ret.Add(new [] { x - 1, y + 1 });
//		}
//		
//		if (x + 1 < width && y + 1 < height && map [x + 1, y + 1] == 1) {
//			ret.Add(new [] { x + 1, y + 1 });
//		}

		return ret;
	}

	public Vector3 getPosition(int x, int y) {
		Vector3 pos = new Vector3();
		pos.x = (tileSize/2) + tileSize*x; 
		pos.y = (tileSize/2) + tileSize*y;
		
		return pos;
	}
}
