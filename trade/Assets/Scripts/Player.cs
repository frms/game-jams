using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public string playerName;
	public int playerIndex;
	public GameObject[] party;

	private BattleController bc;

	public void Start() {
		bc = GameObject.Find ("BattleController").GetComponent<BattleController> ();
	}

	public void createMember(int index, GameObject go, Vector3 position, Dictionary<string, object> settings) {
		Ability a = createMember (index, go, position);
		a.setUp (settings);
	}

	public Ability createMember(int index, GameObject go, Vector3 position) {
		party[index] = Instantiate(go, position, Quaternion.identity) as GameObject;
		
		Ability a = party[index].GetComponent<Ability>();
		a.playerIndex = playerIndex;

		return a;
	}

	public bool hasPartyMember() {
		foreach(GameObject member in party) {
			if(member != null)
				return true;
		}
		
		return false;
	}

	public void updateAI() {
		foreach(GameObject go in party) {
			if(go != null) {
				Ability a = go.GetComponent<Ability>();

				if(a.isReady()) {
					if(a is SingleTargetUse) {
						// Find a random enemy
						List<GameObject> enemies = bc.getEnemies(playerIndex);
						GameObject target = enemies[(int)(Random.value*enemies.Count)];

						SingleTargetUse stu = a as SingleTargetUse;
						stu.use(target);
					} else if (a is PointUse) {
						Debug.Log ("No point use AI yet");
					}
				}
			}
		}
	}
}
