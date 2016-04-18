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



	//public GameObject winPanel;

	//public bool sceneIsEnding = false;

	public static int dontSense;
    public static float playerBulletDist;

    public static int enemyKillCount = 0;

    public static int bubbleDestroyed = 0;

	void Awake() {
		dontSense = LayerMask.NameToLayer("DontSense");
	}

    void Start()
    {
        Vector3 screenDiag = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10)) - Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10));
        playerBulletDist = 0.5f * 1.5f * screenDiag.x;

        enemyKillCount = 0;
        bubbleDestroyed = 0;
    }

	// Update is called once per frame
	void Update () {
		//GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		//if(enemies.Length <= 0) {
		//	winPanel.SetActive(true);
		//}
	}
}
