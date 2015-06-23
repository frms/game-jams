using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float scrollSpeed = 70;
	public int scrollArea = 5;

	private TileMap tm;

	void Start() {
		tm = GameObject.Find ("TileMap").GetComponent<TileMap> ();
	}

	// Update is called once per frame
	void Update () {
		// Do camera movement by keyboard 
		Vector3 dir = new Vector3 ();
		dir.x = (Input.GetAxisRaw ("Horizontal") != 0) ? Mathf.Sign (Input.GetAxisRaw ("Horizontal")) : 0;
		dir.y = (Input.GetAxisRaw ("Vertical") != 0) ? Mathf.Sign (Input.GetAxisRaw ("Vertical")) : 0;

		if (dir == Vector3.zero) {
			float mPosX = Input.mousePosition.x; 
			float mPosY = Input.mousePosition.y;
		
			// Do camera movement by mouse position
			if (mPosX < scrollArea && mPosX >= -scrollArea) {
				dir.x = -1;
			}
			if (mPosX >= Screen.width - scrollArea && mPosX <= Screen.width+scrollArea) {
				dir.x = 1;
			}
			if (mPosY < scrollArea && mPosY >= -scrollArea) {
				dir.y = -1;
			}
			if (mPosY >= Screen.height - scrollArea && mPosY <= Screen.height+scrollArea) {
				dir.y = 1;
			}
		}

		dir.Normalize ();
		dir *= scrollSpeed * Time.deltaTime;
		transform.Translate (dir);

		keepOnMap ();
	}

	private void keepOnMap() {
		Rect rect = getCameraRect ();

		Vector3 newPos = transform.position;

		if (rect.x < tm.tileSize) {
			newPos.x = rect.width / 2 + tm.tileSize;
		} else if (rect.xMax > (tm.mapWidth-1) * tm.tileSize) {
			newPos.x = (tm.mapWidth-1) * tm.tileSize - rect.width / 2;
		}
		
		if (rect.y < tm.tileSize) {
			newPos.y = rect.height / 2 + tm.tileSize;
		} else if (rect.yMax > (tm.mapHeight-1) * tm.tileSize) {
			newPos.y = (tm.mapHeight-1) * tm.tileSize - rect.height / 2;
		}

		transform.position = newPos;
	}

	private Rect getCameraRect() {
		float distToGrid = -1*transform.position.z;
		
		float angle = (GetComponent<Camera>().fieldOfView / 2) * Mathf.Deg2Rad;
		
		float halfHeight = Mathf.Tan (angle) * distToGrid;
		float halfWidth = GetComponent<Camera>().aspect * halfHeight;
		
		Rect bounds = new Rect();
		bounds.x = transform.position.x - halfWidth;
		bounds.y = transform.position.y - halfHeight;
		bounds.width = 2 * halfWidth;
		bounds.height = 2 * halfHeight;

		return bounds;
	}
}
