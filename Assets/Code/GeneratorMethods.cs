using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GeneratingFunctionType
{
    Sines,
    DiagLines,
    Perlin,
    Triangles
};

public class GeneratorMethods
{
    // This is supposed to be a place for whatever random/pattern/noise generating functions
    public static float diagonalLines(float x, float y, float scale)
    {
        //Debug.Log(x + " " + y + " " + ((float)(x + y) % scale) / scale);
        return (float)((x+y) % scale) / scale;
    }

    public static float sines(float x, float y)
    {
        return (float)(System.Math.Sin(x) + System.Math.Sin(y)) / 4 + 0.5f;
    }

    public static float basicPerlin(float x, float y)
    {
        return Mathf.PerlinNoise(x, y);
    }

    public static float triangles(float x, float y)
    {
        return Mathf.Abs((x % 1 + y % 1));
    }

    public static System.Func<float, float, float> chooseFunc(GeneratingFunctionType input, float scale)
    {
        switch(input)
        {
        case GeneratingFunctionType.Sines:
            return (x, y) => GeneratorMethods.sines(x / scale, y / scale);
        case GeneratingFunctionType.DiagLines:
            return (x, y) => GeneratorMethods.diagonalLines(x, y, scale);
        case GeneratingFunctionType.Perlin:
            return (x, y) => GeneratorMethods.basicPerlin(x / scale, y / scale);
        case GeneratingFunctionType.Triangles:
            return (x, y) => GeneratorMethods.triangles(x / scale, y / scale);
        default:
            return (x, y) => 1 / (x / scale + y / scale);
        }
    }
}