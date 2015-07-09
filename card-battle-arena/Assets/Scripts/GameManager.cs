using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Left Click
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("Pressed left click, casting ray.");
			CastRay();
		}       
	}
	
	void CastRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Debug.Log (ray.origin);
		Debug.Log (ray.direction);

		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
		if (hit) {
			Debug.Log (hit.collider.gameObject.name);
		}
	}  
}
