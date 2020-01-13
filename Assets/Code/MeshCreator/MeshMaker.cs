using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMaker : MonoBehaviour
{
	public static Mesh ConstructMeshFrom(Defines.ColorThreshold[] colorParams,
		float[,] heightMap, Defines.MeshParams meshParams)
	{
		var samples = GetHeightSamplesFromMap(heightMap, meshParams.samplingRate);
		var vertices = GetVertices(samples, meshParams.maxDepth, meshParams.vertSpacing);
		var triangles = GetTriangles(samples);

		// TODO : pass lambda to retrieve colors
		var colors = new Color[vertices.Length];
		for (int i = 0; i < colors.Length; i++)
			colors[i] = ColorPicker.GetColor(colorParams, samples[i % samples.GetLength(0), i / samples.GetLength(0)]);

		Mesh mesh = new Mesh
		{
			vertices = vertices,
			triangles = triangles.ToArray(),
			colors = colors
		};
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		return mesh;
	}

	public static float[,] GetHeightSamplesFromMap(float[,] map, int samplingRate)
	{
		int width = map.GetLength(0) / samplingRate;
		int height = map.GetLength(1) / samplingRate;
		float[,] result = new float[width, height];

		Debug.LogWarning("Map Size: " + width + " " + height);
		Debug.LogWarning("Size: " + width + " " + height);
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				Debug.Log(x + " " + y);
				result[x, y] = map[x*samplingRate, y*samplingRate];
			}
		}

		return result;
	}

	public static Vector3[] GetVertices(float[,] map, float maxDepth, float vertSpacing = 1f)
	{
		int width = map.GetLength(0);
		int height = map.GetLength(1);

		Vector3[] result = new Vector3[width * height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				result[width*height -1 - (x + y*width)] = new Vector3((float)x*vertSpacing, (float)y * vertSpacing, map[x, y] * maxDepth);
			}
		}

		return result;
	}

	public static List<int> GetTriangles(float[,] map)
	{
		int height = map.GetLength(0);
		int width = map.GetLength(1);

		List<int> result = new List<int>();

		for (int y = 0; y < height - 1; y++)
		{
			for (int x = 0; x < width - 1; x++)
			{
				// upper left triangle
				result.Add(x + y * width);
				result.Add(x + (y + 1) * width);
				result.Add((x + 1) + y * width);

				// lower right
				result.Add((x + 1) + y * width);
				result.Add(x + (y + 1) * width);
				result.Add((x + 1) + (y + 1) * width);
			}
		}
		return result;
	}
}
