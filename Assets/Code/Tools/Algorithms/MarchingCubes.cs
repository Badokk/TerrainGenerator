using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using V3 = UnityEngine.Vector3;
using V2 = UnityEngine.Vector2;

public class MarchingCubes
{ }

public class MarchingCubeFaces 
 {
    // this is not in fact immutable. ToDo: check if it's possible to make it so in a comprehensible way
    public static List<int> trisList = new List<int> { 1, 0, 2, 1, 2, 3 };

    static public List<V3> GetFaceVerts(V3 vertex)
    {
        if (vertex.x % 1 == 0.5f) 
            return new List<V3>{
                vertex + new V3(0,0.5f,0.5f), vertex + new V3(0,0.5f,-0.5f), vertex + new V3(0,-0.5f,0.5f), vertex + new V3(0,-0.5f,-0.5f)};
        else if (vertex.y % 1 == 0.5f)
            return new List<V3>{
                vertex + new V3(0.5f, 0, 0.5f), vertex + new V3(0.5f, 0, -0.5f), vertex + new V3(-0.5f, 0, 0.5f), vertex + new V3(-0.5f, 0, -0.5f)};
        else
            return new List<V3>{
                vertex + new V3(0.5f,0.5f, 0), vertex + new V3(0.5f,-0.5f, 0), vertex + new V3(-0.5f,0.5f, 0), vertex + new V3(-0.5f,-0.5f, 0)};
    }

    public static V3 GetFaceMidpoint(V3 cube1Midpoint, V3 cube2Midpoint)
    {
        return cube1Midpoint + (cube2Midpoint - cube1Midpoint) / 2;
    }

    public static List<V3> GetFacesToDraw(System.Func<float, float, float, bool> spaceProbeFunc, float[,] heightMap, V3 samplingRate, Defines.MapParams mapSize)
    {
        // Todo: Consider refactoring samplingRate into mapSize
        Queue<V3> pointsToHandle = new Queue<V3>();
        HashSet<V3> pointsHandled = new HashSet<V3>();
        List<V3> result = new List<V3>();

        V3 startingPoint = FindStartingPoint(spaceProbeFunc, heightMap, samplingRate, mapSize);
        pointsToHandle.Enqueue(startingPoint);

        while (pointsToHandle.Count > 0)
        {
            var currentPoint = pointsToHandle.Dequeue();
            pointsHandled.Add(currentPoint);

            foreach (var neighbor in NeumannNeighborhood3D(currentPoint))
            {
                var inSpace = spaceProbeFunc(
                    neighbor.x * samplingRate.x,
                    neighbor.y * samplingRate.y,
                    neighbor.z * samplingRate.z);
                var isThisInBounds = InBounds(neighbor, mapSize, samplingRate);

                // we know at this point that currentPoint is solid, so we only create faces
                // between it and its non-solid neighbors
                if (!inSpace && isThisInBounds)
                {
                    var face = GetFaceMidpoint(currentPoint, neighbor);
                    result.Add(face);
                    foreach (var n in GetPositionsNeighboringFace(face))
                    {
                        var isInBounds = InBounds(n, mapSize, samplingRate);
                        var belowGround = isInBounds && (
                            heightMap[
                                (int)(n.x * samplingRate.x),
                                (int)(n.y * samplingRate.y)] * mapSize.depth > n.z);
                        var wasAlreadyHandled = pointsHandled.Contains(n);
                        var spaceCreationAllowsIt = spaceProbeFunc(
                            (n.x) * samplingRate.x,
                            (n.y) * samplingRate.y,
                            (n.z) * samplingRate.z);

                        // ToDo: this could be optimized, I think, by looking at the output of all faces
                        // and removing ones that cannot have neighboring face with the cube
                        if (belowGround && !wasAlreadyHandled && spaceCreationAllowsIt)
                        {
                            pointsToHandle.Enqueue(n);
                            pointsHandled.Add(n);
                        }
                    }
                }
            }
        }

        return result;
    }

    public static bool InBounds(V3 point, Defines.MapParams bounds, V3 sampling)
    {
        return point.x >= bounds.offset.x && point.y >= bounds.offset.y &&
            point.x < (bounds.width + bounds.offset.x) / sampling.x &&
            point.y < (bounds.height + bounds.offset.y) / sampling.y;
    }

    public static V3 FindStartingPoint(System.Func<float, float, float, bool> spaceProbeFunc, float[,] heightMap, V3 samplingRate, Defines.MapParams mapSize)
    {
        // Todo: find a sensible null check for C#
        // Todo: this looks from the middle of the chunk and iterates over 1/4 of its area
        //   This will cause issues when there are holes in the map.
        //   Might need to look for a more sophisticated way.
        int x = (int)(mapSize.width)/(int)samplingRate.x/2 + (int)mapSize.offset.x;
        while (x < mapSize.width + mapSize.offset.x)
        {
            int y = (int)(mapSize.height) / (int)samplingRate.y/2 + (int)mapSize.offset.y;
            while (y < mapSize.height + mapSize.offset.y)
            {
                int z = (int)(heightMap[x, y] * mapSize.depth) +1;
                while (z > 0)
                {
                    var isInSpace = spaceProbeFunc(
                            x*samplingRate.x,
                            y*samplingRate.y,
                            z*samplingRate.z);
                    var isBelowGround = heightMap[
                            (int)(x*samplingRate.x),
                            (int)(y*samplingRate.y)] * mapSize.depth > z*samplingRate.z;
                    if (isInSpace && isBelowGround)
                    {
                        return new V3(x, y, z);
                    }
                    z -= (int)samplingRate.z;
                }
                y+=(int)samplingRate.y;
            }
            x+=(int)samplingRate.x;
        }
        Debug.LogWarning("Not a single vertex is of use. Too bad.");
        return new V3(0, 0, 0);
    }

    public static List<V3> GetPositionsNeighboringFace(V3 faceMidpoint)
    {
        // Todo: I wonder if this could be made more generic (to not repeat code for x, y, x)
        char keyAxis;
        if (faceMidpoint.x % 1 == 0.5f) keyAxis = 'x';
        else if (faceMidpoint.y % 1 == 0.5f) keyAxis = 'y';
        else keyAxis = 'z';

        var result = new List<V3>();
        V2 square;
        if (keyAxis == 'x')
        {
            square = new V2(faceMidpoint.y, faceMidpoint.z);
        }
        else if (keyAxis == 'y')
        {
            square = new V2(faceMidpoint.x, faceMidpoint.z);
        }
        else
        {
            square = new V2(faceMidpoint.x, faceMidpoint.y);
        }

        var neighbors = NeumannNeighborhood2D(square);

        foreach (var neighbor in neighbors)
        {
            if (keyAxis == 'x')
            {
                result.Add(new V3(Mathf.Floor(faceMidpoint.x), neighbor.x, neighbor.y));
                result.Add(new V3(Mathf.Floor(faceMidpoint.x) + 1, neighbor.x, neighbor.y));
            }
            else if (keyAxis == 'y')
            {
                result.Add(new V3(neighbor.x, Mathf.Floor(faceMidpoint.y), neighbor.y));
                result.Add(new V3(neighbor.x, Mathf.Floor(faceMidpoint.y) + 1, neighbor.y));
            }
            else
            {
                result.Add(new V3(neighbor.x, neighbor.y, Mathf.Floor(faceMidpoint.z)));
                result.Add(new V3(neighbor.x, neighbor.y, Mathf.Floor(faceMidpoint.z) + 1));
            }
        }

        return result;
    }

    static V2[] neumannNeighborhood2D = new V2[]
    {
        new V2(1,0), new V2(-1,0), new V2(0,1), new V2(0,-1)
    };
    public static List<V2> NeumannNeighborhood2D(V2 vertex)
    {
        List<V2> result = new List<V2>();
        foreach (var neighbor in neumannNeighborhood2D)
            result.Add(vertex + neighbor);
        return result;
    }

    static V3[] neumannNeighborhood3D = new V3[]
    {
        new V3(1,0,0), new V3(-1,0,0), new V3(0,1,0), new V3(0,-1,0), new V3(0,0,1), new V3(0,0,-1)
    };

    public static List<V3> NeumannNeighborhood3D(V3 vertex)
    {
        List<V3> result = new List<V3>();
        foreach (var neighbor in neumannNeighborhood3D)
            result.Add(vertex + neighbor);
        return result;
    }

}
