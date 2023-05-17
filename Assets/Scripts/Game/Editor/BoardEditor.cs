using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    public override void OnInspectorGUI()
    {

        if (GUILayout.Button("Init"))
        {
            ((Board)target).InitBoard(10,10);
        }
    }
}
