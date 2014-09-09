using UnityEngine;
using System.Collections;

public class Enemy1 : MonoBehaviour {

	public GameObject healthBarPrefab;
	public Vector3 healthBarSize = new Vector3 (1, 1, 1);
	public Vector3 healthBarOffset = new Vector3(-0.5f, 1, 0);
	public float health = 100;
	public float maxHealth = 100;

	private Transform healthBar;

	// Use this for initialization
	void Start () {
		GameObject clone = Instantiate(healthBarPrefab, (transform.position + healthBarOffset), Quaternion.identity) as GameObject;
		healthBar = clone.transform;
		healthBar.localScale = healthBarSize;
	}
	
	// Update is called once per frame
	void Update () {
		// Make the health bar follow the enemy
		healthBar.position = transform.position + healthBarOffset;

		// Make the health bar scale to the current health
		healthBar.localScale = new Vector3((health/maxHealth) * healthBarSize.x, healthBarSize.y, healthBarSize.z);
	}
}
