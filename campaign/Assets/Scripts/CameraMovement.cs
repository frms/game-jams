using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float scrollSpeed = 70;
	public int scrollArea = 5;

	// Use this for initialization
	void Start () {
	
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
	}
}
