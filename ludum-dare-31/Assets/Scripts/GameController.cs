using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public Button restartBtn;

	public Enemies[] enemies;

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");

		foreach(Enemies e in enemies) {
			e.init();

			for(int i = 0; i < e.startNum; i++) {
				e.add(createGameObj(e.go));
			}
		}
	}
	

	private GameObject createGameObj(GameObject go) {
		float x = Random.value * 16 - 8;
		float y = Random.value * 9 - 4.5f;
		
		Quaternion orientation = Quaternion.Euler(0, 0, Random.value * 360);
		
		return Instantiate (go, new Vector3(x, y, 0), orientation) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(player == null) {
			restartBtn.gameObject.SetActive(true);
		} else {
			foreach(Enemies e in enemies) {
				if(Time.time >= e.nextSpawnTime) {
					e.add(createGameObj(e.go));
					e.calcSpawn();
				}
			}
		}
	}

	public void restartGame() {
		Application.LoadLevel(0);
	}
}

[System.Serializable]
public class Enemies
{
	public GameObject go;
	public int startNum;
	public int maxNum;
	public Vector2 spawnTimer;
	public float lastSpawnTime;
	public float nextSpawnTime;

	public List<GameObject> mob;

	public void init() {
		mob = new List<GameObject>();
		calcSpawn();
	}

	public void calcSpawn() {
		lastSpawnTime = Time.time;
		nextSpawnTime = Time.time + Random.Range(spawnTimer.x, spawnTimer.y);
	}

	public bool add(GameObject go) {
		// Remove any deleted game objects
		mob.RemoveAll(x => x == null);

		if(mob.Count <= maxNum) {
			mob.Add(go);
		}

		return (mob.Count <= maxNum);
	}
}
