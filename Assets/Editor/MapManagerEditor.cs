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

        if (GUILayout.Button("Generate Map"))
        {
            if (Application.isPlaying)
            {
                script.GenerateMap();
            }
        }

        if (GUILayout.Button("Load Map"))
        {
            if (Application.isPlaying)
            {
                script.LoadMap();
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
