using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GuiMesh
{
	readonly GameObject meshDisplay;
	MeshRenderer renderer;
	MeshFilter filter;

	public Defines.MeshParams meshParams;

	public GuiMesh(GameObject display)
	{
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

	public void UpdateMesh(Defines.ColorThreshold[] colorSteps,
	IEnumerable<Defines.GeneratorLayer> generatorLayers,
		Defines.MapParams mapSize)
	{
		Init();
		meshDisplay.transform.position = new Vector3(
			mapSize.width * meshParams.vertSpacing / Defines.UnityUnitsToPixelRatio,
			mapSize.height * meshParams.vertSpacing / Defines.UnityUnitsToPixelRatio,
			0);

		var heightMap = MultilayerGeneration.Generate(generatorLayers, mapSize);

		filter.mesh = MeshMaker.ConstructMeshFrom(colorSteps, heightMap, meshParams);
	}
}
