using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public Transform player;

	// Use this for initialization
	void Start () {
		MapBuilder mb = GameObject.Find ("TileMap").GetComponent<MapBuilder> ();

		Instantiate (player, mb.getHeroeStartPos (), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
