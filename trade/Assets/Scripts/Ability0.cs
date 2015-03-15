using UnityEngine;

public class Ability0 : DmgAbility, SingleTargetUse {

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
			Health health = go.GetComponent<Health>();
			health.applyDamage(dmg);
			resetCoolDown();
		}
	}
}
