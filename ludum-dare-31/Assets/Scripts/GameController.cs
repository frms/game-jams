using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public Button restartBtn;

	public GameObject enemy1;
	public int numOfEnemy = 5;

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");

		for(int i = 0; i < numOfEnemy; i++) {
			float x = Random.value * 16 - 8;
			float y = Random.value * 9 - 4.5f;

			Quaternion orientation = Quaternion.Euler(0, 0, Random.value * 360);

			Instantiate (enemy1, new Vector3(x, y, 0), orientation);
		}
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
