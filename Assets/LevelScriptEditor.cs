using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelController))]
public class LevelScriptEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        LevelController myTarget = (LevelController)target;

        if (GUILayout.Button("Generate Grids"))
        {
            if (!myTarget.IsRandom()) myTarget.Init();
        }
    }
}