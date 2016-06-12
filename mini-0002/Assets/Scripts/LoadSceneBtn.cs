using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneBtn : MonoBehaviour
{
    public string sceneName;

    public void loadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
