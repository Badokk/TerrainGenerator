using UnityEngine;
using System.Collections;
using System.Linq;

[ExecuteInEditMode]
public class GuiScript : MonoBehaviour
{
    public GameObject texturePreview;
    public bool textureAutoUpdate = false;
    public GameObject meshPreview;
    public bool meshAutoUpdate = false;
    public GameObject voxelPreview;

    public Defines.MapParams mapSize;
    [Range(0, 10)]
    public int layersToUse;
    public Defines.GeneratorLayer[] layerParams;

    public GuiTexture guiTexture;
    public GuiMesh guiMesh;
    public GuiVoxel guiVoxel;

    public bool rngSeedFromTime = true;
    public int rngSeedValue = 0;
    [Range(1, 10)]
    public int randomLayerNumberFrom = 1;
    [Range(1, 10)]
    public int randomLayerNumberTo = 5;

    public Defines.ColorThreshold[] colorSteps;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    public void Init()
    {
        guiTexture = new GuiTexture(texturePreview, colorSteps);
        guiMesh = new GuiMesh(meshPreview);
        guiVoxel = new GuiVoxel(voxelPreview);
    }

    public void DrawTexture()
    { 
        guiTexture.DrawTexture(layerParams.Take(layersToUse), mapSize);
    }

    public void GenerateMesh()
    {
        guiMesh.UpdateMesh(layerParams.Take(layersToUse), mapSize);
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
        DrawTexture();
        GenerateMesh();
        DrawPlaneOfVoxels();
    }

    public int voxelNum = 10;

    public void DrawVoxel()
    {
        guiVoxel.UpdateMesh();
    }

    public void DrawSomeRandomColoredVoxels()
    {
        guiVoxel.UpdateMeshWithManyCubesRandomColors(voxelNum);
    }

    public void DrawPlaneOfVoxels()
    {
        guiVoxel.UpdateMeshWithLayerOfCubesFromHeightMap(layerParams.Take(layersToUse), mapSize);
    }

    public void DrawVoxelSpace()
    {
        guiVoxel.UpdateMeshFrom3d(mapSize);
    }

    public void DrawVoxelSpaceLimitedByHeightMap()
    {
        guiVoxel.UpdateMeshFrom3dBoundedByHeightMap(layerParams.Take(layersToUse), mapSize);
    }
}
