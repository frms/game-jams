using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Map))]
public class MapInspector : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Map myScript = (Map)target;

        if (GUILayout.Button("Recreate Map", GUILayout.ExpandWidth(false)))
        {
            myScript.buildMap();
        }
    }
}
