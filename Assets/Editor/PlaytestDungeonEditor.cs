using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlaytestDungeon))]
public class PlaytestDungeonEditor : Editor
{
    private string mapname;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlaytestDungeon script = (PlaytestDungeon)target;

        mapname = EditorGUILayout.TextField(mapname);

        if (GUILayout.Button("Playtest Map"))
        {
            if (Application.isPlaying)
            {
                if (FileIO.MapExists(mapname))
                {
                    script.InitialiseDungeon(FileIO.LoadMap(mapname));
                }
                else
                {
                    Debug.Log("Map didn't exist");
                }
            }
        }
    }
}
