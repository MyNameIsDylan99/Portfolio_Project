using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridCreator))]
[CanEditMultipleObjects]
public class GridCreatorEditor : Editor
{
    SerializedProperty Height;
    SerializedProperty Width;
    SerializedProperty Depth;
    SerializedProperty CellPrefab;
    private void OnEnable()
    {
        Height = serializedObject.FindProperty("height");
        Width = serializedObject.FindProperty("width");
        Depth = serializedObject.FindProperty("depth");
        CellPrefab = serializedObject.FindProperty("cellPrefab");
    }
    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();

    //}
}
