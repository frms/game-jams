using UnityEngine;
using System.Collections;

public class RestartBtn : MonoBehaviour {

	public void restart() {
		Application.LoadLevel("Scene1");
		GameManager.Instance.sceneIsEnding = true;
	}
}
