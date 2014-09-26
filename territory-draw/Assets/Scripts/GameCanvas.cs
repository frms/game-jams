using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameCanvas : MonoBehaviour {
	public bool canvasFill = true;
	
	private Vector2 originWorldCoords;
	private Texture2D tex;
	
	void Start(){
		// Clone the current texture so we don't modify the original one. I don't know why unity acts like this.
		tex = Instantiate(renderer.material.mainTexture) as Texture2D;
		renderer.material.mainTexture = tex;
		
		// Calculate the world coordinates of the texture's 0,0 position
		float x = transform.position.x - (transform.localScale.x / 2);
		float y = transform.position.y - (transform.localScale.y / 2);
		originWorldCoords = new Vector3 (x, y);
	}
	
	// Update is called once per frame
	void Update () {
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
	
}