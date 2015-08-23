using UnityEngine;
using System.Collections;

public class Enemy1 : Mover {

	public bool isLooping;

	private Renderer rend;
	private Color initialColor;

	private static Color r = new Color( 1f, 0.341f, 0.133f);

	public override void Start() {
		base.Start();

		rend = GetComponent<Renderer>();
		initialColor = rend.material.color;
	}
	
	public virtual void OnMouseEnter() {
		rend.material.color = r;
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
