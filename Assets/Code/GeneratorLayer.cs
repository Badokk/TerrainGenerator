using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GeneratorLayer
{
    public GeneratingFunctionType function;
    public float scale;
    [Range(0, 1)]
    public float significance;
    public Vector2 offset;
}

public class MultilayerGeneration
{
    public static float[,] generate(GeneratorLayer[] layerParams, Vector2 mapSize)
    {
        float[,] result = new float[(int)mapSize.x, (int)mapSize.y];
        foreach (GeneratorLayer layer in layerParams)
        {
            System.Func<float, float, float> generatorMethod = GeneratorMethods.chooseFunc(layer.function, layer.scale);
            float[,] map = HeightMapAssembler.assembleHeightMap(mapSize, generatorMethod);

            // prone to change when I find an elegant way of doing this (Zip?)
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(0); j++)
                    result[i, j] += map[i,j] * layer.significance;
        }

        normalize(result);
        return result;
    }

    public static void normalize(float[,] map)
    {
        float maxValue = float.MinValue;
        float minValue = float.MaxValue;
        // prone to change when I find an elegant way of doing this (Linq? flattenning the array? Is it cheap enough?)
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(0); j++)
            {
                maxValue = Mathf.Max(maxValue, map[i, j]);
                minValue = Mathf.Min(minValue, map[i, j]);
            }

        maxValue -= minValue;

        // prone to change when I find an elegant way of doing this
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(0); j++)
                map[i, j] = (map[i, j] - minValue) / maxValue;
    }


}