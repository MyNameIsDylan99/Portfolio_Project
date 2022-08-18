using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor {

	string parameterPresetName = "Parameter Preset Name";

	public override void OnInspectorGUI() {
		MapGenerator mapGen = (MapGenerator)target;

		if (DrawDefaultInspector ()) {
			if (mapGen.AutoUpdate) {
				mapGen.DrawMapInEditor ();
			}
		}

		if (GUILayout.Button ("Generate")) {
			mapGen.DrawMapInEditor ();
		}

		GUILayout.Space(10f);

		parameterPresetName = EditorGUILayout.TextField (parameterPresetName);
		if (GUILayout.Button(new GUIContent("Save Current Parameters as Preset"))) {
			mapGen.SaveParametersAsPreset(parameterPresetName);
		}

        if (GUILayout.Button(new GUIContent("Use Parameter Preset","Sets the parameter values to the values of the 0th element of the parameter preview array."))) {
			mapGen.UseParameterPreset0();
			mapGen.DrawMapInEditor();
        }

    }
}
