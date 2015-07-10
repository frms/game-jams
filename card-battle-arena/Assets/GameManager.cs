﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public Dictionary<Color, List<Hero>> teams = new Dictionary<Color, List<Hero>>();

	// Update is called once per frame
	void Update () {
		// Left Click
		if (Input.GetMouseButtonDown (0)) {
			Hero target = castRay ();

			if(target != null && target.teamId == TeamMember.TEAM_1) {
				selectHero(target);
			}
		}
	}

	private Hero castRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
		if (hit) {
			return hit.transform.GetComponent<Hero>();
		}
		
		return null;
	}

	public void addHero(Hero h) {
		List<Hero> team;

		if (teams.ContainsKey (h.teamId)) {
			team = teams[h.teamId];
		} else {
			team = new List<Hero>();
			teams[h.teamId] = team;
		}

		team.Add (h);
	}

	public void selectHero(Hero h) {
		List<Hero> team = teams [h.teamId];

		for(int i = 0; i < team.Count; i++) {
			team[i].playerControlled = false;
		}

		h.playerControlled = true;
	}
}
