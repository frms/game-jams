using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject winPanel;

	public static int enemyMask;

	void Awake() {
//		teamMemberMask = LayerMask.GetMask ("TeamMember", "Hero");
//		defaultMask = ~LayerMask.GetMask ("Ignore Raycast", "SelectionBox");
//
//		selectionBoxLayer = LayerMask.NameToLayer ("SelectionBox");

		enemyMask = LayerMask.GetMask ("Enemy");
	}

	// Update is called once per frame
	void Update () {
		if(Map.enemiesRemaining <= 0) {
			winPanel.SetActive(true);
		}
	}
}
