using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(ColliderToMesh))]
public class ColliderToMeshInspector : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector ();
		
		ColliderToMesh myScript = (ColliderToMesh) target;
		
		if(GUILayout.Button("Collider To Mesh",  GUILayout.ExpandWidth(false)))
		{
			myScript.colliderToMesh();
		}
	}
}