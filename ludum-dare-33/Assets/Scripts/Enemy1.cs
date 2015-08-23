﻿using UnityEngine;
using System.Collections;

public class Enemy1 : Mover {

	public bool isLooping;

	private Renderer rend;
	private Color initialColor;

	public override void Start() {
		base.Start();

		rend = GetComponent<Renderer>();
		initialColor = rend.material.color;
	}
	
	public virtual void OnMouseEnter() {
		rend.material.color = Color.red;
	}
	
	public virtual void OnMouseExit() {
		rend.material.color = initialColor;
	}

	// Update is called once per frame
	public virtual void Update () {
		if(isLooping && currentPath != null && isAtEndOfPath()) {
			currentPath = AStar.findPath(Map.map, currentPath.endNode, currentPath[0], null, 0);
		}

		moveUnit ();
	}

	public bool isAtEndOfPath () {
		return Vector3.Distance (currentPath.endNode, transform.position) < followPath.stopRadius;
	}

}
