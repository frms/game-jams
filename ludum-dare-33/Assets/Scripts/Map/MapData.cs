using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapData {
	public int width;
	public int height;
	public float tileSize;

	public int[,] tiles;
	public MonoBehaviour[,] objs;
	
	public MapData(int width, int height, float tileSize) {
		this.width = width;
		this.height = height;
		this.tileSize = tileSize;
		
		tiles = new int[width,height];
		objs = new MonoBehaviour[width, height];
	}


	public bool isConnectedTiles (int tileType) {
		bool result = true;

		int startX = -1;
		int startY = -1;
		
		int count = 0;
		
		for(int x = 0; x < width; x++) {
			for(int y = 0; y < height; y++) {
				if(tiles[x, y] == tileType) {
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
		int tileType = tiles [startX, startY];

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

				if(tiles[x,y] == tileType) {
					count++;

					if (x > 0 && !visited[x - 1, y]) {
						list.Add ((x - 1) * height + y);
					}

					if (x < this.width - 1 && !visited[x + 1, y]) {
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

	public List<Connection> getConnectedNodes(int[] node, MonoBehaviour target, bool tilesOnly) {
		List<Connection> ret = new List<Connection> ();
		
		int x = node [0];
		int y = node [1];
		
		bool left, right, down, up;
		
		if (left = (x > 0 && isWalkable (x - 1, y, target, tilesOnly) )) {
			ret.Add(new Connection( x - 1, y, Connection.DEFAULT_COST ));
		}
		
		if (right = (x + 1 < width && isWalkable (x + 1, y, target, tilesOnly) )) {
			ret.Add(new Connection( x + 1, y, Connection.DEFAULT_COST ));
		}
		
		if (down = (y > 0 && isWalkable (x, y - 1, target, tilesOnly) )) {
			ret.Add(new Connection( x, y - 1, Connection.DEFAULT_COST ));
		}
		
		if (up = (y + 1 < height && isWalkable (x, y + 1, target, tilesOnly) )) {
			ret.Add(new Connection( x, y + 1, Connection.DEFAULT_COST ));
		}
		
		if (left && down && isWalkable (x - 1, y - 1, target, tilesOnly) ) {
			ret.Add(new Connection( x - 1, y - 1, Connection.DIAGONAL_COST ));
		}
		
		if (right && down && isWalkable (x + 1, y - 1, target, tilesOnly) ) {
			ret.Add(new Connection( x + 1, y - 1, Connection.DIAGONAL_COST ));
		}
		
		if (left && up && isWalkable (x - 1, y + 1, target, tilesOnly) ) {
			ret.Add(new Connection( x - 1, y + 1, Connection.DIAGONAL_COST ));
		}
		
		if (right && up && isWalkable (x + 1, y + 1, target, tilesOnly) ) {
			ret.Add(new Connection( x + 1, y + 1, Connection.DIAGONAL_COST ));
		}
		
		return ret;
	}

	private bool isWalkable(int x, int y, MonoBehaviour target, bool tilesOnly) {
		bool walkable = tiles [x, y] == 0;

		if(tilesOnly == false) {
			if(target == null) {
				walkable = walkable && objs [x, y] == null;
			} else {
				walkable = walkable && (objs [x, y] == null || objs [x, y] == target);
			}
		}

		return  walkable;
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

//	public void placeBuilding (GameObj b, int x, int y) {
//		int baseHeight = b.mapCollider.GetLength (0);
//		int baseWidth = b.mapCollider.GetLength (1);
//
//		int startY = y - (baseHeight / 2);
//		int startX = x - (baseWidth / 2);
//
//		for (int j = 0; j < baseHeight; j++) {
//			for(int i = 0; i < baseWidth; i++) {
//				if(b.mapCollider[j, i]) {
//					objs[startX + i, startY + j] = b;
//				}
//			}
//		}
//	}

	public override string ToString() {
		string ret = "tiles:\n";

		for(int j = height-1; j >= 0; j--) {
			for (int i = 0; i < width; i++) {
				ret += tiles[i,j] + " ";
			}
			ret += "\n";
		}

		ret += "objs:\n";
		
		for(int j = height-1; j >= 0; j--) {
			for (int i = 0; i < width; i++) {
				string str = (objs[i,j] != null) ? "1 " : "0 ";
				ret += str;
			}
			ret += "\n";
		}

		return ret;
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
