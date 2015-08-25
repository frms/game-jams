using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	//Here is a private reference only this class can access
	private static GameManager _instance;
	
	//This is the public reference that other classes will use
	public static GameManager Instance
	{
		get
		{
			//If _instance hasn't been set yet, we grab it from the scene!
			//This will only happen the first time this reference is used.
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<GameManager>();
			return _instance;
		}
	}



	public GameObject winPanel;

	public bool sceneIsEnding = false;

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
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		if(enemies.Length <= 0) {
			winPanel.SetActive(true);
		}
	}
}
