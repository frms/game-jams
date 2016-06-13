using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public string sceneName;


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
