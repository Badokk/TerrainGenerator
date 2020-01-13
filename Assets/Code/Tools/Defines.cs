using UnityEngine;
using System.Collections;

public static class Defines
{
	[System.Serializable]
	public class MapParams
	{
		[Range(0, 1000)]
		public int width = 500;
		[Range(0, 1000)]
		public int height = 500;
		[Range(0, 100)]
		public int depth = 10;

		public MapParams(int w, int h)
		{
			width = Mathf.Max(0, Mathf.Min(1000, w));
			height = Mathf.Max(0, Mathf.Min(1000, h));
		}
		public static MapParams operator /(MapParams mp1, int i)
		{
			mp1.width /= i;
			mp1.height /= i;
			return mp1;
		}
	}

	[System.Serializable]
	public class MeshParams
	{
		[Range(1, 1000)]
		public int samplingRate = 50;
		[Range(1, 20)]
		public float maxDepth = 1;
		[Range(0.5f, 10f)]
		public float vertSpacing = 1;

		public MeshParams(int sr, float md, float vs)
		{
			samplingRate = Mathf.Max(1, Mathf.Min(1000, sr));
			maxDepth = Mathf.Max(1, Mathf.Min(20, md));
			vertSpacing = Mathf.Max(0.5f, Mathf.Min(10f, vs));
		}
	}

	[System.Serializable]
	public class GeneratorLayer
	{
		public GeneratingFunctionType2d function;
		public float scale;
		[Range(0, 1)]
		public float significance;
		public Vector2 offset;
	}

	public static int UnityUnitsToPixelRatio = 100;
	public static Material baseMapMaterial = Resources.Load<Material>("Materials/BaseMapMaterial");

	[System.Serializable]
	public class ColorThreshold
	{
		[Range(0, 1)]
		public float heightFrom;
		public Color color;
	}
}

