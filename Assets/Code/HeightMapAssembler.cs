using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapAssembler
{
    public static float[,] assembleHeightMap(Vector2 mapSize, System.Func<float, float, float> function)
    {
        float[,] map = new float[(int)mapSize.x, (int)mapSize.y];


        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                map[x, y] = function(x, y);
            }
        }

        return map;
    }
}
