using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(MapUtils))]
public class MapUtilsEditor : Editor {
	
	public override void OnInspectorGUI () {
//		EditorGUILayout.Vector3Field ("Look At Point", new Vector3(0, 1, 2));
//
//		if (GUI.changed)
//			EditorUtility.SetDirty (target);

		DrawDefaultInspector ();

		MapUtils myScript = (MapUtils) target;

		if(GUILayout.Button("Randomize Enemies"))
		{
			myScript.buildObject();
		}

		if(GUILayout.Button("Stop Wandering Enemies"))
		{
			myScript.stopWanderingEnemies();
		}

		if(GUILayout.Button("Start Wandering Enemies"))
		{
			myScript.startWanderingEnemies();
		}
	}
}
