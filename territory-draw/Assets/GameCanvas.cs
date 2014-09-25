using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameCanvas : MonoBehaviour {
	public int x = 0;
	public int y = 0;
	
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
		clickPixel();
	}
	
	private void clickPixel ()
	{
		if (!Input.GetButtonDown ("Fire1"))
			return;
		
//		RaycastHit hit;
//		if (!Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit))
//			return;
//		
//		Renderer renderer = hit.collider.renderer;
//		MeshCollider meshCollider = hit.collider as MeshCollider;
//		if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
//			return;
//		
//		Vector2 pixelUV = hit.textureCoord;
//		int x = (int)(pixelUV.x * tex.width);
//		int y = (int)(pixelUV.y * tex.height);
//		Debug.Log (x + "--" + y);

		DateTime startTime = DateTime.Now;

		Color[] colors = tex.GetPixels ();
		bool[] visited = new bool[colors.Length];

		for(int i = 0; i < colors.Length; i++) {
			if(colors[i] != Color.red && !visited[i]) {
				List<int> fillList = fill (colors, visited, i);
				if(fillList != null) {
					foreach(int index in fillList) {
						setPixelByIndex(index, Color.green);
					}
				}
			}
		}

		TimeSpan diff = DateTime.Now - startTime;
		Debug.Log(diff.Milliseconds);
		Debug.Log(Time.deltaTime * 1000);
	}
	
	// This fill takes around 62 milliseconds and a frame lasts only 16 millisecond 
	private List<int> fill(Color[] colors, bool[] visited, int start) {
		List<int> fillList = new List<int>();
		bool returnFillList = true;
		
		Queue<int> nodes = new Queue<int> (colors.Length);

		tryToEnqueue (colors, visited, nodes, start);
		
		while(nodes.Count > 0) {
			int currentNode = nodes.Dequeue();
			
			// If this node has already been visited then skip it
			if(visited[currentNode]) {
				continue;
			}
			
			visited[currentNode] = true;
			fillList.Add(currentNode);
			
			// Add the left child
			returnFillList &= tryToEnqueue(colors, visited, nodes, currentNode - 1);
			// Add the right child
			returnFillList &= tryToEnqueue(colors, visited, nodes, currentNode + 1);
			// Add the top child
			returnFillList &= tryToEnqueue(colors, visited, nodes, currentNode + tex.width);
			// Add the bottom child
			returnFillList &= tryToEnqueue(colors, visited, nodes, currentNode - tex.width);
		}

		if(returnFillList) {
			return fillList;
		} else {
			return null;
		}
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
	private bool tryToEnqueue(Color[] colors, bool[] visited, Queue<int> nodes, int node) {
		if(node >= 0 && node < visited.Length) {
			if(colors[node] != Color.red && !visited[node]) {
				nodes.Enqueue(node);
			}
			return true;
		} else {
			return false;
		}
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