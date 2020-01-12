using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultilayerGeneration
{
    public static float[,] Generate(IEnumerable<Defines.GeneratorLayer> layerParams, Defines.MapParams mapSize)
    {
        float[,] result = new float[mapSize.width, mapSize.height];
        foreach (Defines.GeneratorLayer layer in layerParams)
        {
            System.Func<float, float, float> generatorMethod = GeneratorMethods.ChooseFunc(layer.function, layer.scale);
            float[,] map = HeightMapAssembler.AssembleHeightMap(mapSize, generatorMethod);

            // prone to change when I find an elegant way of doing this (Zip?)
            for (int i = 0; i < mapSize.width; i++)
                for (int j = 0; j < mapSize.height; j++)
                    result[i, j] += map[i,j] * layer.significance;
        }

        Normalize(result);
        return result;
    }

    public static void Normalize(float[,] map)
    {
        float maxValue = float.MinValue;
        float minValue = float.MaxValue;
        // prone to change when I find an elegant way of doing this (Linq? flattenning the array? Is it cheap enough?)
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
            {
                maxValue = Mathf.Max(maxValue, map[i, j]);
                minValue = Mathf.Min(minValue, map[i, j]);
            }

        maxValue -= minValue;

        // prone to change when I find an elegant way of doing this
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
                map[i, j] = (map[i, j] - minValue) / maxValue;
    }


}