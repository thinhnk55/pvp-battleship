using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WSClientHandler))]
public class WSClientEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();

        if (GUILayout.Button("Request Server"))
        {
            //((WSClient)target).RequestServer();
        }
    }
}
