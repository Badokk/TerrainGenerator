using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GuiVoxel
{
	readonly GameObject voxelDisplay;
	readonly Defines.ColorThreshold[] colorParams;
	List<GameObject> chunkDisplay = new List<GameObject>();
	MeshRenderer renderer;
	MeshFilter filter;
	int chunkSize;

	public GuiVoxel(GameObject display, Defines.ColorThreshold[] colors, int chunkSize)
	{
		voxelDisplay = display;
		colorParams = colors;
		this.chunkSize = chunkSize;
		Init();
	}

	void Init()
	{
		if (voxelDisplay.GetComponent<MeshRenderer>() == null)
			renderer = voxelDisplay.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		if (voxelDisplay.GetComponent<MeshFilter>() == null)
			filter = voxelDisplay.AddComponent(typeof(MeshFilter)) as MeshFilter;

		renderer = voxelDisplay.GetComponent<MeshRenderer>();
		filter = voxelDisplay.GetComponent<MeshFilter>();

		renderer.material = Defines.baseMapMaterial;
	}

	public void UpdateMeshFrom3dBoundedByHeightMap(
		float[,] heightMap, Defines.MapParams mapSize,
		Vector3 samplingRate, float spaceCreationThreshold)
	{
		var spaceMap = HeightMapAssembler.AssembleSpaceMap(mapSize);
		var vertices = new List<Vector3>();
		var colors = new List<Color>();

		for (int x = 0; x < mapSize.width; x += (int)samplingRate.x)
		{
			for (int y = 0; y < mapSize.height; y += (int)samplingRate.y)
			{
				for (int z = 0; z < mapSize.depth; z+= (int)samplingRate.z)
				{
					//Debug.Log("Vertex " + x + ", " + y + ", " + z + " = " + spaceMap[x, y, z]);
					if (heightMap[x,y]*mapSize.depth > z && spaceMap[x, y, z] > spaceCreationThreshold)
					{
						vertices.Add(new Vector3(x / (int)samplingRate.x, y / (int)samplingRate.y, z));
						colors.Add(ColorPicker.GetColor(colorParams, heightMap[x, y]));
					}
				}
			}
		}
		var mesh = VoxelHandler.CreateMulticubeMesh(vertices, colors);

		filter.mesh = mesh;
	}

	public void UpdateMeshFrom3dBoundedByHeightMapOptimized(
		float[,] heightMap, Defines.MapParams mapSize,
		Vector3 samplingRate, System.Func<float,float,float,bool> spaceProbingFunction)
	{
		cleanupChunks();

		// ToDo: Could pass height generating function instead of height map to
		// free ourselves from array.Length() sanity checks and solve potential rounding problems
		System.Func<float, float, float, bool> spaceProbingFunc =
			(x, y, z) => z == 0 ||
				(spaceProbingFunction(x, y, z) &&
				x >= 0 &&
				y >= 0 &&
				z >= 0 &&
				x < heightMap.GetLength(0) &&
				y < heightMap.GetLength(1) &&
				z < heightMap[(int)x, (int)y] * mapSize.depth);

		int xBot = 0;
		while (xBot < mapSize.width)
		{
			int xTop = Mathf.Min(mapSize.width, xBot + chunkSize);
			int yBot = 0;
			while (yBot < mapSize.height)
			{
				int yTop = Mathf.Min(mapSize.height, yBot + chunkSize);
				var chunkMapSize = new Defines.MapParams(
					new Vector3(xTop - xBot, yTop - yBot, mapSize.depth),
					new Vector3(xBot, yBot, mapSize.offset.z));

				GameObject newChunk = prepareNewChunk();

				var chunkMesh = VoxelHandler.CreateMulticubeMeshOptimized(heightMap, spaceProbingFunc, samplingRate, chunkMapSize, colorParams);
				chunkMesh.Optimize();
				newChunk.GetComponent<MeshFilter>().mesh = chunkMesh;
				yBot += chunkSize;
			}
			xBot += chunkSize;
		}
	}

	void cleanupChunks()
	{
		while (true)
		{
			var child = voxelDisplay.transform.Find("chunk");
			if (!child)
			{
				break;
			}
			GameObject.DestroyImmediate(child.gameObject);
		}
		foreach (GameObject chunk in chunkDisplay)
			GameObject.DestroyImmediate(chunk);
		chunkDisplay = new List<GameObject>();
	}

	GameObject prepareNewChunk()
	{
		GameObject result = new GameObject("chunk");
		result.transform.SetParent(voxelDisplay.transform);
		result.transform.position = new Vector3(0, 0, 0);
		result.transform.rotation = new Quaternion();
		result.transform.localScale = new Vector3(1, 1, 1);

		var chunkRenderer = result.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		var chunkFilter = result.AddComponent(typeof(MeshFilter)) as MeshFilter;
		chunkRenderer.material = Defines.baseMapMaterial;

		return result;
	}

}
