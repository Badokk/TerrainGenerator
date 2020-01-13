using UnityEngine;
using System.Collections.Generic;

public static class VoxelHandler
{
    /* TODO: This class is a mess. Need to refactor the code and the interfaces, and later optimize
     * mesh creation (probably gonna need to extract it)
     */

    private static Vector3 voxelSize = new Vector3(1,1,1);

    public static List<Vector3> GetCubeVertices(Vector3 midpoint)
    {
        List<Vector3> result = new List<Vector3>();
        float[] translations = new float[] { -0.5f, 0.5f };
        foreach (var x in translations)
            foreach (var y in translations)
                foreach (var z in translations)
                    result.Add(midpoint + new Vector3(voxelSize.x * x, voxelSize.y * y, voxelSize.z * z));
        return result;
    }

    public static List<List<int>> GetCubeFaces()
    {
        // All faces are defined clockwise, when looking at the face from outside
        return new List<List<int>>
        {
            new List<int> {0, 2, 3, 1},
            new List<int> {0, 4, 6, 2},
            new List<int> {0, 1, 5, 4},

            new List<int> {7, 6, 4, 5},
            new List<int> {7, 3, 2, 6},
            new List<int> {7, 5, 1, 3}
        };
    }

    public static List<List<int>> GetTrisFromSquare()
    {
        return new List<List<int>>
        {
            new List<int> {0, 2, 1},
            new List<int> {0, 3, 2 },
        };
    }

    public static List<int> GetTrisForCube()
    {
        List<int> result = new List<int>();
        foreach (var square in GetCubeFaces())
            foreach (var triangle in GetTrisFromSquare())
                foreach (var vertId in triangle)
                    result.Add(square[vertId]);
        return result;
    }

    public static Mesh CreateCubeMesh(List<Vector3> vertices, Color color)
    {
        var mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = GetTrisForCube().ToArray(),
            colors = new Color[] { color, color, color, color, color, color, color, color}
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    public static Mesh CreateMulticubeMesh(List<Vector3> vertices, List<Color> colors)
    {
        // TODO: Naive implementation, duplicating vertices and faces, also creating faces
        // that cannot be seen (faces inside a cluster, for example)

        var meshVertices = new List<Vector3>();
        var triangles = new List<int>();
        var vertColors = new List<Color>();

        for (var i = 0; i < vertices.Count; i++)
        {
            var singleMeshVertices = GetCubeVertices(vertices[i]);
            var singleTriangles = GetTrisForCube();
            var singleVertColors = new Color[] { colors[i], colors[i], colors[i], colors[i], colors[i], colors[i], colors[i], colors[i] };

            foreach (var v in singleMeshVertices)
                meshVertices.Add(v);

            foreach (var t in singleTriangles)
                triangles.Add(t + i * 8); // because there are 8 verts per cube

            foreach (var c in singleVertColors)
                vertColors.Add(c);
        }

        var mesh = new Mesh
        {
            vertices = meshVertices.ToArray(),
            triangles = triangles.ToArray(),
            colors = vertColors.ToArray()
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
