using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;

namespace TerrainGenerator {

    [CustomEditor (typeof(Spawner))]
    [CanEditMultipleObjects]
    public class SpawnerEditor : Editor {

        Spawner _target;

        private void OnEnable() {
            _target = (Spawner)target;
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            if(GUILayout.Button("Spawn Objects")) {
                _target.SpawnObjects();
            }

        }

    }
}

