using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Map))]
public class MapInspector : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		//Map myScript = (Map) target;

		if(GUILayout.Button("Print Map",  GUILayout.ExpandWidth(false)))
		{
			Debug.Log (Map.map);
		}
	}
}
