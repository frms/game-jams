using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(MapHelper))]
public class MapHelperEditor : Editor {
	
	public override void OnInspectorGUI () {
		//		EditorGUILayout.Vector3Field ("Look At Point", new Vector3(0, 1, 2));
		//
		//		if (GUI.changed)
		//			EditorUtility.SetDirty (target);
		
		DrawDefaultInspector ();
		
		MapHelper myScript = (MapHelper) target;
		
		if(GUILayout.Button("Randomize Blocks"))
		{
			myScript.randomizeBlocks();
		}

		if(GUILayout.Button("Toggle Point Blocks Look"))
		{
			myScript.togglePointBlockLook();
		}

	}
}
