using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuiTexture
{
	public bool showSeparateLayers = false;

	readonly GameObject textureDisplay;
	readonly Defines.ColorThreshold[] colorSteps;

	public GuiTexture(GameObject display, Defines.ColorThreshold[] colorSteps)
	{
		this.colorSteps = colorSteps;
		textureDisplay = display;
		Init();
	}

	/**Methods**/
	// Public methods
	public void DrawTexture(float[,] heightMap)
	{
		Texture2D texture = MakeTexture(heightMap);
		DisplayTexture(texture);
	}

	// Private, helper methods
	void Init()
	{
		if (textureDisplay.transform.Find("MainCanvas") == null)
		{
			GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
			plane.name = "MainCanvas";
			plane.transform.SetParent(textureDisplay.transform);
		}
	}

	public void DisplayTexture(Texture2D texture)
	{
		Renderer canvas = textureDisplay.transform.Find("MainCanvas").gameObject.GetComponent<Renderer>();
		canvas.sharedMaterial.mainTexture = texture;
		canvas.transform.localScale = new Vector3(
			(float)texture.width / Defines.UnityUnitsToPixelRatio,
			1,
			(float)texture.height / Defines.UnityUnitsToPixelRatio);
		textureDisplay.transform.position = new Vector3(
			texture.width * -5f / Defines.UnityUnitsToPixelRatio,
			texture.height * 5f/ Defines.UnityUnitsToPixelRatio,
			0);
	}

	public Texture2D MakeTexture(float[,] heightMap)
	{
		return TextureGenerator.Generate(colorSteps, heightMap);
	}
}
