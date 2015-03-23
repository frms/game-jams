using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyA : MonoBehaviour {
	private MapData map;
	
	// Use this for initialization
	void Start () {
		map = GameObject.Find ("Map").GetComponent<TileMap> ().map;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// A Button
		if (Input.GetButtonDown ("Fire1")) {
			setStart();
		}
		// B Button
		else if (Input.GetButtonDown ("Fire2")) {
			findPath();
		}
	}

	private int[] start;
	
	private void setStart() {
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		start = new int[2];
		start [0] = Mathf.FloorToInt (pos.x);
		start [1] = Mathf.FloorToInt (pos.y);
		print (start);
	}
	
	private void findPath() {
		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		sw.Start();
		
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		int[] end = new int[2];
		end [0] = Mathf.FloorToInt (pos.x);
		end [1] = Mathf.FloorToInt (pos.y);
		
		AStar a = new AStar();
		
		List<int[]> l = a.findPathAStar (map, start, end);
		sw.Stop();

		print (end);

		draw(l);
		//print (l);

		Debug.Log (sw.Elapsed.TotalMilliseconds);
	}
	
	private static void draw(List<int[]> l) {
		if (l == null) {
			Debug.Log("No path found");
		} else {
			for (int i = 0; i < l.Count-1; i++) {
				Vector3 p1 = new Vector3(l[i][0] + 0.5f, l[i][1] + 0.5f, 0);
				Vector3 p2 = new Vector3(l[i+1][0] + 0.5f, l[i+1][1] + 0.5f, 0);
				
				Debug.DrawLine(p1, p2, Color.cyan, Mathf.Infinity, false);
			}
		}
		Debug.Log ("-----------------------------------------------");
	}
	
	private static void print(List<int[]> l) {
		if (l == null) {
			Debug.Log("No path found");
		} else {
			for (int i = 0; i < l.Count; i++) {
				print (l[i]);
			}
		}
		Debug.Log ("-----------------------------------------------");
	}
	
	private static void print(int[] arr) {
		Debug.Log ("[" + arr[0] + ", " + arr[1] + "]");
	}
}
