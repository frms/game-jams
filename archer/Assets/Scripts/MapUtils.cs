using UnityEngine;
using System.Collections;

[ExecuteInEditMode] 
public class MapUtils : MonoBehaviour {
	public GameObject enemy;
	public float mapWidth = 40.96f;
	public float mapHeight = 40.96f;
	public int numberOfEnemies = 15;

	public void buildObject() {
		// Get the enemies parent or create the game object if it doesn't exist
		Transform enemiesParent = transform.Find ("Enemies");

		if (enemiesParent == null) {
			GameObject enemies = new GameObject("Enemies");
			enemiesParent = enemies.transform;

			enemiesParent.parent = this.transform;
			enemiesParent.localPosition = Vector3.zero;
			enemiesParent.localRotation = Quaternion.identity;
			enemiesParent.localScale = Vector3.one;
		}

		// Destory all current enemy game objects
		while(enemiesParent.childCount > 0) {
			Transform child = enemiesParent.GetChild(0);
			DestroyImmediate(child.gameObject);
		}

		// Try to make the enemies
		for (int i = 0; i < numberOfEnemies; i++) {
			tryToCreateEnemy(enemiesParent);
		}

	}

	/* Trys to create an enemy on the map. It keeps tryign to find a good random spot
	 * to put the enemy before it finally makes the enemy or gives up. */
	private void tryToCreateEnemy(Transform enemiesParent) {

		for (int i = 0; i < 10; i++) {
			Vector2 position = new Vector2 ();
			position.x = (mapWidth * Random.value) - (mapWidth / 2);
			position.y = (mapHeight * Random.value) - (mapHeight / 2);

			//Debug.Log (position);

			// If the position is on the map then make the game object and end the loop
			if (isPointOnMapGround (position)) {
				//Debug.Log ("On the map!");

				Quaternion rotation = Quaternion.Euler(0, 0, Random.value*360);

				GameObject clone = Instantiate (enemy, new Vector3 (position.x, position.y, -1), rotation) as GameObject;
				clone.transform.parent = enemiesParent;

				break;
			}
		}
	}

	/* Determines if the point is on the map. It does this by shooting a ray from
	 * the point and counting how many times the ray collides with the edges of the
	 * map. If its an odd number then the point is on the map. */
	private bool isPointOnMapGround(Vector2 position) {
		int mask = (1 << LayerMask.NameToLayer("GroundFence"));

		int hits = raycastHits (position, Vector2.right, Mathf.Infinity, mask);

		//Debug.Log (hits);

		return (hits % 2 == 1);
	}

	/* A helper function that repeatably calls ray cast to find the number of hits. I
	 * cannot use the built in raycastAll because they only give one hit per collider
	 * even if the ray hits the collider multiple times */
	private int raycastHits(Vector2 origin, Vector2 direction, float distance, int layerMask) {

		RaycastHit2D hit;
		int loopCount = 0;
		int hits = 0;
		
		do {
			hit = Physics2D.Raycast(origin, direction, distance, layerMask);
			
			// If we have a hit increase the hit count and move the ray origin a little past hit point
			if(hit.collider != null) {
				hits++;
				
				//Debug.Log (hit.point);
				
				origin = hit.point;
				origin.x += 0.05f;
			}
			
			if(loopCount > 1000) {
				Debug.Log ("Finding raycast hits went too long!");
				break;
			}
			loopCount++;
		} while(hit.collider != null);

		return hits;
	}

	public void stopWanderingEnemies() {
		Transform enemiesParent = transform.Find ("Enemies");
		
		if (enemiesParent != null) {
			foreach (Transform child in enemiesParent)
			{
				// Disable the wander script and remove any movement currently on the enemy
				Wander wander = child.GetComponent<Wander>();
				wander.enabled = false;
				child.rigidbody2D.velocity = Vector2.zero;
				child.rigidbody2D.angularVelocity = 0;
			}
		}
	}

	public void startWanderingEnemies() {
		Transform enemiesParent = transform.Find ("Enemies");
		
		if (enemiesParent != null) {
			foreach (Transform child in enemiesParent)
			{
				// Enable the wander script
				Wander wander = child.GetComponent<Wander>();
				wander.enabled = true;
			}
		}
	}
}
