using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartBtn : MonoBehaviour {

	public void restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
