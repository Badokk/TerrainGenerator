using UnityEngine;
using System.Collections;

public enum GeneratingFunctionType3d
{
    Sines,
    DiagLines,
    Perlin,
    Squares
};

public class GeneratorMethods3d
{
    public static float DiagonalLines(float x, float y, float z, float scale)
    {
        return (float)((x + y + z) % scale) / scale;
    }

    public static float Sines(float x, float y, float z)
    {
        return (float)(System.Math.Sin(x) + System.Math.Sin(y) + System.Math.Sin(z)) / 6 + 0.5f;
    }
}
