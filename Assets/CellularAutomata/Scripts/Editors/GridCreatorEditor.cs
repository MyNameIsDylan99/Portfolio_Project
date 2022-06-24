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
    SerializedProperty GridPrefab;
    GridCreator _target;
    GUIStyle style = new GUIStyle();
    private void OnEnable()
    {
        Height = serializedObject.FindProperty("height");
        Width = serializedObject.FindProperty("width");
        Depth = serializedObject.FindProperty("depth");
        CellPrefab = serializedObject.FindProperty("cellPrefab");
        GridPrefab = serializedObject.FindProperty("gridPrefab");
        _target = (GridCreator)target;
        style.richText = true;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("<b>Properties</b>",style);
        EditorGUILayout.PropertyField(Height);
        EditorGUILayout.PropertyField(Width);
        EditorGUILayout.PropertyField(Depth);
        EditorGUILayout.PropertyField(CellPrefab);
        EditorGUILayout.PropertyField(GridPrefab);
        EditorGUILayout.Space(2);
        EditorGUILayout.LabelField("<b>Buttons</b>",style);
        if (GUILayout.Button(new GUIContent("GenerateGrid", "Press this button to generate a grid with the selected properties above")) == true)
            _target.GenerateGrid();
        if (GUILayout.Button(new GUIContent("DeleteGrid", "Press this button to delete the current grid")) == true)
            _target.DeleteGrid();
        if (GUILayout.Button(new GUIContent("UpdateGrid", "Press this button to start the simulation")) == true)
            _target.UpdateGrid();
        serializedObject.ApplyModifiedProperties();
    }
}
