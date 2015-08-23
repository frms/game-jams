using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MapBuilder {
	public int mapWidth;
	public int mapHeight;
	public int numberOfRooms;
	public bool overlappingRooms;
	public bool mirrorMap;
	public int[] roomWidthRange;
	public int[] roomHeightRange;
	public int[] innerHallwayWidthRange;

	private MapData map;
	private List<Room> rooms;

	public MapBuilder() {
		this.mapWidth = 30;
		this.mapHeight = 30;
		this.numberOfRooms = 20;
		this.overlappingRooms = false;
		this.mirrorMap = false;
		this.roomWidthRange = new [] {4, 8};
		this.roomHeightRange = new [] {4, 8};
		this.roomHeightRange = new [] {1, 1};
	}


}
