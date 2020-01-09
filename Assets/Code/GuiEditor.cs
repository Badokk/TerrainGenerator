using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GuiTexture))]
public class MapGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        GuiTexture guiTexture = (GuiTexture)target;

        if (DrawDefaultInspector())
        {
            if (guiTexture.autoUpdate)
            {
                guiTexture.DrawTexture();
            }
        }

        if (GUILayout.Button("Generate the map"))
        {
            guiTexture.DrawTexture();
        }

        if (GUILayout.Button("Generate from multilayer"))
        {
            guiTexture.DrawMultilayerTexture();
        }
    }
}