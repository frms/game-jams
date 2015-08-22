using UnityEngine;
using System.Collections;

public class Enemy1 : Mover {

	public bool isLooping;

	// Update is called once per frame
	void Update () {
		moveUnit (isLooping);
	}
}
