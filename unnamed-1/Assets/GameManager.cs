﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public Transform city;
	public int numCities;

	public Transform playerCity;
	public Transform playerTroop;

	private List<Vector3> cities;
	private Vector3 nullPos = new Vector3 (-1000, -1000, -1000);

	public Transform groupOneTarget;

	// Use this for initialization
	void Start () {
		cities = new List<Vector3> ();

		addCity (playerCity);

		for (int i = 0; i < numCities; i++) {
			addCity(city);
		}

		InvokeRepeating("SendGroups", 0, 5);
	}

	private void addCity(Transform c) {
		Vector3 pos;
		
		for (int j = 0; j < 5; j++) {
			pos = getPos ();
			
			if (pos != nullPos) {
				Instantiate (c, pos, Quaternion.identity);
				cities.Add (pos);
				
				break;
			}
		}
	}

	private Vector3 getPos() {
		Vector3 pos = new Vector3(-4.5f + Random.Range(0, 10), 0, -4.5f + Random.Range(0, 10));

		for (int i = 0; i < cities.Count; i++) {
			if(Vector3.Distance(pos, cities[i]) < 1.5) {
				return nullPos;
			}
		}

		return pos;
	}

	void SendGroups() {
		if (groupOneTarget != null) {
			Transform t = Instantiate (playerTroop, cities[0], Quaternion.identity) as Transform;
			Troop troop = t.GetComponent<Troop>();
			troop.target = groupOneTarget.position;
			troop.startTime = Time.time;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
