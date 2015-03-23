using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyA : MonoBehaviour {
	private MapData map;
	private SteeringUtils steeringUtils;
	private FollowPath followPath;
	
	// Use this for initialization
	void Start () {
		map = GameObject.Find ("Map").GetComponent<TileMap> ().map;
		steeringUtils = GetComponent<SteeringUtils> ();
		followPath = GetComponent<FollowPath> ();
	}

	private LinePath currentPath;
	
	// Update is called once per frame
	void FixedUpdate () {
		// A Button
		if (Input.GetButtonDown ("Fire1")) {
			currentPath = null;
		}
		// B Button
		else if (Input.GetButtonDown ("Fire2")) {
			currentPath = findPath();
		}

		if (currentPath != null) {
			Vector2 accel = followPath.getSteering(currentPath);
			steeringUtils.steer(accel);
		}
	}

	private LinePath findPath() {
		int[] start = new int[2];
		start [0] = Mathf.FloorToInt (transform.position.x);
		start [1] = Mathf.FloorToInt (transform.position.y);

		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		sw.Start();
		
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		int[] end = new int[2];
		end [0] = Mathf.FloorToInt (pos.x);
		end [1] = Mathf.FloorToInt (pos.y);
		
		AStar a = new AStar();
		
		LinePath lp = a.findPathAStar (map, start, end);
		sw.Stop();

		print (end);

		if (lp != null) {
			lp.draw ();
		}
		//print (l);

		Debug.Log (sw.Elapsed.TotalMilliseconds);

		return lp;
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
