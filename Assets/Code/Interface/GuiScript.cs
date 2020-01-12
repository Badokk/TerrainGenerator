﻿using UnityEngine;
using System.Collections;
using System.Linq;

[ExecuteInEditMode]
public class GuiScript : MonoBehaviour
{
    public GameObject texturePreview;
    public bool textureAutoUpdate = false;
    public GameObject meshPreview;
    public bool meshAutoUpdate = false;

    public Defines.MapParams mapSize;
    [Range(0, 10)]
    public int layersToUse;
    public Defines.GeneratorLayer[] layerParams;

    public GuiTexture guiTexture;
    public GuiMesh guiMesh;

    public bool rngSeedFromTime = true;
    public int rngSeedValue = 0;
    [Range(1, 10)]
    public int randomLayerNumberFrom = 1;
    [Range(1, 10)]
    public int randomLayerNumberTo = 5;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    public void Init()
    {
        guiTexture = new GuiTexture(texturePreview);
        guiMesh = new GuiMesh(meshPreview);
    }

    public void DrawTexture()
    { 
        if (guiTexture == null) guiTexture = new GuiTexture(texturePreview);
        guiTexture.DrawTexture(layerParams.Take(layersToUse), mapSize);
    }

    public void GenerateMesh()
    {
        if (guiMesh == null) guiMesh = new GuiMesh(meshPreview);
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
    }
}
