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
    }
}