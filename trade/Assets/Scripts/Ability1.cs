using UnityEngine;
using System.Collections.Generic;

public class Ability1 : DmgAbility, SingleTargetUse {
	public int numOfTargets = 3;

	private BattleController bc;

	// Use this for initialization
	public void Start () {
		bc = GameObject.Find ("BattleController").GetComponent<BattleController> ();
	}
	
	/* This function is called once per frame when the ability is in use. Returns 
	 * true if the ability is still running and false if its done.
	 */
	public override bool handleUserInput() {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, memberMask)) {
				use (hit.transform.gameObject);
			}
			
			return false;
		}
		
		return true;
	}

	public void use(GameObject go) {
		if(go != gameObject) {
			chainLighting(go);
			resetCoolDown();
		}
	}

	private void chainLighting(GameObject firstTarget) {
		List<GameObject> targets = getTargets(firstTarget);

		int numOfFoundTargets = Mathf.Min (numOfTargets, targets.Count);

		// Attack all the chain lighting targets
		for(int i = 0; i < numOfFoundTargets; i++) {
			Health health = targets[i].GetComponent<Health>();
			health.applyDamage(dmg);
		}
	}

	private List<GameObject> getTargets(GameObject firstTarget) {
		List<GameObject> list = new List<GameObject>();

		// Loop through all enemy players 
		for(int i = 0; i < bc.players.Length; i++) {
			if( i != playerIndex) {
				GameObject[] party = bc.players[i].party;

				// Add each enemy party member to the list (besides the first target)
				for(int j = 0; j < party.Length; j++) {
					if(party[j] != null && party[j] != firstTarget) {
						list.Add(party[j]);
					}
				}
			}
		}

		// Randomize the first (numOfTargets - 1) number of enemies
		Utils.shuffle (list, numOfTargets - 1);

		//Add the first target to the begining of the list 
		list.Insert(0, firstTarget);

		return list;
	}
}
