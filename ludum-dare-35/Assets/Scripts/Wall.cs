using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Fader f = GetComponent<Fader>();
        f.setAlpha(0);

        f.targetAlpha(1, 0.5f + 1.7f*Random.value);
        //f.targetAlpha(1, 1.5f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
