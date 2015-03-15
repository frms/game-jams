using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour {

	public Ability ability;

	private BattleController bc;

	private Player user;

	private Color originalColor;

	private int memberMask;

	public void Start() {
		memberMask = (1 << LayerMask.NameToLayer("Member"));
		bc = GameObject.Find ("BattleController").GetComponent<BattleController> ();
	}
	
	// Update is called once per frame
	void Update(){
		// If an ability is selected then use it instead of normal user input controls
		if(ability != null) {
			// If the ability is no longer in use then clear the selected ability
			if(!ability.handleUserInput()) {
				unselectObject();
			}
		}
		else if (Input.GetMouseButtonDown(0)){
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, memberMask)) {
				selectObject(hit.transform.gameObject);
			}
		}
		else {
			handleNumberKey();
		}
	}

	private void handleNumberKey() {
		// Check each number key for a key down event
		for ( int i = 0; i < 10; ++i ) {
			if ( Input.GetKeyDown( i.ToString() ) ) {
				// Convert the number key to the correct index for the party array
				int index = (i != 0) ? i : 10;
				index--;

				// Get the user's party
				GameObject[] p = bc.players[0].party;

				// If the index is with in range the party member slot is not null then select that party member
				if(index < p.Length && p[index] != null)
					selectObject(p[index]);
			}
		}
	}

	private void selectObject(GameObject go) {
		Ability a = go.GetComponent<Ability>();

		// If the enemies are using AI then only allow user party members to be selected if they are ready.
		// Else select any party member that is ready.
		if( ((bc.useAI && a.playerIndex == 0) || !bc.useAI) && a.isReady() ) {
			ability = a;
			originalColor = go.renderer.material.color;
			go.renderer.material.color = originalColor + Color.white * 0.4f;
		}
	}

	private void unselectObject() {
		ability.renderer.material.color = originalColor;
		ability = null;
	}
}
