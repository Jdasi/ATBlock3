using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapManager script = (MapManager)target;
        
        if (GUILayout.Button("Create Map"))
        {
            if (Application.isPlaying)
            {
                script.CreateMap();
            }
        }

        if (GUILayout.Button("Toggle Map Editor"))
        {
            if (Application.isPlaying)
            {
                script.ToggleMapEditor();
            }
        }
    }
}
