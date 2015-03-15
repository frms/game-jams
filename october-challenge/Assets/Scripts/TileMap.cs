using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TileMap : MonoBehaviour {

	public GameObject tile;

	public int mapI = 1024;
	public int mapJ = 1024;

	private Tile[,] map;

	public int originI;
	public int originJ;

	public Texture2D gameOverTex;
	public float chanceForGameOver = 0.05f;
	public bool gameOver = false;

	private Texture2D playerTex;


	// Use this for initialization
	void Start () {
		map = new Tile[mapI, mapJ];

		originI = mapI / 2;
		originJ = mapJ / 2;

		createStartTile (originI, originJ);

		GameObject player = GameObject.Find ("Player");
		playerTex = (Texture2D) player.renderer.material.mainTexture;
	}

	private void createStartTile(int i, int j) {
		float temp = chanceForGameOver;
		chanceForGameOver = 0;

		createTile (i, j);

		chanceForGameOver = temp;
	}

	public bool tryToReachTile(int i, int j) {
		bool canReach = true;

		// If we are trying to reach a tile out of range on the map then return false
		if( i < 0 && i >= mapI && j < 0 && j >= mapJ) {
			canReach = false;
		}
		// If the tile doesn't exist
		else if(getTile(i, j) == null) {
			// And it has a tile it has a neightbor tile that does exist then create this tile
			if(hasNeighborTile(i, j)) {
				createTile(i, j);
			}
			// Else we can't reach it
			else {
				canReach = false;
			}
		}

		return canReach;
	}

	public bool hasNeighborTile(int i, int j) {
		return getTile(i - 1, j - 1) != null || getTile(i, j - 1) != null || getTile(i + 1, j - 1) != null ||
				getTile(i - 1, j) != null || getTile(i + 1, j) != null ||
				getTile(i - 1, j + 1) != null || getTile(i, j + 1) != null || getTile(i + 1, j + 1) != null;
	}

	// A save with to get a tile without index out of range exceptions. Returns null if i or j are out of range
	public Tile getTile(int i, int j) {
		if( i >= 0 && i < mapI && j >= 0 && j < mapJ) {
			return map[i, j];
		} else {
			return null;
		}
	}

	public void createTile(int i, int j) {
		Vector3 position = indicesToWorldCoords (i, j);

		GameObject clone = Instantiate (tile, position, Quaternion.identity) as GameObject;
		clone.transform.parent = transform;
		map [i, j] = clone.GetComponent<Tile> ();

		/* Check to see if the next tile will be the game over tile */
		if(UnityEngine.Random.value <= chanceForGameOver) {
			map[i, j].setUp(gameOverTex);
			gameOver = true;
		} else {
			map[i, j].setUp(new Color (UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value), UnityEngine.Random.value * 0.25f);
		}
	}

	DateTime startTime;
	List<Tile> tileList = new List<Tile>();
	Tile currentTile;

	void Update() {

		// Go through the list of colors waiting to be applied to the player
		while(tileList.Count > 0) {
			startTime = DateTime.Now;

			if(currentTile == null) {
				currentTile = tileList[0];
			}

			// If coloring the player has not finished then stop the loop and continue the work next Update()
			if(!colorPlayer(currentTile)) {
				break;
			}

			// If we reach here we are done coloring the current tile so remove it from
			// the list and clear the current tile.
			tileList.Remove(currentTile);
			currentTile = null;
		}
	}

	/* This fuction adds the tile at the given indices to the list of colors to apply to
	 * the player. It does not immediately color the player.
	 */
	public void colorPlayer(int i, int j) {
		if(map[i, j] != null) {
			tileList.Add(map[i, j]);
		}
	}

	/* Color the player with the given tile's color. This function is interruptable so it will continue
	 * where it left off if it goes too long and is called again.
	 * 
	 * The function returns true if it finished and false if it has not finished.
	 */

	Color[] currentColors;
	int currentColorsIndex;

	private bool colorPlayer(Tile tile) {
		if(currentColors == null) {
			currentColors = playerTex.GetPixels();
			currentColorsIndex = 0;
		}
		
		for(; currentColorsIndex < currentColors.Length; currentColorsIndex++) {
			if(shouldPauseWork()) {
				return false;
			}

			if(currentColors[currentColorsIndex].a != 0 && UnityEngine.Random.value < tile.changeChance) {
				currentColors[currentColorsIndex] = tile.color;
			}
		}
		
		playerTex.SetPixels(currentColors);
		
		playerTex.Apply ();

		currentColors = null;

		return true;
	}


	private bool shouldPauseWork() {
		TimeSpan diff = DateTime.Now - startTime;
		return diff.Milliseconds > 3;
	}

	public Vector3 indicesToWorldCoords(int i, int j) {
		float x = (Grid.tileSize/2) + ( (i - originI) * Grid.tileSize );
		float y = (Grid.tileSize/2) + ( (j - originJ) * Grid.tileSize );

		return new Vector3(x, y, Grid.zCoord);
	}
}
