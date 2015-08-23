using UnityEngine;
using System.Collections;

public class Enemy2 : Enemy1 {

	// Use this for initialization
	public override void Start () {
		base.Start();

		GameObject player = GameObject.Find("Player");
		target = player.GetComponent<Mover>();
	}
	
//	// Update is called once per frame
//	void Update () {
//	
//	}
}
