using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GuiVoxel
{
	readonly GameObject voxelDisplay;
	readonly Defines.ColorThreshold[] colorParams;
	MeshRenderer renderer;
	MeshFilter filter;

	[Range(0,1)]
	public float spaceCreationThreshold = 0.5f;

	public GuiVoxel(GameObject display, Defines.ColorThreshold[] colors)
	{
		voxelDisplay = display;
		colorParams = colors;
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
	public void UpdateMesh()
	{
		Init();

		var mesh = VoxelHandler.CreateCubeMesh(VoxelHandler.GetCubeVertices(new Vector3(0,0,0)), Color.green);
		filter.mesh = mesh;
	}

	public void UpdateMeshWithManyCubesRandomColors(int voxelNum)
	{
		Init();;

		var vertices = new List<Vector3>();
		var colors = new List<Color>();
		for (int i = 0; i < voxelNum; i++)
		{
			vertices.Add(new Vector3(0, i, 0));
			colors.Add(Random.ColorHSV());
		}

		var mesh = VoxelHandler.CreateMulticubeMesh(vertices, colors);
		filter.mesh = mesh;
	}

	public void UpdateMeshWithLayerOfCubesFromHeightMap(float[,] heightMap, Defines.MapParams mapSize)
	{
		var vertices = new List<Vector3>();
		var colors = new List<Color>();

		for (int x = 0; x < mapSize.width; x += 50)
		{
			for (int y = 0; y < mapSize.height; y += 50)
			{
				vertices.Add(new Vector3(x / 50, y / 50, heightMap[x, y]));
				colors.Add(Color.Lerp(Color.blue, Color.white, heightMap[x, y]));
			}
		}
		var mesh = VoxelHandler.CreateMulticubeMesh(vertices, colors);

		filter.mesh = mesh;
	}

	public void UpdateMeshFrom3d(Defines.MapParams mapSize)
	{
		var spaceMap = HeightMapAssembler.AssembleSpaceMap(mapSize);
		var vertices = new List<Vector3>();
		var colors = new List<Color>();

		for (int x = 0; x < mapSize.width; x += 50)
		{
			for (int y = 0; y < mapSize.height; y += 50)
			{
				for (int z = 0; z < mapSize.depth; z++)
				{
					//Debug.Log("Vertex " + x + ", " + y + ", " + z + " = " + spaceMap[x, y, z]);
					if (spaceMap[x, y, z] > spaceCreationThreshold)
					{
						vertices.Add(new Vector3(x / 50, y / 50, z));
						colors.Add(Color.Lerp(Color.blue, Color.white, spaceMap[x, y, z]));
					}
				}
			}
		}
		var mesh = VoxelHandler.CreateMulticubeMesh(vertices, colors);

		filter.mesh = mesh;
	}
	public void UpdateMeshFrom3dBoundedByHeightMap(float[,] heightMap, Defines.MapParams mapSize)
	{
		var spaceMap = HeightMapAssembler.AssembleSpaceMap(mapSize);
		var vertices = new List<Vector3>();
		var colors = new List<Color>();

		for (int x = 0; x < mapSize.width; x += 50)
		{
			for (int y = 0; y < mapSize.height; y += 50)
			{
				for (int z = 0; z < mapSize.depth; z++)
				{
					//Debug.Log("Vertex " + x + ", " + y + ", " + z + " = " + spaceMap[x, y, z]);
					if (heightMap[x,y]*mapSize.depth > z && spaceMap[x, y, z] > spaceCreationThreshold)
					{
						vertices.Add(new Vector3(x / 50, y / 50, z));
						colors.Add(ColorPicker.GetColor(colorParams, heightMap[x, y]));
					}
				}
			}
		}
		var mesh = VoxelHandler.CreateMulticubeMesh(vertices, colors);

		filter.mesh = mesh;
	}


}
