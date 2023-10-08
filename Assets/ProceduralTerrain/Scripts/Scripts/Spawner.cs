using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject spawnObject;

    [SerializeField]
    Vector3 spawnOffset;
    [SerializeField]
    float minHeight;
    [SerializeField]
    float maxHeight;
    [SerializeField]
    [Range(1,100)]
    int randomFillPercent;
    [SerializeField]
    float spawnDensity = 0.5f;

    [SerializeField]
    Vector3[] vertices;

    public void SpawnObjects() {
        GameObject spawnedObjects = new GameObject("spawnedObjects");
        if(vertices == null) {
            return;
        }
        int modulo = (int)(1 / spawnDensity);
        for (int i = 0; i < vertices.Length; i++) {
            if (i % modulo == 0) {
                int rndm = Mathf.RoundToInt(Random.Range(1, 100));
                if (vertices[i].y >= minHeight && vertices[i].y <= maxHeight) {
                    if (rndm <= randomFillPercent) {
                        GameObject instance = Instantiate(spawnObject);
                        instance.transform.position = vertices[i] + spawnOffset;
                        instance.transform.SetParent(spawnedObjects.transform);
                    }
                }
            }
        }
       
    }

    public void SetVertices(Vector3[] vertices) { 
        this.vertices = vertices;
    }

    private void OnValidate() {

        if (spawnDensity == 0) {
            spawnDensity = 0.00001f;
        }

    }
}
