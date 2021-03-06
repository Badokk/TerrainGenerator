﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator
{
    public static Texture2D Generate(Defines.ColorThreshold[] colorParams, float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Texture2D result = new Texture2D(width, height);
        Color[] colorMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = ColorPicker.GetColor(colorParams, heightMap[x, y]);
            }
        }
        
        result.SetPixels(colorMap);
        result.Apply();
        return result;
    }
}