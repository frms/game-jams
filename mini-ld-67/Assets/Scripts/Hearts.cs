using UnityEngine;
using System.Collections;

public class Hearts : MonoBehaviour {
    public Transform heartPrefab;

    private Health playerHealth;

    // Use this for initialization
    void Start () {
        playerHealth = GameObject.Find("Player").GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {
        float currentHealth = (playerHealth) ? playerHealth.currentHealth : 0;

        for (int i = transform.childCount; i < currentHealth; i++)
        {
            Transform t = Instantiate(heartPrefab) as Transform;
            t.SetParent(transform, false);
        }

        for (int i = transform.childCount; i > currentHealth; i--)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
