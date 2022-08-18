using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
[CustomEditor(typeof(RaycastManager))]
public class EditorRaycastManager : Editor {

    #region Fields
    RaycastManager _target;
    Ray ray;
    RaycastHit hit;
    #endregion

    #region Methods
    private void OnEnable() {
        _target = (RaycastManager)target;
    }
    private void OnSceneGUI() {
        ShootRayCast();
    }
    void ShootRayCast() {
        ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Physics.Raycast(ray, out hit, 1000f);
        _target.ray = ray;
        _target.hit = hit;
    }
    #endregion

}
