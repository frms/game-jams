using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	public static int duration = 60;
	public static int numberOfOpponents = 2;

	public string durationStr;
	public string numberOfOpponentsStr;

	// Use this for initialization
	void Start () {
		durationStr = duration.ToString ();
		numberOfOpponentsStr = numberOfOpponents.ToString ();
	}
	
	float native_width = 1920;
	float native_height = 1080;
	
	void OnGUI ()
	{
		Matrix4x4 saveMat = GUI.matrix; // save current matrix
		
		//set up scaling
		float rx = Screen.width / native_width;
		float ry = Screen.height / native_height;
		GUI.matrix = Matrix4x4.TRS (new Vector3(0, 0, 0), Quaternion.identity, new Vector3 (rx, ry, 1)); 
		
		// Do normal gui code from here on as though the resolution is guarenteed to be the native resolution

		drawTitle ();

		drawInputs ();
		
		// Finish doing gui code
		
		GUI.matrix = saveMat; // restore matrix
	}

	private void drawTitle () {
		GUIStyle style = new GUIStyle ();
		style.fontSize = 180;
		style.richText = true;
		
		string text1 = "<color=#159f09ff>Territory</color> <color=#7c0cccff>Draw</color>";
		Vector2 size1 = style.CalcSize(new GUIContent(text1));
		
		float x = native_width / 2 - size1.x / 2;
		float y = 170;
		
		GUI.Label(new Rect(x, y, size1.x, size1.y), text1, style);
	}

	private void drawInputs() {
		GUIStyle style = new GUIStyle ();
		style.fontSize = 80;
		
		string text1 = "Duration (seconds):";
		Vector2 size1 = style.CalcSize(new GUIContent(text1));

		GUI.Label(new Rect(519, 450, size1.x, size1.y), text1, style);

		GUIStyle tfStyle = new GUIStyle ("textfield");
		tfStyle.fontSize = 80;
		tfStyle.alignment = TextAnchor.MiddleRight;
		
		string text2 = "000";
		Vector2 size2 = tfStyle.CalcSize(new GUIContent(text2));

		durationStr = GUI.TextField(new Rect(1262, 447, size2.x, size2.y), durationStr, 3, tfStyle);

		
		string text3 = "Number of Opponents:";
		Vector2 size3 = style.CalcSize(new GUIContent(text3));

		GUI.Label(new Rect(486, 600, size3.x, size3.y), text3, style);

		string text4 = "00";
		Vector2 size4 = tfStyle.CalcSize(new GUIContent(text4));
		
		numberOfOpponentsStr = GUI.TextField(new Rect(1342, 596, size4.x, size4.y), numberOfOpponentsStr, 2, tfStyle);

		GUIStyle btnStyle = new GUIStyle ("button");
		btnStyle.fontSize = 80;
		btnStyle.padding = new RectOffset (10, 10, 5, 5);

		string text5 = "Play";
		Vector2 size5 = btnStyle.CalcSize(new GUIContent(text5));
		float x = native_width / 2 - size5.x / 2;

		if(GUI.Button(new Rect(x, 750, size5.x, size5.y), text5, btnStyle)) {
			duration = int.Parse(durationStr);
			numberOfOpponents = int.Parse(numberOfOpponentsStr);

			Application.LoadLevel(1);
		}
	}
}
