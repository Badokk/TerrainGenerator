using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class GuiScript : MonoBehaviour
{
    public bool heightMapAutoUpdate = false;
    public GameObject texturePreview;
    public bool textureAutoUpdate = false;
    public GameObject meshPreview;
    public bool meshAutoUpdate = false;
    public GameObject voxelPreview;

    public Defines.MapParams mapSize;
    [Range(0, 10)]
    public int layersToUse;
    public Defines.GeneratorLayer[] layerParams;
    public Vector3 globalScale = new Vector3(1, 1, 1);
    public Vector3 samplingRate = new Vector3(20, 20, 1);

    public GuiTexture guiTexture;
    public GuiMesh guiMesh;
    public GuiVoxel guiVoxel;
    [Range(0, 1)]
    public float spaceCreationThreshold = 0.5f;
    [Range(10, 100)]
    public int chunkSize = 50;

    public bool rngSeedFromTime = true;
    public int rngSeedValue = 0;
    [Range(1, 10)]
    public int randomLayerNumberFrom = 1;
    [Range(1, 10)]
    public int randomLayerNumberTo = 5;

    public Defines.ColorThreshold[] colorSteps;

    // Privates
    float[,] heightMap;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    public void Init()
    {
        guiTexture = new GuiTexture(texturePreview, colorSteps);
        guiMesh = new GuiMesh(meshPreview, colorSteps);
        guiVoxel = new GuiVoxel(voxelPreview, colorSteps, chunkSize);
        GenerateHeightMap();
    }

    public void GenerateHeightMap()
    {
        heightMap = MultilayerGeneration.Generate(layerParams.Take(layersToUse), mapSize, globalScale);
    }

    public void DrawTexture()
    {
        guiTexture.DrawTexture(heightMap);
    }

    public void GenerateMesh()
    {
        guiMesh.UpdateMesh(heightMap);
    }

    public void MakeRandomLayers()
    {
        System.Random rng;
        if (rngSeedFromTime)
            rng = new System.Random();
        else
            rng = new System.Random(rngSeedValue);

        layersToUse = rng.Next(randomLayerNumberFrom, randomLayerNumberTo);
        layerParams = MultilayerGeneration.GetRandomLayers(rng.Next(), 10);
        GenerateHeightMap();
        DrawTexture();
        GenerateMesh();
        DrawVoxelSpaceLimitedByHeightMapOptimized();
    }

    public int voxelNum = 10;

    public void DrawVoxelSpaceLimitedByHeightMap()
    {
        guiVoxel.UpdateMeshFrom3dBoundedByHeightMap(heightMap, mapSize, samplingRate, spaceCreationThreshold);
    }
    public void DrawVoxelSpaceLimitedByHeightMapOptimized()
    {
        // ToDo: There still seem to be some artifacts, will look into this when designing better
        // 3d generation functions
        System.Func<float, float, float, bool> spaceSampling =
            (x, y, z) => GeneratorMethods3d.Sines(x, y, z) > spaceCreationThreshold;
        guiVoxel.UpdateMeshFrom3dBoundedByHeightMapOptimized(heightMap, mapSize, samplingRate, spaceSampling);
    }
    public void TEST()
    {
        HashSet<Vector3> x = new HashSet<Vector3>();
        Vector3 a = new Vector3(1, 2, 3);
        Vector3 b = new Vector3(1, 2, 3);
        Vector3 c = new Vector3(1, 3, 3);

        Debug.Log(x.Count);
        x.Add(a);
        Debug.Log(x.Count);
        x.Add(b);
        Debug.Log(x.Count);
        x.Add(c);
        Debug.Log(x.Count);
    }
}
