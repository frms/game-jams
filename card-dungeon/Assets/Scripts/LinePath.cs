using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinePath  {
	private Vector3[] nodes;
	private float[] distances;
	public float maxDist;

	// Indexer declaration.
	public Vector3 this[int i]
	{
		get
		{
			return nodes[i];
		}
		
		set
		{
			nodes[i] = value;
		}
	}

	public int Length
	{
		get {
			return nodes.Length;
		}
	}

	/* This function creates a path of line segments */
	public LinePath(Vector3[] nodes) {
		this.nodes = nodes;

		calcDistances();
	}
	
	/* Loops through the path's nodes and determines how far each node in the path is 
	 * from the starting node */
	private void calcDistances() {
		distances = new float[nodes.Length];
		distances[0] = 0;

		for(var i = 0; i < nodes.Length - 1; i++) {
			distances[i+1] = distances[i] + Vector3.Distance(nodes[i], nodes[i+1]);
		}
		
		maxDist = distances[distances.Length-1];
	}
	
	/* Draws the path in the scene view */
	public void draw() {
		for (int i = 0; i < nodes.Length-1; i++) {
			Debug.DrawLine(nodes[i], nodes[i+1], Color.cyan, Mathf.Infinity, false);
		}
	}
	
	/* Gets the param for the closest point on the path given a position */
	public float getParam(Vector3 position) {
		/* Find the first point in the closest line segment to the path */
		float closestDist = distToSegment(position, nodes[0], nodes[1]);
		int closestSegment = 0;
		
		for(int i = 1; i < nodes.Length - 1; i++) {
			float dist = distToSegment(position, nodes[i], nodes[i+1]);
			
			if(dist < closestDist) {
				closestDist = dist;
				closestSegment = i;
			}
		}
		
		float param = this.distances[closestSegment] + getParamForSegment(position, nodes[closestSegment], nodes[closestSegment+1]);
		
		return param; 
	}
	
	/* Given a param it gets the position on the path */
	public Vector3 getPosition(float param) {
		/* Find the first node that is farther than given param */
		int i = 0;
		for(; i < distances.Length; i++) {
			if(distances[i] > param) {
				break;
			}
		}
		
		/* Convert it to the first node of the line segment that the param is in */
		if (i > distances.Length - 2) {
			i = distances.Length - 2;
		} else {
			i -= 1;
		}
		
		/* Get how far along the line segment the param is */
		float t = (param - distances[i]) / Vector3.Distance(nodes[i], nodes[i+1]);
		
		/* Get the position of the param */
		return Vector3.Lerp(nodes[i], nodes[i+1], t);
	}
	
	/* Gives the distance of a point to a line segment.
	 * p is the point, v and w are the two points of the line segment */
	private static float distToSegment(Vector3 p, Vector3 v, Vector3 w) { 
		float l2 = Vector3.Distance(v, w)*Vector3.Distance(v, w);
		
		if (l2 == 0) {
			return Vector3.Distance(p, v);
		}
		
		float t = ((p.x - v.x) * (w.x - v.x) + (p.y - v.y) * (w.y - v.y)) / l2;
		
		if (t < 0) {
			return Vector3.Distance(p, v);
		}
		
		if (t > 1) {
			return Vector3.Distance(p, w);
		}
		
		Vector3 closestPoint = Vector3.Lerp(v, w, t);
		
		return Vector3.Distance(p, closestPoint);
	}
	
	/* Finds the param for the closest point on the segment vw given the point p */
	private static float getParamForSegment(Vector3 p, Vector3 v, Vector3 w) {
		float l2 = Vector3.Distance(v, w)*Vector3.Distance(v, w);
		
		if (l2 == 0) {
			return 0;
		}
		
		float t = ((p.x - v.x) * (w.x - v.x) + (p.y - v.y) * (w.y - v.y)) / l2;
		
		if(t < 0) {
			t = 0;
		} else if (t > 1) {
			t = 1;
		}
		
		return t * Mathf.Sqrt(l2);
	}
}
