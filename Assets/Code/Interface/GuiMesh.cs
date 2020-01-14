using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GuiMesh
{
	public Defines.MeshParams meshParams;

	readonly GameObject meshDisplay;
	readonly Defines.ColorThreshold[] colorSteps;
	MeshRenderer renderer;
	MeshFilter filter;

	public GuiMesh(GameObject display, Defines.ColorThreshold[] colorSteps)
	{
		this.colorSteps = colorSteps;
		meshParams = new Defines.MeshParams(10, 1, 1);
		meshDisplay = display;
		Init();
	}

	void Init()
	{
		if (meshDisplay.GetComponent<MeshRenderer>() == null)
			renderer = meshDisplay.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		if (meshDisplay.GetComponent<MeshFilter>() == null)
			filter = meshDisplay.AddComponent(typeof(MeshFilter)) as MeshFilter;

		renderer = meshDisplay.GetComponent<MeshRenderer>();
		filter = meshDisplay.GetComponent<MeshFilter>();

		renderer.material = Defines.baseMapMaterial;
	}

	public void UpdateMesh(float[,] heightMap)
	{
		Init();
		meshDisplay.transform.position = new Vector3(
			heightMap.GetLength(0) * meshParams.vertSpacing / Defines.UnityUnitsToPixelRatio,
			heightMap.GetLength(1) * meshParams.vertSpacing / Defines.UnityUnitsToPixelRatio,
			0);

		filter.mesh = MeshMaker.ConstructMeshFrom(colorSteps, heightMap, meshParams);
	}
}
