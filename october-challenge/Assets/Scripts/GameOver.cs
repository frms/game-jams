using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		player.transform.position = Vector3.zero;

		Player playerScript = player.GetComponent<Player> ();
		playerScript.targetPosition = Vector3.zero;
	}

	float native_width = 1920;
	float native_height = 1080;
	
	void OnGUI ()
	{
		Matrix4x4 saveMat = GUI.matrix; // save current matrix
		
		//set up scaling
		float ry = Screen.height / native_height;
		float rx = Screen.width / native_width;
		GUI.matrix = Matrix4x4.TRS (new Vector3(0, 0, 0), Quaternion.identity, new Vector3 (rx, ry, 1)); 
		
		// Do normal gui code from here on as though the resolution is guarenteed to be the native resolution

		GUIStyle buttonStyle = new GUIStyle ("button");
		buttonStyle.fontSize = 110;
		buttonStyle.padding = new RectOffset (10, 10, 5, 5);
		
		string text1 = "restart";
		Vector2 size1 = buttonStyle.CalcSize(new GUIContent(text1));
		
		float x = native_width / 2 - size1.x / 2;
		float y = native_height - size1.y - 50;
		
		if (GUI.Button (new Rect (x, y, size1.x, size1.y), text1, buttonStyle)) {
			Destroy(player);
			Application.LoadLevel(0);
		}
		
		// Finish doing gui code
		
		GUI.matrix = saveMat; // restore matrix
	}

}
