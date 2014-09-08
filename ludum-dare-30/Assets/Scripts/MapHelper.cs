using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MapHelper : MonoBehaviour {

	public GameObject block;
	public GameObject boundaryBlock;
	public GameObject pointBlock;

	public Sprite blockSprite;
	public Sprite pointBlockSprite;

	public float mapWidth = 20;
	public float mapHeight = 20;
	public int numberOfBlocks = 15;
	public int numberOfPointBlocks = 50;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		drawGrid();
	}

	void drawGrid() {
		int halfWidth = (int) (mapWidth/2);
		int halfHeight = (int) (mapHeight/2);

		for(int i = 0; i <= mapWidth; i++) {
			Vector2 start = new Vector2(i - halfWidth, -halfHeight);
			Vector2 end = new Vector2(i - halfWidth, halfHeight);
			Debug.DrawLine(start, end);
		}


		for(int i = 0; i <= mapHeight; i++) {
			Vector2 start = new Vector2(halfWidth, i - halfHeight);
			Vector2 end = new Vector2(-halfWidth, i - halfHeight);
			Debug.DrawLine(start, end);
		}
	}

	public void randomizeBlocks() {
		createBlocks("Blocks", block, numberOfBlocks);
		createBlocks("PointBlocks", pointBlock, numberOfPointBlocks);

		createBoundary();
	}

	private void createBlocks(string name, GameObject prefab, int num) {
		// Get the blocks parent or create the game object if it doesn't exist
		Transform blocksParent = getOrganizationChild (name);
		
		// Destory all current blocks game objects
		while(blocksParent.childCount > 0) {
			Transform child = blocksParent.GetChild(0);
			DestroyImmediate(child.gameObject);
		}
		
		// Try to make the blocks
		for (int i = 0; i < num; i++) {
			tryToCreateBlock(blocksParent, prefab);
		}
	}

	private void createBoundary() {
		Transform boundaryParent = getOrganizationChild ("Boundary");

		// Destory all current boundary blocks game objects
		while(boundaryParent.childCount > 0) {
			Transform child = boundaryParent.GetChild(0);
			DestroyImmediate(child.gameObject);
		}

		int halfWidth = (int) (mapWidth/2);
		int halfHeight = (int) (mapHeight/2);

		// Bottom boundary
		float x = -halfWidth - 1f + 0.5f;
		float y = -halfHeight - 1f + 0.5f;
		for(int i = 0; i < (mapWidth+2); i++) {
			Vector2 position = new Vector2(x + i, y);

			GameObject clone = Instantiate (boundaryBlock, new Vector3 (position.x, position.y, 0), Quaternion.identity) as GameObject;
			clone.transform.parent = boundaryParent;
		}

		// Top boundary
		x = -halfWidth - 1f + 0.5f;
		y = halfHeight + 1f - 0.5f;
		for(int i = 0; i < (mapWidth+2); i++) {
			Vector2 position = new Vector2(x + i, y);
			
			GameObject clone = Instantiate (boundaryBlock, new Vector3 (position.x, position.y, 0), Quaternion.identity) as GameObject;
			clone.transform.parent = boundaryParent;
		}

		// Left boundary
		x = -halfWidth - 1f + 0.5f;
		y = halfHeight - 1f + 0.5f;
		for(int i = 0; i < mapHeight; i++) {
			Vector2 position = new Vector2(x, y - i);
			
			GameObject clone = Instantiate (boundaryBlock, new Vector3 (position.x, position.y, 0), Quaternion.identity) as GameObject;
			clone.transform.parent = boundaryParent;
		}

		// Right boundary
		x = halfWidth + 0.5f;
		y = halfHeight - 1f + 0.5f;
		for(int i = 0; i < mapHeight; i++) {
			Vector2 position = new Vector2(x, y - i);
			
			GameObject clone = Instantiate (boundaryBlock, new Vector3 (position.x, position.y, 0), Quaternion.identity) as GameObject;
			clone.transform.parent = boundaryParent;
		}
	}

	private Transform getOrganizationChild(string name) {
		Transform child = transform.Find (name);
		
		if (child == null) {
			GameObject go = new GameObject(name);
			child = go.transform;
			
			child.parent = this.transform;
			child.localPosition = Vector3.zero;
			child.localRotation = Quaternion.identity;
			child.localScale = Vector3.one;
		}

		return child;
	}

	private void tryToCreateBlock(Transform parent, GameObject prefab) {
		for (int i = 0; i < 10; i++) {
			Vector2 position = new Vector2 ();
			position.x = ( (int) (mapWidth * Random.value) ) - (mapWidth / 2) + 0.5f;
			position.y = ( (int) (mapHeight * Random.value) ) - (mapHeight / 2) + 0.5f;
			
			//Debug.Log (position);
			
			// If the position is on the map then make the game object and end the loop
			if (isPositionAvailable(position)) {
				//Debug.Log ("On the map!");
				
				//Quaternion rotation = Quaternion.Euler(0, 0, Random.value*360);
				
				GameObject clone = Instantiate (prefab, new Vector3 (position.x, position.y, 0), Quaternion.identity) as GameObject;
				clone.transform.parent = parent;
				
				break;
			}
		}
	}

	private bool isPositionAvailable(Vector2 position) {
		Transform parent = getOrganizationChild("Blocks");

		foreach(Transform child in parent) {
			if(child.position.x == position.x && child.position.y == position.y) {
				return false;
			}
		}

		parent = getOrganizationChild("PointBlocks");
		
		foreach(Transform child in parent) {
			if(child.position.x == position.x && child.position.y == position.y) {
				return false;
			}
		}

		return true;
	}

	private bool showPointBlockLook = false;

	public void togglePointBlockLook() {
		showPointBlockLook = !showPointBlockLook;

		Transform parent = getOrganizationChild("PointBlocks");
		
		foreach(Transform child in parent) {
			SpriteRenderer sp = child.gameObject.GetComponent<SpriteRenderer>();
			if(showPointBlockLook) {
				sp.sprite = pointBlockSprite;
			} else {
				sp.sprite = blockSprite;
			}
		}

	}
}
