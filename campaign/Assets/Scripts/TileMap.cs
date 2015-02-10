using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TileMap : MonoBehaviour {
	public int tileResolution = 64;
	public int mapWidth = 3;
	public int mapHeight = 2;
	public float tileSize = 1.0f;
	public float halfMapDepth = 0.125f;

	public float perlinScale = 4f;
	public float landCutoff = 0.35f;

	public bool cityDebug = false;
	public float cityPerlinScale = 3f;
	public float cityLandCutoff = 0.1f;

	public int numberOfCities = 20;
	[HideInInspector]
	public int[] roomWidthRange = new [] {4, 8};
	[HideInInspector]
	public int[] roomHeightRange = new [] {4, 8};

	private MapData map;
	private List<Room> cities;

	// Use this for initialization
	void Start () {
		buildMap ();
	}

	public void buildMap() {
		buildMapData ();
		buildMesh ();
	}

	public void buildMesh() {
		int numTiles = mapWidth * mapHeight;
		int numTris = numTiles * 2;

		int numVertsX = 2 * (mapWidth - 1) + 2;
		int numVertsZ = 2 * (mapHeight - 1) + 2;
		int numVerts = numVertsX * numVertsZ;

		float textureStep = (float)tileResolution / renderer.sharedMaterial.mainTexture.width;

		Mesh mesh = new Mesh ();

		Vector3[] vertices = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];

		for (int z = 0; z < numVertsZ; z++) {
			for(int x = 0; x < numVertsX; x++) {
				int i = z * numVertsX + x;

				Vector3 vert = new Vector3((x / 2) * tileSize, 0, (z / 2) * tileSize);
				
				// Set vertex depth
				if(z == 0 || z % 2 == 1) {
					if(x == numVertsX-1 || x % 2 == 0) {
						vert.y = Random.Range(-halfMapDepth, halfMapDepth);
						
						if(x != 0 && x != numVertsX-1 ) {
							vertices[i-1].y = vert.y;
						}
					}
				} else {
					vert.y = vertices[i-numVertsX].y;
				}

				Vector2 texCoord;

				if(x % 2 == 0 && z % 2 == 0) {
					int type = map[x/2, z/2];
					texCoord = new Vector2(type * textureStep, 0);
				} else {
					int tileX = (x/2)*2;
					int tileZ = (z/2)*2;
					int tileIndex = tileZ * numVertsX + tileX;
					texCoord = uv[tileIndex];
				}

				if(x % 2 == 1) {
					vert.x += tileSize;
					texCoord.x += textureStep;
				}

				if(z % 2 == 1) {
					vert.z += tileSize;
					texCoord.y = 1;
				}

				vertices[i] = vert;
				uv[i] = texCoord;
			}
		}

		int[] triangles = new int[numTris * 3];

		for (int z = 0; z < mapHeight; z++) {
			for (int x = 0; x < mapWidth; x++) {
				int i = (z * mapWidth + x) * 2 * 3;
				int j = z*2 * numVertsX + x*2;

				triangles [i] = j;
				triangles [i + 1] = j + numVertsX;
				triangles [i + 2] = j + 1;

				triangles [i + 3] = j + numVertsX;
				triangles [i + 4] = j + numVertsX + 1;
				triangles [i + 5] = j + 1;
			}
		}

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;

		mesh.RecalculateNormals ();

		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		meshFilter.mesh = mesh;

		MeshCollider meshCollider = GetComponent<MeshCollider> ();
		meshCollider.sharedMesh = mesh;
	}

	private void buildMapData() {
		map = new MapData (mapWidth, mapHeight);

		cities = new List<Room>();

		createLandSea ();

		for (int i = 0; i < numberOfCities; i++) {
			int roomWidth = Random.Range (roomWidthRange [0], roomWidthRange [1]);
			int roomHeight = Random.Range (roomHeightRange [0], roomHeightRange [1]);
			/* Add 1 because Random.Range() for ints excludes the max value */
			int roomX = Random.Range (0, map.width - roomWidth + 1);
			int roomY = Random.Range (0, map.height - roomHeight + 1);
			
			Room c = new Room (roomX, roomY, roomWidth, roomHeight);

			if(!cityCollides(c)) {
				cities.Add (c);
				createCity (c);
			}
		}
	}

	private void createLandSea() {
		PerlinHelper ph = new PerlinHelper (mapWidth, mapHeight, perlinScale);
		
		for(int y = 0; y < mapHeight; y++) {
			for(int x = 0; x < mapWidth; x++) {
				if(ph[x, y] >= landCutoff) {
					map[x,y] = 1;
				} else {
					map[x,y] = 0;
				}
			}
		}
	}

	private void createCity(Room c) {
		PerlinHelper ph = new PerlinHelper (c.width, c.height, cityPerlinScale);
		
		for (int x = 0; x < c.width; x++) {
			for (int y = 0; y < c.height; y++) {
				int tile;
				Vector2 centerDist = new Vector2(Mathf.Abs(x - c.width/2), Mathf.Abs(y - c.height/2));
				
				if(centerDist.magnitude <= 2) {
					tile = 3;
				} else {
					float result = ph[x, y] - (centerDist.magnitude/c.radius);
					
					if(result >= cityLandCutoff) {
						tile = 1;
					} else {
						tile = 2;
					}
				}

				if(tile != 2 || cityDebug) { 
					map[c.x + x, c.y + y] = tile;
				}
			}
		}
	}

	public bool cityCollides(Room c) {
		foreach (Room c2 in cities) {
			if(c.innerRoomCollidesWith(c2)) {
				return true;
			}
		}
		
		return false;
	}
}
