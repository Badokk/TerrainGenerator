using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapAssembler
{
    public static float[,] AssembleHeightMap(
        Defines.MapParams mapSize, System.Func<float, float, float> function, Vector3 globalScale)
    {
        float[,] map = new float[mapSize.width, mapSize.height];


        for (int y = 0; y < mapSize.height; y++)
        {
            for (int x = 0; x < mapSize.width; x++)
            {
                map[x, y] = function(x * globalScale.x, y * globalScale.y);
            }
        }

        return map;
    }

    // this thing needs to be streamlined, same way 2d height map is being created
    public static float[,,] AssembleSpaceMap(
        Defines.MapParams mapSize)
    {
        float[,,] space = new float[mapSize.width, mapSize.height, mapSize.depth];

        for (int z = 0; z < mapSize.depth; z++)
        {
            for (int y = 0; y < mapSize.height; y++)
            {
                for (int x = 0; x < mapSize.width; x++)
                {
                    space[x, y, z] = GeneratorMethods3d.Sines(x, y, z);
                }
            }
        }

        return space;
    }
}
