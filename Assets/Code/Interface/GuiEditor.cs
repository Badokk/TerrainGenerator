using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GuiScript))]
public class MapGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        GuiScript guiScript = (GuiScript)target;

        if (DrawDefaultInspector())
        {
            if (guiScript.heightMapAutoUpdate)
            {
                guiScript.GenerateHeightMap();
            }
            if (guiScript.textureAutoUpdate)
            {
                guiScript.DrawTexture();
            }
            if (guiScript.meshAutoUpdate)
            {
                guiScript.GenerateMesh();
            }
        }

        if (GUILayout.Button("Initialize stuff"))
        {
            guiScript.Init();
        }

        if (GUILayout.Button("TEST"))
        {
            guiScript.TEST();
        }

        if (GUILayout.Button("Regenerate height map"))
        {
            guiScript.GenerateHeightMap();
        }

        if (GUILayout.Button("Generate layer params"))
        {
            guiScript.MakeRandomLayers();
        }

        if (GUILayout.Button("Draw the map"))
        {
            guiScript.DrawTexture();
        }

        if (GUILayout.Button("Draw the mesh"))
        {
            guiScript.GenerateMesh();
        }

        if (GUILayout.Button("Generate cube space (WARNING: EXPENSIVE)"))
        {
            guiScript.DrawVoxelSpaceLimitedByHeightMap();
        }
        if (GUILayout.Button("Generate cube-based space"))
        {
            guiScript.DrawVoxelSpaceLimitedByHeightMapOptimized();
        }
    }
}