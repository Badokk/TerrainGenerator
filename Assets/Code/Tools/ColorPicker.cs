using UnityEngine;

public class ColorPicker
{
    public static Color GetColor(Defines.ColorThreshold[] colorParams, float height)
    {
        foreach(var color in colorParams)
        {
            if(height > color.heightFrom & height <= color.heightTo)
            {
                return color.color;
            }
        }
        return Color.black;
    }
}