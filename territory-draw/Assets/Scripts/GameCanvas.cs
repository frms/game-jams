using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameCanvas : MonoBehaviour {
	public bool canvasFill = true;
	public GameObject enemy;
	public int numberOfEnemies = 2;
	public float timer = 60;
	
	private Vector2 originWorldCoords;
	private Texture2D tex;
	private GameObject player;
	private GameObject[] enemies;

	void Start(){
		// Clone the current texture so we don't modify the original one. I don't know why unity acts like this.
		tex = Instantiate(renderer.material.mainTexture) as Texture2D;
		renderer.material.mainTexture = tex;
		
		// Calculate the world coordinates of the texture's 0,0 position
		float x = transform.position.x - (transform.localScale.x / 2);
		float y = transform.position.y - (transform.localScale.y / 2);
		originWorldCoords = new Vector3 (x, y);

		player = GameObject.Find ("Player");

		createEnemies ();
	}

	private void createEnemies() {
		// Create the parent game object to hold the enemies
		enemies = new GameObject[numberOfEnemies];

		for(int i = 0; i < numberOfEnemies; i++) {
			float x = (UnityEngine.Random.value * transform.localScale.x) - transform.localScale.x / 2;
			float y = (UnityEngine.Random.value * transform.localScale.y) - transform.localScale.y / 2;

			Quaternion rotation = Quaternion.Euler(0, 0, UnityEngine.Random.value*360);
			GameObject clone = Instantiate (enemy, new Vector3 (x, y, -1), rotation) as GameObject;
			enemies[i] = clone;
		}
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		if(timer <= 0.1) {
			endGame();
		}

		if(Input.GetButtonDown("Fire1")) {
			canvasFill = !canvasFill;
		}

		fillInColors();
	}

	Color[] fillColors = new [] { Color.blue, Color.red };
	int fillInColorsIndex = -1;
	DateTime startTime;

	private void fillInColors ()
	{
		if (!canvasFill)
			return;

		startTime = DateTime.Now;

		// Start filling for the first color
		if(fillInColorsIndex == -1) {
			fillInColorsIndex = 0;
		}

		for(; fillInColorsIndex < fillColors.Length; fillInColorsIndex++) {
			if(!fillGivenColor (fillColors[fillInColorsIndex])) {
//				TimeSpan diff1 = DateTime.Now - startTime;
//				Debug.Log(diff1.Milliseconds);
//				Debug.Log(Time.deltaTime * 1000);
				return;
			}
		}

		// Reset the index if we've done all our work
		if(fillInColorsIndex >= fillColors.Length) {
			fillInColorsIndex = -1;
		}

//		TimeSpan diff2 = DateTime.Now - startTime;
//		Debug.Log(diff2.Milliseconds);
//		Debug.Log(Time.deltaTime * 1000);
	}

	int fillGivenColorIndex = -1;
	Color[] colors;
	bool[] visited;

	List<int> fillList;
	bool useFillList;
	Queue<int> nodes;

	private bool fillGivenColor(Color fillColor) {
		if(fillGivenColorIndex == -1) {
			fillGivenColorIndex = 0;
			colors = tex.GetPixels ();
			visited = new bool[colors.Length];
		}
		
		for(; fillGivenColorIndex < colors.Length; fillGivenColorIndex++) {
			// If work is still in progress for the given pixel or its a new pixel and its a valid one to fill
			// then call fill on that pixel.
			if( nodes != null || (colors[fillGivenColorIndex] != fillColor && !visited[fillGivenColorIndex]) ) {
				bool isComplete = fill (colors, visited, fillGivenColorIndex, fillColor);
				
				if(isComplete) {
					if(useFillList) {
						foreach(int index in fillList) {
							setPixelByIndex(index, fillColor);
						}
						tex.Apply ();
					}

					// Clear the nodes list so the fill state is reset the next time it runs
					nodes = null;
				}
				// The last fill didnt finish so we've run out of time for this frame
				else {
					return false;
				}
			}
		}

		// Reset the index if we've done all our work
		if (fillGivenColorIndex >= colors.Length) {
			fillGivenColorIndex = -1;
		}

		return true;
	}

	// This fill takes around 62 milliseconds and a frame lasts only 16 millisecond 
	private bool fill(Color[] colors, bool[] visited, int start, Color fillColor) {
		if(nodes == null) {
			fillList = new List<int>();
			useFillList = true;
			nodes = new Queue<int> (colors.Length);

			tryToEnqueue (colors, visited, nodes, start, fillColor);
		}
		
		while(nodes.Count > 0) {
			if(shouldPauseWork()) {
				return false;
			}

			int currentNode = nodes.Dequeue();
			
			// If this node has already been visited then skip it
			if(visited[currentNode]) {
				continue;
			}
			
			visited[currentNode] = true;
			fillList.Add(currentNode);
			
			// Add the left child
			useFillList &= tryToEnqueue(colors, visited, nodes, currentNode - 1, fillColor);
			// Add the right child
			useFillList &= tryToEnqueue(colors, visited, nodes, currentNode + 1, fillColor);
			// Add the top child
			useFillList &= tryToEnqueue(colors, visited, nodes, currentNode + tex.width, fillColor);
			// Add the bottom child
			useFillList &= tryToEnqueue(colors, visited, nodes, currentNode - tex.width, fillColor);
		}

		return true;
	}
	
	private void setPixelByIndex(int currentNode, Color color) {
		int x = currentNode % tex.width;
		int y = currentNode / tex.width;
		
		tex.SetPixel (x, y, color);
	}
	/**
	 * Tries to enqueue the given pixel if it makes sense.
	 * 
	 * Returns a bool if we can still fill the pixels that are reached
	 */
	private bool tryToEnqueue(Color[] colors, bool[] visited, Queue<int> nodes, int node, Color fillColor) {
		if(node >= 0 && node < visited.Length) {
			if(colors[node] != fillColor && !visited[node]) {
				nodes.Enqueue(node);
			}
			return true;
		} else {
			return false;
		}
	}

	private bool shouldPauseWork() {
		TimeSpan diff = DateTime.Now - startTime;
		return diff.Milliseconds > 3;
	}
	
	public void drawColor(Vector2 worldPosition, Color color) {
		//Debug.Log (worldPosition);
		
		Vector2 offsetCoords = worldPosition - originWorldCoords;
		int x = (int) ( (offsetCoords.x / transform.localScale.x) * tex.width );
		int y = (int) ( (offsetCoords.y / transform.localScale.y) * tex.height );
		
		//Debug.Log (texCoords);
		
		// Draw a square at the current location
		Color[] colors = new Color[4];
		for(int i = 0; i < 4; i++) {
			colors[i] = color;
		}
		tex.SetPixels (x-1, y-1, 2, 2, colors);
		
		tex.Apply ();
	}

	private int playerColorCount;
	private int opponentColorCount;
	private bool gameOver = false;

	private void endGame() {
		playerColorCount = 0;
		opponentColorCount = 0;

		Color[] colors = tex.GetPixels ();

		for(int i = 0; i < colors.Length; i++) {
			if(colors[i] == fillColors[0]) {
				playerColorCount++;
			} else if(colors[i] == fillColors[1]) {
				opponentColorCount++;
			}
		}

		player.SendMessage ("timeIsUp");

		for(var i = 0 ; i < enemies.Length; i++) {
			enemies[i].SendMessage("timeIsUp");
		}

		gameOver = true;
	}


	float native_width = 1920;
	float native_height = 1080;
	
	void OnGUI ()
	{
		Matrix4x4 saveMat = GUI.matrix; // save current matrix
		
		//set up scaling
		float rx = Screen.width / native_width;
		float ry = Screen.height / native_height;
		GUI.matrix = Matrix4x4.TRS (new Vector3(0, 0, 0), Quaternion.identity, new Vector3 (rx, ry, 1)); 
		
		// Do normal gui code from here on as though the resolution is guarenteed to be the native resolution
		if(!gameOver) {
			drawTime ();
		} else {
			drawGameOver();
		}
		
		// Finish doing gui code
		
		GUI.matrix = saveMat; // restore matrix
	}

	private void drawTime() {
		GUIStyle style = new GUIStyle ();
		style.fontSize = 60;
		
		string text = ((int) (timer)).ToString();
		Vector2 size = style.CalcSize(new GUIContent(text));
		float x = native_width / 2 - size.x / 2;
		
		GUI.Label(new Rect(x, 10, size.x, size.y), text, style);
	}

	private void drawGameOver() {
		GUIStyle style = new GUIStyle ();
		style.fontSize = 120;
		
		string text1 = "Game Over";
		Vector2 size1 = style.CalcSize(new GUIContent(text1));

		float x = native_width / 2 - size1.x / 2;
		float y = native_height / 2 - size1.y;
		
		GUI.Label(new Rect(x, y, size1.x, size1.y), text1, style);

		string text2 = playerColorCount+" "+opponentColorCount;
		Vector2 size2 = style.CalcSize(new GUIContent(text2));
		
		x = native_width / 2 - size2.x / 2;
		y = native_height / 2;
		
		GUI.Label(new Rect(x, y, size2.x, size2.y), text2, style);
	}
}