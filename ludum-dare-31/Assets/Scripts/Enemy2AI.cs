using UnityEngine;
using System.Collections;

public class Enemy2AI : MonoBehaviour {
	private SteeringUtils steeringUtils;

	private Transform player;

	// Use this for initialization
	void Start () {
		steeringUtils = gameObject.GetComponent<SteeringUtils> ();

		player = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 acceleration = steeringUtils.arrive (player.position);
		
		steeringUtils.steer (acceleration);
		
		steeringUtils.lookWhereYoureGoing ();
	}
}
