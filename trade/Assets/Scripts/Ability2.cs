using UnityEngine;
using System.Collections;

public class Ability2 : Ability, SingleTargetUse {

	public float statChange = 5;

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
			DmgAbility dmgAbility = go.GetComponent<DmgAbility>();
			
			// If the game object has the damage stat then buff/nerf it
			if(dmgAbility != null) {
				if(dmgAbility.playerIndex == this.playerIndex) {
					dmgAbility.dmg += statChange;
				} else {
					dmgAbility.dmg -= statChange;
				}
				
				resetCoolDown();
			}
		}
	}
}
