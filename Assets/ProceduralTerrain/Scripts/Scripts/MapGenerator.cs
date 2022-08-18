using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

    #region Properties

    public bool AutoUpdate { get => autoUpdate; }

    #endregion

    #region Fields
    public enum DrawMode { NoiseMap, ColourMap, Mesh, FalloffMap };

    [SerializeField]
    DrawMode drawMode;

    [SerializeField]
    Noise.NormalizeMode normalizeMode;

    public const int MapChunkSize = 239;
    [Range(0, 6)]
    [SerializeField]
    int editorPreviewLOD;
    [SerializeField]
    float noiseScale;

    [SerializeField]
    int octaves;
    [Range(0, 1)]
    [SerializeField]
    float persistance;
    [SerializeField]
    float lacunarity;

    [SerializeField]
    int seed;
    [SerializeField]
    Vector2 offset;

    [SerializeField]
    bool useFalloff;

    [SerializeField]
    float meshHeightMultiplier;
    [SerializeField]
    AnimationCurve meshHeightCurve;

    [SerializeField]
    bool autoUpdate;

    [SerializeField]
    TerrainType[] regions;
    [SerializeField]
    List<ParameterPreset> parameterPresets = new List<ParameterPreset>();

    float[,] falloffMap;

    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    #endregion

    #region Methods
    void Awake() {
        falloffMap = FalloffGenerator.GenerateFalloffMap(MapChunkSize);
    }

    public void DrawMapInEditor() {
        MapData mapData = GenerateMapData(Vector2.zero);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColourMap) {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(mapData.colourMap, MapChunkSize, MapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh) {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editorPreviewLOD), TextureGenerator.TextureFromColourMap(mapData.colourMap, MapChunkSize, MapChunkSize));
        }
        else if (drawMode == DrawMode.FalloffMap) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(MapChunkSize)));
        }
    }

    public void RequestMapData(Vector2 centre, Action<MapData> callback) {
        ThreadStart threadStart = delegate {
            MapDataThread(centre, callback);
        };

        new Thread(threadStart).Start();
    }

    void MapDataThread(Vector2 centre, Action<MapData> callback) {
        MapData mapData = GenerateMapData(centre);
        lock (mapDataThreadInfoQueue) {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }

    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback) {
        ThreadStart threadStart = delegate {
            MeshDataThread(mapData, lod, callback);
        };

        new Thread(threadStart).Start();
    }

    void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback) {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod);
        lock (meshDataThreadInfoQueue) {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }

    void Update() {
        if (mapDataThreadInfoQueue.Count > 0) {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++) {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }

        if (meshDataThreadInfoQueue.Count > 0) {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++) {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    MapData GenerateMapData(Vector2 centre) {
        float[,] noiseMap = Noise.GenerateNoiseMap(MapChunkSize + 2, MapChunkSize + 2, seed, noiseScale, octaves, persistance, lacunarity, centre + offset, normalizeMode);

        Color[] colourMap = new Color[MapChunkSize * MapChunkSize];
        for (int y = 0; y < MapChunkSize; y++) {
            for (int x = 0; x < MapChunkSize; x++) {
                if (useFalloff) {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight >= regions[i].height) {
                        colourMap[y * MapChunkSize + x] = regions[i].colour;
                    }
                    else {
                        break;
                    }
                }
            }
        }


        return new MapData(noiseMap, colourMap);
    }

    void OnValidate() {
        if (lacunarity < 1) {
            lacunarity = 1;
        }
        if (octaves < 0) {
            octaves = 0;
        }

        falloffMap = FalloffGenerator.GenerateFalloffMap(MapChunkSize);
    }

    public void SaveParametersAsPreset(string name) {
        parameterPresets.Add(new ParameterPreset(name, drawMode, normalizeMode, editorPreviewLOD, noiseScale, octaves, persistance, lacunarity, seed, offset, useFalloff, meshHeightMultiplier, meshHeightCurve, regions));
    }

    public void UseParameterPreset0() {
        if (parameterPresets.Count > 0) {

            drawMode = parameterPresets[0].DrawMode;
            normalizeMode = parameterPresets[0].NormalizeMode;
            editorPreviewLOD = parameterPresets[0].EditorPreviewLOD;
            noiseScale = parameterPresets[0].NoiseScale;
            octaves = parameterPresets[0].Octaves;
            persistance = parameterPresets[0].Persistance;
            lacunarity = parameterPresets[0].Lacunarity;
            seed = parameterPresets[0].Seed;
            offset = parameterPresets[0].Offset;
            useFalloff = parameterPresets[0].UseFalloff;
            meshHeightMultiplier = parameterPresets[0].MeshHeightMultiplier;
            meshHeightCurve = new AnimationCurve(parameterPresets[0].MeshHeightCurve.keys);
            regions = parameterPresets[0].Regions;
        }
    }
    #endregion

    #region Classes
    struct MapThreadInfo<T> {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter) {
            this.callback = callback;
            this.parameter = parameter;
        }

    }

    [Serializable]
    struct ParameterPreset {

        public string Name;

        public DrawMode DrawMode;
        public Noise.NormalizeMode NormalizeMode;
        [Range(0, 6)]
        public int EditorPreviewLOD;
        public float NoiseScale;
        public int Octaves;
        [Range(0, 1)]
        public float Persistance;
        public float Lacunarity;
        public int Seed;
        public Vector2 Offset;
        public bool UseFalloff;
        public float MeshHeightMultiplier;
        public AnimationCurve MeshHeightCurve;
        public TerrainType[] Regions;

        public ParameterPreset(string name, DrawMode drawMode, Noise.NormalizeMode normalizeMode, int editorPreviewLOD, float noiseScale, int octaves, float persistance, float lacunarity, int seed, Vector2 offset, bool useFalloff, float meshHeightMultiplier, AnimationCurve meshHeightCurve, TerrainType[] regions) {
            this.Name = name;
            this.DrawMode = drawMode;
            this.NormalizeMode = normalizeMode;
            this.EditorPreviewLOD = editorPreviewLOD;
            this.NoiseScale = noiseScale;
            this.Octaves = octaves;
            this.Persistance = persistance;
            this.Lacunarity = lacunarity;
            this.Seed = seed;
            this.Offset = offset;
            this.UseFalloff = useFalloff;
            this.MeshHeightMultiplier = meshHeightMultiplier;
            this.MeshHeightCurve = meshHeightCurve;
            this.Regions = regions;
        }
    }
    #endregion
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color colour;
}

public struct MapData {
    public readonly float[,] heightMap;
    public readonly Color[] colourMap;

    public MapData(float[,] heightMap, Color[] colourMap) {
        this.heightMap = heightMap;
        this.colourMap = colourMap;
    }
}
