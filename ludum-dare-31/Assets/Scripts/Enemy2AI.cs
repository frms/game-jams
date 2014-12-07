using UnityEngine;
using System.Collections;

public class Enemy2AI : MonoBehaviour {
	public GameObject meleeObj;
	public float meleeArc = 180;
	public float meleeTime = 1;
	private float nextMelee = 0.0F;
	private bool attacking = false;

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

		if(acceleration != Vector2.zero) {
			steeringUtils.steer (acceleration);
			attacking = false;
		} else {
			attacking = true;
		}

		updateMeleeAttack();
		
		steeringUtils.lookWhereYoureGoing ();
	}

	private void updateMeleeAttack() {
		// If the enemy should be attacking and is not already in a melee attack then start attacking
		if( attacking && Time.time > nextMelee) {
			nextMelee = Time.time + meleeTime;
		}
		
		// If we are still melee attacking then animate it
		if (nextMelee - Time.time >= 0) {
			float angle = meleeArc * (1 - (nextMelee - Time.time) / meleeTime);
			angle -= meleeArc/2f;

			if(float.IsNaN(angle)) {
				Debug.Log ("YOOOOOOOOOOOO");
			}

			Vector2 position = new Vector2 (Mathf.Cos (angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad));
			meleeObj.transform.localPosition = position*0.6f;
			meleeObj.transform.localRotation = Quaternion.Euler(0, 0, angle);
			
			meleeObj.SetActive(true);
		}
		// Else hide the attack image
		else {
			meleeObj.SetActive(false);
		}
	}
}
