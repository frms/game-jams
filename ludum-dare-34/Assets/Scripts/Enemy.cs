using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public Transform atkPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetButtonDown("Jump"))
        {
            Instantiate(atkPrefab, transform.position, Quaternion.identity);
        }
	}
}
