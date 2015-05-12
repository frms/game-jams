using UnityEngine;
using System.Collections;

public class BallBoundary : MonoBehaviour {
	private EdgeCollider2D col;

	// Use this for initialization
	void Awake () {
		float halfY = Camera.main.orthographicSize;
		float halfX = halfY * Camera.main.aspect;

		Vector2[] points = new Vector2[5];

		points [0] = new Vector2 (-halfX, -halfY);
		points [1] = new Vector2 (halfX, -halfY);
		points [2] = new Vector2 (halfX, halfY);
		points [3] = new Vector2 (-halfX, halfY);
		points [4] = new Vector2 (-halfX, -halfY);

		col = GetComponent<EdgeCollider2D> ();

		col.points = points;
	}

}
