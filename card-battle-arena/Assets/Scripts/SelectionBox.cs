using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionBox : MonoBehaviour {

	public HashSet<Hero> collidingHeroes = new HashSet<Hero>();

	void OnTriggerEnter2D(Collider2D other) {
		Hero h = other.GetComponent<Hero> ();
		if (h != null && h.teamId == TeamMember.TEAM_1) {
			collidingHeroes.Add (h);
			h.inSelectionBox = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		Hero h = other.GetComponent<Hero> ();
		if (h != null && h.teamId == TeamMember.TEAM_1) {
			collidingHeroes.Remove (h);
			h.inSelectionBox = false;
		}
	}

	public void turnOff() {
		foreach (Hero h in collidingHeroes) {
			h.inSelectionBox = false;
		}

		collidingHeroes.Clear ();

		gameObject.SetActive (false);
	}
}
