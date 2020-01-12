using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapAssembler
{
    public static float[,] AssembleHeightMap(Defines.MapParams mapSize, System.Func<float, float, float> function)
    {
        float[,] map = new float[mapSize.width, mapSize.height];


        for (int y = 0; y < mapSize.height; y++)
        {
            for (int x = 0; x < mapSize.width; x++)
            {
                map[x, y] = function(x, y);
            }
        }

        return map;
    }
}
