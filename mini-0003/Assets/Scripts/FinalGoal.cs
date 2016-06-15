using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalGoal : MonoBehaviour
{
    public GameObject winText;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.GetComponent<Player>().enabled = false;
            winText.SetActive(true);
        }
    }
}
