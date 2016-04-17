using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartBtn : MonoBehaviour {

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            restart();
        }
    }

	public void restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
