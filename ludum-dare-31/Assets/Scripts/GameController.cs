using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public Button restartBtn;

	public Enemies[] enemies;

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");

		foreach(Enemies e in enemies) {
			createGameObjs(e.go, e.startNum);
		}
	}

	private void createGameObjs(GameObject go, int num) {
		for(int i = 0; i < num; i++) {
			createGameObj(go);
		}
	}

	private void createGameObj(GameObject go) {
		float x = Random.value * 16 - 8;
		float y = Random.value * 9 - 4.5f;
		
		Quaternion orientation = Quaternion.Euler(0, 0, Random.value * 360);
		
		Instantiate (go, new Vector3(x, y, 0), orientation);
	}
	
	// Update is called once per frame
	void Update () {
		if(player == null) {
			restartBtn.gameObject.SetActive(true);
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
}
