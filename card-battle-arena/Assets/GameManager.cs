using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public Dictionary<Color, List<Hero>> teams = new Dictionary<Color, List<Hero>>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
