using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleController : MonoBehaviour {

	public bool useAI = true;

	public float enemyCoolDownTime = 4f;
	public float enemyDmg = 5f;
	public float enemyHealth = 20f;

	public GameObject[] memberPrefabs;

	internal Player[] players;

	private string gameOverMsg;

	// Use this for initialization
	void Start () {
		players = new Player[2];

		Player user = createPlayer("You", 0, 5);
		for(int i = 0; i < 4; i++) {
			Vector3 pos = new Vector3(-2, 0.5f, i*1.5f - 2.25f);
			user.createMember(i, memberPrefabs[i], pos);
		}

		Player enemy = createPlayer("Enemy", 1, 18);

		Dictionary<string, object> settings = new Dictionary<string, object> ();
		settings ["coolDownTime"] = enemyCoolDownTime;
		settings ["dmg"] = enemyDmg;
		settings ["health"] = enemyHealth;

		for(int i = 0; i < 18; i++) {
			settings["sleepTime"] = Random.value*2;
			enemy.createMember(i, memberPrefabs[0], numberToPosition(i), settings);
		}
	}

	/**
	 * Given a number between 0 and 17 this function will return a position on the battlefield
	 */
	private Vector3 numberToPosition(int n) {
		Vector3 pos = new Vector3 ();
		pos.y = 0.5f;

		if(n >= 0 && n <= 3) {
			pos.x = 1.25f;
			pos.z = -2.25f + 1.5f*n;
		} else if(n >= 4 && n <= 8) {
			pos.x = 2.5f;
			pos.z = -3f + 1.5f*(n-4);
		} else if(n >= 9 && n <= 12) {
			pos.x = 3.75f;
			pos.z = -2.25f + 1.5f*(n-9);
		} else if(n >= 13 && n <= 17) {
			pos.x = 5f;
			pos.z = -3f + 1.5f*(n-13);
		}

		return pos;
	}

	private Player createPlayer(string name, int index, int partySize) {
		Player p = gameObject.AddComponent<Player>();
		p.playerName = name;
		p.playerIndex = index;
		p.party = new GameObject[partySize];

		players[index] = p;

		return p;
	}
	
	// Update is called once per frame
	void Update () {
		Player winner = isBattleOver ();

		if(winner != null) {
			gameOverMsg = winner.playerName;

			// Silly ingrish fix for when the user wins (who is currently named You right now)
			if(winner == players[0]) {
				gameOverMsg += " Win!";
			} else {
				gameOverMsg += " Wins!";
			}
		} else {
			if(useAI) {
				// Go through all players after player 0 (b/c player 0 is the user)
				for(int i = 1; i < players.Length; i++) {
					players[i].updateAI();
				}
			}
		}
	}

	private Player isBattleOver() {
		Player winner = null;

		for(int i = 0; i < players.Length; i++) {
			// If the player still has a party member left
			if(players[i].hasPartyMember()) {
				// If we have no winning player yet then make the current player the winning player
				if(winner == null) {
					winner = players[i];
				}
				// Else we have encountered another player with a party still left so set the 
				// winner to null and break the loop. The battle is still going on.
				else {
					winner = null;
					break;
				}
			}
		}
		
		return winner;
	}

	/**
	 * Get a list of enemies for the given player index
	 */
	public List<GameObject> getEnemies(int playerIndex) {
		List<GameObject> list = new List<GameObject>();
		
		// Loop through all enemy players 
		for(int i = 0; i < players.Length; i++) {
			if( i != playerIndex) {
				GameObject[] party = players[i].party;
				
				// Add each enemy party member to the list
				for(int j = 0; j < party.Length; j++) {
					if(party[j] != null) {
						list.Add(party[j]);
					}
				}
			}
		}
		
		return list;
	}

	private float native_width = 1920;
	private float native_height = 1080;

	private GUIStyle labelStyle;
	private GUIStyle buttonStyle;
	
	void OnGUI ()
	{
		Matrix4x4 saveMat = GUI.matrix; // save current matrix
		
		//set up scaling
		float ry = Screen.height / native_height;
		float rx = Screen.width / native_width;
		GUI.matrix = Matrix4x4.TRS (new Vector3(0, 0, 0), Quaternion.identity, new Vector3 (rx, ry, 1)); 
		
		// Do normal gui code from here on as though the resolution is guarenteed to be the native resolution

		if(gameOverMsg != null) {
			createStyles ();

			drawGameOverMsg ();
			
			drawRestartBtn ();
		}

		// Finish doing gui code
		
		GUI.matrix = saveMat; // restore matrix
	}

	/* Create the styles objects only once rather than every GUI frame */
	private void createStyles() {
		if(labelStyle == null) {
			labelStyle = new GUIStyle ();
			labelStyle.fontSize = 180;
			labelStyle.normal.textColor = Color.white;
			
			buttonStyle = new GUIStyle ("button");
			buttonStyle.fontSize = 110;
			buttonStyle.padding = new RectOffset (10, 10, 5, 5);
		}
	}

	private void drawGameOverMsg () {
		Vector2 size = labelStyle.CalcSize(new GUIContent(gameOverMsg));
		
		float x = native_width / 2 - size.x / 2;
		float y = 170;
		
		GUI.Label(new Rect(x, y, size.x, size.y), gameOverMsg, labelStyle);
	}

	private void drawRestartBtn() {
		string text = "restart";
		Vector2 size = buttonStyle.CalcSize(new GUIContent(text));
		
		float x = native_width / 2 - size.x / 2;
		float y = native_height / 2 - size.y / 2;
		
		if (GUI.Button (new Rect (x, y, size.x, size.y), text, buttonStyle)) {
			Application.LoadLevel(0);
		}
	}
}
