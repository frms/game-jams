using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public Transform selectionBox;
	private SelectionBox sb;

	public Dictionary<Color, List<Hero>> teams = new Dictionary<Color, List<Hero>>();

	private Vector3 mouseDownPos;

	void Start() {
		sb = selectionBox.GetComponent<SelectionBox> ();
	}

	// Update is called once per frame
	void Update () {
		// Left Click
		if (Input.GetMouseButtonDown (0)) {
			Hero target = castRay ();

			if (target != null && target.teamId == TeamMember.TEAM_1) {
				selectHero (target);
			}

			mouseDownPos = Input.mousePosition;
		} else if (Input.GetMouseButton (0) && Vector3.Magnitude (mouseDownPos - Input.mousePosition) > 2) {
			selectionBox.gameObject.SetActive (true);

			selectionBox.position = screenToWorldPoint(mouseDownPos);

			selectionBox.localScale = screenToWorldPoint (Input.mousePosition) - selectionBox.position;
		} else {
			selectHeroes(sb.collidingHeroes);
			sb.turnOff();
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

	public void selectHeroes(HashSet<Hero> heroes) {
		if (heroes.Count > 0) {
			clearSelectedHeroes ();

			foreach (Hero h in heroes) {
				h.playerControlled = true;
			}
		}
	}

	public void selectHero(Hero h) {
		clearSelectedHeroes ();

		h.playerControlled = true;
	}

	public void clearSelectedHeroes() {
		List<Hero> team = teams [TeamMember.TEAM_1];
		
		for(int i = 0; i < team.Count; i++) {
			team[i].playerControlled = false;
		}
	}

	private Vector3 screenToWorldPoint (Vector3 point)
	{
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint (point);
		worldPoint.z = 0;
		return worldPoint;
	}
}
