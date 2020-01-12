using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GeneratingFunctionType
{
    Sines,
    DiagLines,
    Perlin,
    Squares
};

public class GeneratorMethods
{
    // This is supposed to be a place for whatever random/pattern/noise generating functions
    public static float DiagonalLines(float x, float y, float scale)
    {
        return (float)((x+y) % scale) / scale;
    }

    public static float Sines(float x, float y)
    {
        return (float)(System.Math.Sin(x) + System.Math.Sin(y)) / 4 + 0.5f;
    }

    public static float BasicPerlin(float x, float y)
    {
        return Mathf.PerlinNoise(x, y);
    }

    public static float Squares(float x, float y)
    {
        return Mathf.Abs((x % 1 + y % 1));
    }

    public static System.Func<float, float, float> ChooseFunc(Defines.GeneratorLayer layerParams)
    {
        var scale = layerParams.scale;
        var offset = layerParams.offset;
        var funcType = layerParams.function;
        switch (funcType)
        {
        case GeneratingFunctionType.Sines:
            return (x, y) => GeneratorMethods.Sines(
                (x+offset.x) / scale, (y + offset.y) / scale);
        case GeneratingFunctionType.DiagLines:
            return (x, y) => GeneratorMethods.DiagonalLines(
                (x + offset.x), (y + offset.y), scale);
        case GeneratingFunctionType.Perlin:
            return (x, y) => GeneratorMethods.BasicPerlin(
                (x + offset.x) / scale, (y + offset.y) / scale);
        case GeneratingFunctionType.Squares:
            return (x, y) => GeneratorMethods.Squares(
                (x + offset.x) / scale, (y + offset.y) / scale);
        default:
            return (x, y) => 1 / ((x + offset.x) / scale + (y + offset.y) / scale);
        }
    }
}