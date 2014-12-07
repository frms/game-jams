using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject enemy1;
	public int numOfEnemy = 5;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < numOfEnemy; i++) {
			float x = Random.value * 16 - 8;
			float y = Random.value * 9 - 4.5f;

			Quaternion orientation = Quaternion.Euler(0, 0, Random.value * 360);

			Instantiate (enemy1, new Vector3(x, y, 0), orientation);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
