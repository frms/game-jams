using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartBtn : MonoBehaviour {

	public void restart() {
		SceneManager.LoadScene("Scene1");
		//GameManager.Instance.sceneIsEnding = true;
	}
}
