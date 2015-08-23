using UnityEngine;
using System.Collections;

public class Enemy2 : Enemy1 {
	public float seekTargetDist = 10f;

	private Mover player;
	private HealthBar playerHealth;

	// Use this for initialization
	public override void Start () {
		base.Start();

		GameObject playerGO = GameObject.Find("Player");
		player = playerGO.GetComponent<Mover>();
		playerHealth = player.GetComponent<HealthBar>();
	}
	
	// Update is called once per frame
	public override void Update () {
		if(player != null && Vector2.Distance(player.transform.position, transform.position) < seekTargetDist) {
			target = player;
			enemyHealth = playerHealth;
		} else {
			target = null;
			enemyHealth = null;
		}

		moveUnit ();

		tryToAttack();

		float v = GetComponent<Rigidbody>().velocity.magnitude;
		if(v > 0) {
			Debug.Log(name + " " + v);
		}
	}
}
