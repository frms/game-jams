using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public Transform capsule;
	public int numCapsules = 10;

	// Use this for initialization
	void Start () {
		float groundDist = Camera.main.transform.position.y;

		Vector3 bottomLeft = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, groundDist));
		Vector3 topRight = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, groundDist));

		//Debug.Log (bottomLeft + " " + topRight);

		for(int i = 0; i < numCapsules; i++) {
			Vector3 pos = new Vector3();
			pos.x = Random.Range(bottomLeft.x, topRight.x);
			pos.y = capsule.position.y;
			pos.z = Random.Range(bottomLeft.z, topRight.z);

			Instantiate(capsule, pos, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
