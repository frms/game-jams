using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {
    public float time;

    private float endTime;

	// Use this for initialization
	void Start () {
        endTime = Time.time + time;
	}
	
	// Update is called once per frame
	void Update () {
        if (endTime < Time.time)
            Destroy(gameObject);
	}
}
