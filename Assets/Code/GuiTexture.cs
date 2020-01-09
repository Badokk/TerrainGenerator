using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiTexture : MonoBehaviour
{
    // Public variables, to be manipulated in editor
    public bool autoUpdate = false;
    public int scale = 70;
    public Renderer textureRenderer;
    public GeneratingFunctionType generatingFunction;

    public GeneratorLayer[] layerParams;

    // Private variables
    Texture2D tex;
    int width = 500;
    int height = 500;

    /**Methods**/
    // Public methods
    public void DrawTexture()
    {
        makeTexture();
        DisplayTexture(tex);
    }
    public void DrawMultilayerTexture()
    {
        makeMultilayerTexture();
        DisplayTexture(tex);
    }

    // Builtin methods
    void Start()
    {
        DrawTexture();
    }

    // Private, helper methods
    public void DisplayTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
    }

    public void makeTexture()
    {
        System.Func<float, float, float> generator = GeneratorMethods.chooseFunc(generatingFunction, scale);
        float[,] heightMap = HeightMapAssembler.assembleHeightMap(new Vector2(width, height), generator);
        Texture2D texture = TextureGenerator.generate(heightMap);

        tex = texture;
    }

    public void makeMultilayerTexture()
    {
        float[,] heightMap = MultilayerGeneration.generate(layerParams, new Vector2(width, height));
        Texture2D texture = TextureGenerator.generate(heightMap);

        tex = texture;
    }
}
