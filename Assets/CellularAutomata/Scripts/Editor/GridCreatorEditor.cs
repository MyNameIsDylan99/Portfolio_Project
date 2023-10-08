using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace CellularAutomata {
    [CustomEditor(typeof(GridCreator))]
    [CanEditMultipleObjects]
    public class GridCreatorEditor : Editor {

        #region Fields
        SerializedProperty Height;
        SerializedProperty Width;
        SerializedProperty Depth;
        SerializedProperty CellPrefab;
        SerializedProperty GridContainerPrefab;
        SerializedProperty RandomFillPercent;
        SerializedProperty RaycastManager;
        SerializedProperty UpdateFrequency;
        SerializedProperty CreateSingleBlockInCenter;
        SerializedProperty DestroyPreviewMat;

        static GridCreator _target;

        GUIStyle style = new GUIStyle();
        GUIStyle handleStyle = new GUIStyle();

        Transform currentCameraTransform = null;

        static bool gridCreatorIsActive = false;

        static Mode mode;
        enum Mode {
            Neutral,
            Placing,
            Breaking
        }
        #endregion

        #region Methods
        private void OnEnable() {
            Height = serializedObject.FindProperty("height");
            Width = serializedObject.FindProperty("width");
            Depth = serializedObject.FindProperty("depth");
            CellPrefab = serializedObject.FindProperty("cellPrefab");
            GridContainerPrefab = serializedObject.FindProperty("gridPrefab");
            RandomFillPercent = serializedObject.FindProperty("randomFillPercent");
            RaycastManager = serializedObject.FindProperty("raycastManager");
            UpdateFrequency = serializedObject.FindProperty("updateFrequency");
            CreateSingleBlockInCenter = serializedObject.FindProperty("createSingleBlockInCenter");
            DestroyPreviewMat = serializedObject.FindProperty("destroyPreviewMat");
            _target = (GridCreator)target;
            style.richText = true;
            style.fontSize = 12;
            style.normal.textColor = Color.white;
            handleStyle.richText = true;
            handleStyle.fontSize = 18;
            gridCreatorIsActive = true;
        }
        public override void OnInspectorGUI() {

            serializedObject.Update();

            EditorGUILayout.LabelField("<b>Properties</b>", style);

            EditorGUILayout.PropertyField(Height);
            EditorGUILayout.PropertyField(Width);
            EditorGUILayout.PropertyField(Depth);
            EditorGUILayout.PropertyField(RandomFillPercent);
            EditorGUILayout.PropertyField(CreateSingleBlockInCenter);
            EditorGUILayout.PropertyField(UpdateFrequency);

            EditorGUILayout.Space(2);

            EditorGUILayout.LabelField("<b>Buttons</b>", style);

            if (GUILayout.Button(new GUIContent("GenerateGrid", "Press this button to generate a grid with the selected properties above")) == true)
                _target.GenerateGrid();

            if (GUILayout.Button(new GUIContent("DeleteGrid", "Press this button to delete the current grid")) == true) {
                _target.DeleteGrid();
                _target.DestroyPreviewCellBuildMode();
            }

            if (GUILayout.Button(new GUIContent("SaveGrid", "Press this button to save the current grid as a prefab")) == true) {
                _target.SaveGridContainerAsPrefab();
            }
                

            EditorGUILayout.Space(2);

            EditorGUILayout.LabelField("<b>References</b>", style);

            EditorGUILayout.PropertyField(CellPrefab);
            EditorGUILayout.PropertyField(GridContainerPrefab);
            EditorGUILayout.PropertyField(RaycastManager);
            EditorGUILayout.PropertyField(DestroyPreviewMat);

            serializedObject.ApplyModifiedProperties();


        }
           public void OnSceneGUI() {
            if (currentCameraTransform == null)
                currentCameraTransform = Camera.current.transform;

            Vector3 cameraPosition = currentCameraTransform.position;

            Vector3 handlePosition = cameraPosition + currentCameraTransform.forward * 2 + currentCameraTransform.right * -1.5f + currentCameraTransform.transform.up;


            Handles.Label(handlePosition, "<b><color=black>Hotkeys : BuildMode: B | DestroyMode : D | Build/Destroy : 1 </color></b>", handleStyle);

            handlePosition -= currentCameraTransform.up * 0.1f;
            switch (mode) {
                case Mode.Placing:
                    Handles.Label(handlePosition, "<b><color=green>Build</color></b>", handleStyle);
                    _target.ShowPreviewCellBuildMode();
                    break;
                case Mode.Neutral:
                    Handles.Label(handlePosition, "<b><color=black>Neutral</color></b>", handleStyle);
                    break;
                case Mode.Breaking:
                    Handles.Label(handlePosition, "<b><color=red>Destroy</color></b>", handleStyle);
                    _target.PreviewRaycastTargetDestroyMode();
                    break;
            }

        }

        #endregion

        #region MenuItems

        [MenuItem("MyMenu / ActivatePlacingMode _b")]
        static void ActivatePlacingMode() {
            if (!gridCreatorIsActive)
                return;
            _target.RemoveAllPreviews();
            if (mode == Mode.Placing) {
                mode = Mode.Neutral;
            }
            else {
                mode = Mode.Placing;
            }

        }
        [MenuItem("MyMenu / ActivateBreakMode _d")]
        static void ActivateBreakMode() {
            if (!gridCreatorIsActive)
                return;
            _target.RemoveAllPreviews();

            if (mode == Mode.Breaking) {
                mode = Mode.Neutral;
            }
            else {
                mode = Mode.Breaking;
            }
        }
        [MenuItem("MyMenu / BreakOrPlace _1")]
        static void AddPreviewCellToGrid() {
            if (!gridCreatorIsActive)
                return;
            switch (mode) {
                case Mode.Placing:
                    _target.AddPreviewCellToGrid();
                    break;
                case Mode.Neutral:
                    break;
                case Mode.Breaking:
                    _target.SetRaycastTargetToAir();
                    break;
            }

        }
        #endregion
    }
}