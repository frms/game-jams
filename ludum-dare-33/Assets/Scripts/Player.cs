﻿using UnityEngine;
using System.Collections;

public class Player : Mover {

	public GameObject gameOverPanel;

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			target = getTarget();

			Vector3 endPos;

			if(target == null) {
				endPos = getMousePosition ();

				findPath(endPos);
			}

			if(target != null && target.tag == "Enemy") {
				enemyHealth = target.GetComponent<HealthBar>();
			} else {
				enemyHealth = null;
			}
		}

		moveUnit ();

		tryToAttack();

		float v = GetComponent<Rigidbody>().velocity.magnitude;
		if(v > 0) {
			Debug.Log(name + " " + v);
		}
	}

	private Mover getTarget() {
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(r, out hit, Mathf.Infinity, GameManager.enemyMask)) {
			return hit.transform.GetComponent<Mover>();
		}

		return null;
	}

	private Vector3 getMousePosition () {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -1 * Camera.main.transform.position.z;

		return Camera.main.ScreenToWorldPoint (mousePos);
	}

	void OnDestroy() {
		if(gameOverPanel != null) {
			gameOverPanel.SetActive(true);
		}
	}
}
