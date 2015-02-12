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
			if (mPosX < scrollArea && mPosX >= 0) {
				dir.x = -1;
			}
			if (mPosX >= Screen.width - scrollArea && mPosX <= Screen.width) {
				dir.x = 1;
			}
			if (mPosY < scrollArea && mPosY >= 0) {
				dir.y = -1;
			}
			if (mPosY >= Screen.height - scrollArea && mPosY <= Screen.height) {
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

		if (rect.x < 0) {
			newPos.x = rect.width / 2;
		} else if (rect.xMax > tm.mapWidth * tm.tileSize) {
			newPos.x = tm.mapWidth * tm.tileSize - rect.width / 2;
		}
		
		if (rect.y < 0) {
			newPos.z = rect.height / 2;
		} else if (rect.yMax > tm.mapHeight * tm.tileSize) {
			newPos.z = tm.mapHeight * tm.tileSize - rect.height / 2;
		}

		transform.position = newPos;
	}

	private Rect getCameraRect() {
		float distToGrid = transform.position.y;
		
		float angle = (camera.fieldOfView / 2) * Mathf.Deg2Rad;
		
		float halfHeight = Mathf.Tan (angle) * distToGrid;
		float halfWidth = camera.aspect * halfHeight;
		
		Rect bounds = new Rect();
		bounds.x = transform.position.x - halfWidth;
		bounds.y = transform.position.z - halfHeight;
		bounds.width = 2 * halfWidth;
		bounds.height = 2 * halfHeight;

		//print ("y: " + bounds.y + " ymax: " + bounds.yMax + " halfHeight: " + halfHeight);
		return bounds;
	}
}
