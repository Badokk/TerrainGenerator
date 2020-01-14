using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultilayerGeneration
{
    public static float[,] Generate(IEnumerable<Defines.GeneratorLayer> layerParams, Defines.MapParams mapSize, Vector3 globalScale)
    {
        float[,] result = new float[mapSize.width, mapSize.height];
        foreach (Defines.GeneratorLayer layer in layerParams)
        {
            System.Func<float, float, float> generatorMethod = GeneratorMethods2d.ChooseFunc(layer);
            float[,] map = HeightMapAssembler.AssembleHeightMap(mapSize, generatorMethod, globalScale);

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

    public static Defines.GeneratorLayer GetRandomLayer(System.Random rng)
    {
        Defines.GeneratorLayer result = new Defines.GeneratorLayer {
            function = (GeneratingFunctionType2d)rng.Next(
                System.Enum.GetValues(typeof(GeneratingFunctionType2d)).Length),
            scale = rng.Next(5, 500),
            significance = (float)rng.NextDouble() % 1f,
            offset = new Vector2((float)rng.NextDouble(), (float)rng.NextDouble())
        };

        return result;
    }

    public static Defines.GeneratorLayer[] GetRandomLayers(int seed, int layerNum)
    {
        Defines.GeneratorLayer[] result = new Defines.GeneratorLayer[layerNum];
        var rng = new System.Random(seed);

        for (int i = 0; i < layerNum; i++)
        {
            result[i] = GetRandomLayer(rng);
        }

        return result;
    }
}