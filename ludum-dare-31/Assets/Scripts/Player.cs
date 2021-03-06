﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	public Image healthBar;
	public float health = 100;
	public float maxHealth = 100;
	public float outOfCombatTime = 3;
	public float healthRegenRate = 15;
	private float lastTimeInCombat = 0;

	public float speed = 5;

	public GameObject meleeObj;
	public float meleeArc = 180;
	public float meleeTime = 1;
	private float nextMelee = 0.0F;

	public int numOfElementA = 0;
	public int numOfElementB = 0;
	public int numOfElementC = 0;

	public Ability secondaryAbility = Ability.None;

	public enum Ability {None, Arrow, Dash, Stun};

	public GameObject projectile;
	public Transform spawnPoint;
	public float maxArrowDistance = 10;
	public float arrowSpeed = 10;
	public float arrowDamage = 10;

	public GameObject dashObj;
	public float dashDistance = 10;
	public float dashSpeed = 15;
	public float dashMaxDmgTaken = 25;
	private bool dashing = false;
	private Vector3 dashStartPoint;

	public GameObject stunObj1;
	public GameObject stunObj2;
	public GameObject stunObj3;
	public GameObject stunObj4;

	public AudioClip playerHitAudio;
	public AudioClip elementAudio;
	public AudioClip stunAudio;
	public AudioClip shieldAudio;

	private float mouseAngle;

	// Use this for initialization
	void Start () {
		
	}

	bool foo = false;
	
	// Update is called once per frame
	void Update () {
		// Make the health bar scale to the current health
		healthBar.transform.localScale = new Vector3(health/maxHealth, 1, 1);

		// Kill the player if it loses all its health
		if(health <= 0) {
			Destroy(gameObject);
			return;
		}

		// Health regen
		if(health <= maxHealth && Time.time >= lastTimeInCombat+outOfCombatTime) {
			if(!foo && Time.time >= lastTimeInCombat+outOfCombatTime + 0.2f) {
				AudioSource.PlayClipAtPoint(shieldAudio, transform.position, 1f);
				foo = true;
			}
			heal (healthRegenRate * Time.deltaTime);

		} else {
			foo = false;
		}
		
		if(!dashing) {
			Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			dir.z = 0;
			dir.Normalize();

			if(dir != Vector3.zero) {
				mouseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			}

			updateMoveCharacter();
		}

		updateMeleeAttack ();

		updateStun();

		// Trigger secondary action
		if (Input.GetButtonDown ("Fire2")) {

			switch (secondaryAbility) {
				case Ability.Arrow:
					fireArrow(maxArrowDistance);
					break;
				case Ability.Dash:
					startDashing();
					break;
				case Ability.Stun:
					fireStun();
					break;
			}
		}

		if(dashing && Vector3.Distance(transform.position, dashStartPoint) >= dashDistance) {
			stopDashing();
		}

		//Debug.DrawRay(Vector3.zero, new Vector3(Mathf.Cos(22.5f*Mathf.Deg2Rad), Mathf.Sin(22.5f*Mathf.Deg2Rad), 0)*6);
		//Debug.DrawRay(Vector3.zero, new Vector3(Mathf.Cos(-22.5f*Mathf.Deg2Rad), Mathf.Sin(-22.5f*Mathf.Deg2Rad), 0)*6);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		// If we're dashing and we collide with anything then stop dashing
		if(dashing) {
			stopDashing();
		}
	}

	private void updateMoveCharacter() {
		Vector3 stickDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
		
		//stickDirection = stickDirection.normalized;
		
		if(stickDirection.sqrMagnitude > 0.031) {
			stickDirection.Normalize();

			GetComponent<Rigidbody2D>().velocity = stickDirection * speed;
		} else {
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}

		transform.rotation = Quaternion.Euler(0, 0, mouseAngle);
	}

	private void updateMeleeAttack() {
		// If melee button is hit and not already in a melee attack then start attacking
		if(Input.GetButtonDown("Fire1") && Time.time > nextMelee) {
			nextMelee = Time.time + meleeTime;
		}

		// If we are still melee attacking then animate it
		if (nextMelee - Time.time >= 0) {
			float percent = Mathfx.Hermite(0, 1, (1 - (nextMelee - Time.time) / meleeTime));
			float angle = mouseAngle + meleeArc * percent;
			angle -= meleeArc/2f;
			
			Vector3 position = new Vector3 (Mathf.Cos (angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad), 0);
			meleeObj.transform.position = transform.position + position*0.6f;
			meleeObj.transform.rotation = Quaternion.Euler(0, 0, angle);

			//Debug.Log (angle);
			
			meleeObj.SetActive(true);
		}
		// Else hide the attack image
		else {
			meleeObj.SetActive(false);
		}
	}

	public float stunEnemyDuration = 2;

	private void updateStun() {
		if(Time.time < nextStunTime) {
			float dist = ((nextStunTime - Time.time) / stunTime) * 3;
			dist = 3 - dist;

			//Debug.Log (dist);

			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

			foreach(GameObject go in enemies) {
				if(go == null) {
					continue;
				}

				Vector3 enemyPos = go.transform.position;

				if(Vector3.Distance(transform.position, enemyPos) <= dist) {
					go.SendMessage("stun", stunEnemyDuration);
				}
			}
		}
	}

	private void fireArrow(float distance) {
		GameObject clone = Instantiate (projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;
		
		ArrowProperties props = new ArrowProperties ();
		props.maxDistance = distance;
		props.damage = arrowDamage;
		clone.SendMessage ("setUp", props);

		clone.GetComponent<Rigidbody2D>().velocity = transform.right * arrowSpeed;
	}

	private float stunTime = 0.6818182f;
	private float nextStunTime = 0;


	private void fireStun() {
		if(nextStunTime <= Time.time) {
			nextStunTime = Time.time + stunTime;

			Invoke("showStun1", 0);
			Invoke("showStun2", 0.15f);
			Invoke("showStun3", 0.3f);
			Invoke("showStun4", 0.45f);
			Invoke("hideStun", stunTime);

			AudioSource.PlayClipAtPoint(stunAudio, transform.position, 1f);
		}
	}

	public void showStun1() {
		stunObj1.SetActive(true);
	}


	public void showStun2() {
		stunObj2.SetActive(true);
	}


	public void showStun3() {
		stunObj3.SetActive(true);
	}

	public void showStun4() {
		stunObj4.SetActive(true);
	}

	public void hideStun() {
		stunObj1.SetActive(false);
		stunObj2.SetActive(false);
		stunObj3.SetActive(false);
		stunObj4.SetActive(false);
	}

	private float dashShield;

	private void startDashing() {
		dashing = true;

		dashShield = dashMaxDmgTaken;

		dashStartPoint = transform.position;
		GetComponent<Rigidbody2D>().velocity = transform.right * dashSpeed;

		dashObj.SetActive(true);
	}

	private void stopDashing() {
		dashing = false;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;

		dashObj.SetActive(false);
	}
	
	public void gainElementA() {
		numOfElementA++;
		
		secondaryAbility = Ability.Arrow;

		AudioSource.PlayClipAtPoint(elementAudio, transform.position, 1f);
	}
	
	public void gainElementB() {
		numOfElementB++;
		
		secondaryAbility = Ability.Dash;

		AudioSource.PlayClipAtPoint(elementAudio, transform.position, 1f);
	}
	
	public void gainElementC() {
		numOfElementC++;
		
		secondaryAbility = Ability.Stun;

		AudioSource.PlayClipAtPoint(elementAudio, transform.position, 1f);
	}

	public void applyDamage(float damage) {
		// If dashing take less damage
		if(dashing) {
			damage = Mathf.Min(damage, dashShield);

			dashShield -= damage;
		}

		health -= damage;

		if(health <= 0) {
			health = 0;
		}

		lastTimeInCombat = Time.time;

		// Screen shake
		Hashtable ht = new Hashtable(); ht.Add("x",0.115f); ht.Add("y",0.115f); ht.Add("time", 0.3f);
		iTween.ShakePosition(Camera.main.gameObject, ht);

		AudioSource.PlayClipAtPoint(playerHitAudio, transform.position, 1f);
	}

	public void heal(float amt) {
		health += amt;
		
		if(health >= maxHealth) {
			health = maxHealth;
		}
	}
}
