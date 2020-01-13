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

        if (GUILayout.Button("Generate the map"))
        {
            guiScript.DrawTexture();
        }

        if (GUILayout.Button("Update mesh"))
        {
            guiScript.GenerateMesh();
        }

        if (GUILayout.Button("Generate layer params"))
        {
            guiScript.MakeRandomLayers();
        }

        if (GUILayout.Button("LET THERE BE CUBE"))
        {
            guiScript.DrawVoxel();
        }

        if (GUILayout.Button("LET THERE BE CUBES"))
        {
            guiScript.DrawSomeRandomColoredVoxels();
        }

        if (GUILayout.Button("LET THERE BE A PLANE OF CUBES"))
        {
            guiScript.DrawPlaneOfVoxels();
        }

        if (GUILayout.Button("LET THERE BE A SPAAACE OF CUBES"))
        {
            guiScript.DrawVoxelSpace();
        }

        if (GUILayout.Button("LET THERE BE A WORLD OF CUBES"))
        {
            guiScript.DrawVoxelSpaceLimitedByHeightMap();
        }
    }
}