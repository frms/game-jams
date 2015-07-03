using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public Transform player;

	// Use this for initialization
	void Start () {
		TileMap tm = GameObject.Find ("TileMap").GetComponent<TileMap> ();

		Vector2 pos = tm.getHeroeStartPos ();
		Instantiate (player, pos, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
