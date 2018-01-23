using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomMapTester))]
public class RandomMapTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RandomMapTester script = (RandomMapTester)target;
        
        if (GUILayout.Button("Create Map"))
        {
            if (Application.isPlaying)
            {
                script.CreateMap();
            }
        }
    }
}
