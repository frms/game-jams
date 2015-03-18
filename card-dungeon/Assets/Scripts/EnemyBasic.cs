using UnityEngine;
using System.Collections;

public class EnemyBasic : MonoBehaviour {

	private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 dest = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			dest.y = 0;
			agent.SetDestination (dest);
		} else if (Input.GetMouseButtonDown (1)) {
			agent.Stop();
		}
	}
}
