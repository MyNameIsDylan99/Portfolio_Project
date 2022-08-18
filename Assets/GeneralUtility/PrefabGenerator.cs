
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class PrefabGenerator {
    public static void SaveAsPrefab(GameObject gameObject, string path) {

        if (!path.Contains(".prefab")) {
            path += ".prefab";
        }

        path = AssetDatabase.GenerateUniqueAssetPath(path);
        PrefabUtility.SaveAsPrefabAsset(gameObject, path);

    }
}
