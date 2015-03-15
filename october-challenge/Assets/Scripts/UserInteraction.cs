using UnityEngine;
using System.Collections;
using System;

public class UserInteraction : MonoBehaviour {

	public float cameraLerpSmooth = 2;

	private Vector3 mouseStartPosition;
	private Vector3 lastMousePosition;
	private bool fingerIsTouching = false;

	private TileMap tileMap;
	private Player player;


	// Use this for initialization
	void Start () {
		tileMap = GameObject.Find ("TileMap").GetComponent<TileMap> ();
		player = GameObject.Find ("Player").GetComponent<Player> ();

		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
	}

	// Update is called once per frame
	void Update () {
		// Don't take in any user interaction anymore if the game is over
		if(tileMap.gameOver) {
			return;
		}

		// If the user touched down
		if(Input.GetMouseButtonDown(0)) {
			mouseStartPosition = Input.mousePosition;
			lastMousePosition = Input.mousePosition;

			fingerIsTouching = true;
		}
		// If user is dragging
		else if(fingerIsTouching && Input.GetMouseButton(0)) {
			// and if the camera is not moving towards the player then move the camera with the drag
			if(!lerpToPlayer && !followPlayer) {
				Vector3 mouseMoved = lastMousePosition - Input.mousePosition;

				Vector3 moveCam = screenToWorldDisplacement(mouseMoved);
				transform.position += moveCam;
			}

			lastMousePosition = Input.mousePosition;
		}
		// If the user released
		else if(Input.GetMouseButtonUp(0)) {
			fingerIsTouching = false;

			if(Input.mousePosition == mouseStartPosition) {
				touchTile ();
			}
		}
	}

	bool followPlayer = false;

	bool wasMovingLast = false;

	float timeToChangeScene = -1;

	void LateUpdate () {
		if (lerpToPlayer) {
			Vector2 newPos = Vector2.Lerp(transform.position, player.transform.position, Time.deltaTime*cameraLerpSmooth);
			transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

			if(Vector2.Distance(transform.position, player.transform.position) < 0.05) {
				player.moveTo(tileMap.indicesToWorldCoords(movePlayer[0], movePlayer[1]));
				lerpToPlayer = false;
			}
		} else {
			if(player.isMoving() || wasMovingLast) {
				transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
				
				wasMovingLast = player.isMoving();
			}
			// If we are here then the camera is not following the player in any way (lerp or otherwise),
			// so if the game over tile has appeared get ready to show the game over screen.
			else if(tileMap.gameOver) {

				// If we haven't set a time to change the scene do it now
				if(timeToChangeScene == -1) {
					timeToChangeScene = Time.time + 0.5f;
				}
				// If its time to change scene load the game over screen
				if(timeToChangeScene < Time.time) {
					Application.LoadLevel(1);
				}
			}
		}
	}

	private Vector3 screenToWorldDisplacement(Vector3 moved) {
		float distToGrid = Grid.zCoord - transform.position.z;
		
		float angle = (camera.fieldOfView / 2) * Mathf.Deg2Rad;
		
		float height = Mathf.Tan (angle) * distToGrid * 2;
		float width = camera.aspect * height;

		moved.x = (moved.x / Screen.width) * width;
		moved.y = (moved.y / Screen.height) * height;

		return moved;
	}

	bool lerpToPlayer = false;
	int[] movePlayer = new int[2];

	private void touchTile() {
		float distToGrid = Grid.zCoord - transform.position.z;
		
		Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distToGrid);
		Vector3 worldPos = camera.ScreenToWorldPoint(screenPos);
		
		//Debug.Log (worldPos);
		
		int i = Mathf.FloorToInt(worldPos.x / Grid.tileSize) + tileMap.originI;
		int j = Mathf.FloorToInt(worldPos.y / Grid.tileSize) + tileMap.originJ;
		
		//Debug.Log (i + " " + j);
		
		if(tileMap.tryToReachTile(i, j)) {

//			DateTime before = DateTime.Now;
			tileMap.colorPlayer(i, j);
//			DateTime after = DateTime.Now; 
//			TimeSpan duration = after.Subtract(before);
//			Debug.Log("Duration in milliseconds: " + duration.Milliseconds);

			// If the player is already moving towards a tile then update the tile he is moving towards
			if(player.isMoving() || wasMovingLast) {
				player.moveTo(tileMap.indicesToWorldCoords(i, j));
			}
			// Otherwise just update the movePlayer data that this script will use in order to move player once close enough to the player.
			else {
				movePlayer [0] = i;
				movePlayer [1] = j;
				lerpToPlayer = true;
			}
		}
	}

}
