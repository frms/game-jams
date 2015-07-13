using UnityEngine;
using System.Collections;

public class Base : TeamMember {

	public bool[,] mapCollider = new bool[,] {
		{ true, true, true },
		{ true, true, true },
		{ true, true, true }
	};

	// Use this for initialization
	public override void Start () {
		base.Start();

		GetComponent<SpriteRenderer> ().color = teamId;
	}
}