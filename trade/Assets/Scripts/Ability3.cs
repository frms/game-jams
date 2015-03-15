using UnityEngine;

public class Ability3 : Ability, PointUse {
	public float time = 2;
	public float aoeSize = 3.25f;

	private int groundMask;
	private GameObject aoe;

	// Use this for initialization
	public void Start () {
		groundMask = (1 << LayerMask.NameToLayer("Ground"));

		GameObject bc = GameObject.Find ("BattleController");
		aoe = bc.transform.Find("AreaOfEffect").gameObject;
	}

	private bool touchDownHappened = false;
	
	/* This function is called once per frame when the ability is in use. Returns 
	 * true if the ability is still running and false if its done.
	 */
	public override bool handleUserInput() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask)) {
			// If touched down has happened then show the AOE circle
			if (Input.GetMouseButtonDown(0)) {
				touchDownHappened = true;
			}
			else if(touchDownHappened) {
				// Move the aoe circle to the cursor location
				aoe.transform.position = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
				aoe.transform.localScale = new Vector3(aoeSize, aoeSize, 1);
				aoe.SetActive(true);

				// If touch up happens then use the ability at the current location.
				if(Input.GetMouseButtonUp(0)) {
					use(hit.point);

					touchDownHappened = false;
					aoe.SetActive(false);
					return false;
				}
			}
		}

		return true;
	}

	public void use(Vector3 point) {
		if(sleepTargets(point)) {
			resetCoolDown();
		}
	}

	private bool sleepTargets(Vector3 point) {
		GameObject[] members = GameObject.FindGameObjectsWithTag("Member");
		
		bool targetFound = false;
		
		foreach(GameObject go in members) {
			if(Vector3.Distance(point, go.transform.position) <= (aoeSize/2f)) {
				Ability a = go.GetComponent<Ability>();
				
				if(a.playerIndex != playerIndex) {
					a.sleep(time);
					targetFound = true;
				}
			}
		}

		return targetFound;
	}
}
