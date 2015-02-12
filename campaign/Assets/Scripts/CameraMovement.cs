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
		float mPosX = Input.mousePosition.x; 
		float mPosY = Input.mousePosition.y;
		
		// Do camera movement by mouse position
		if (mPosX < scrollArea && mPosX >= 0) {
			transform.Translate(Vector3.right * -scrollSpeed * Time.deltaTime);
		}
		if (mPosX >= Screen.width-scrollArea && mPosX <= Screen.width) {
			transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
		}
		if (mPosY < scrollArea && mPosY >= 0) {
			transform.Translate(Vector3.up * -scrollSpeed * Time.deltaTime);
		}
		if (mPosY >= Screen.height-scrollArea && mPosY <= Screen.height) {
			transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
		}
		
		// Do camera movement by keyboard 
		Vector3 keyDir = new Vector3 ();
		keyDir.x = (Input.GetAxisRaw ("Horizontal") != 0) ? Mathf.Sign (Input.GetAxisRaw ("Horizontal")) : 0;
		keyDir.y = (Input.GetAxisRaw ("Vertical") != 0) ? Mathf.Sign (Input.GetAxisRaw ("Vertical")) : 0;
		keyDir.Normalize ();

		keyDir *= scrollSpeed * Time.deltaTime;

		transform.Translate(keyDir);
	}
}
