﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static int enemyMask;

	void Awake() {
//		teamMemberMask = LayerMask.GetMask ("TeamMember", "Hero");
//		defaultMask = ~LayerMask.GetMask ("Ignore Raycast", "SelectionBox");
//
//		selectionBoxLayer = LayerMask.NameToLayer ("SelectionBox");

		enemyMask = LayerMask.GetMask ("Enemy");
	}
}