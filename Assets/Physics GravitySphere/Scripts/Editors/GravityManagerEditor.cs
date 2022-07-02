using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GravityManager))]
[CanEditMultipleObjects]
public class GravityManagerEditor : Editor
{
    SerializedProperty RandomGravityReceiverAmount;
    SerializedProperty GravityReceiverPrefab;
    SerializedProperty GravityTriggerPrefab;
    SerializedProperty Mass;
    GUIStyle style = new GUIStyle();
    GravityManager _target;
    bool gravitySceneIsActive = false;
    private void OnEnable()
    {
        RandomGravityReceiverAmount = serializedObject.FindProperty("randomGravityReceiverAmount");
        GravityReceiverPrefab = serializedObject.FindProperty("gravityReceiverPrefab");
        GravityTriggerPrefab = serializedObject.FindProperty("gravityTriggerPrefab");
        Mass = serializedObject.FindProperty("mass");

        style.richText = true;
        _target = (GravityManager)target;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.LabelField("<color=red>Attention: the current state of the program only works properly in playmode</color>", style);
        if (EditorApplication.isPlaying == false)
            return;
        EditorGUILayout.LabelField("<b>Properties</b>", style);
        EditorGUILayout.PropertyField(RandomGravityReceiverAmount);
        EditorGUILayout.PropertyField(Mass);
        EditorGUILayout.Space(2);
        EditorGUILayout.LabelField("<b>Buttons</b>", style);
        if (GUILayout.Button(new GUIContent("Load Gravity Showcase Scene", "Press this button to load the Gravity Showcase Scene")) == true)
        {
            gravitySceneIsActive = true;
            _target.LoadGravityShowcaseScene();
           _target.FindGravityObjectsInSceneWithDelay(0.1f);
        }
        if (GUILayout.Button(new GUIContent("Unload Gravity Showcase Scene", "Press this button to unload the Gravity Showcase Scene")) == true)
        {
            _target.UnloadGravityShowcaseScene();
            gravitySceneIsActive=false;
        }
        EditorGUILayout.Space(2);
        if (gravitySceneIsActive)
        {
            if (GUILayout.Button(new GUIContent("Generate Gravity Receivers", "Press this button to generate a random amount of gravity receivers in the scene. Influenced by RandomGravityReceiverAmount Property.")) == true)
                _target.CreateGravityReceivers();
            if (GUILayout.Button(new GUIContent("Destroy Gravity Receivers", "Press this button to destroy all the gravity receivers currently in the scene.")) == true)
                _target.DestroyCurrentGravityReceivers();

            EditorGUILayout.Space(2);

            if (GUILayout.Button(new GUIContent("Generate Gravity Trigger", "Press this button to generate a GravityTrigger.")) == true)
                _target.CreateGravityTrigger();
            if (GUILayout.Button(new GUIContent("Destroy Gravity Trigger", "Press this button to destory the current gravity trigger.")) == true)
                _target.DestroyGravityTrigger();

            EditorGUILayout.Space(2);

            if (GUILayout.Button(new GUIContent("Activate Gravity", "Press this button to activate gravity for all gravity receivers")) == true)
                _target.ActivateGravity();
            if (GUILayout.Button(new GUIContent("Deactivate Gravity", "Press this button to deactivate gravity for all gravity receivers")) == true)
                _target.DeactivateGravity();

            EditorGUILayout.Space(2);
        }
        EditorGUILayout.LabelField("<b>References</b>", style);
        EditorGUILayout.PropertyField(GravityTriggerPrefab);
        EditorGUILayout.PropertyField(GravityReceiverPrefab);
        serializedObject.ApplyModifiedProperties();
    }
}
