using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInspectorViewModel : MonoBehaviour {
    // PEN COLOUR
    public static Color penColor = Color.red;
    // PEN WIDTH (actually, it's a radius, in pixels)
    public static int penWidth = 3;

    public void SetDrawColor(Color color) {
        penColor = color;
    }

    public void SetDrawLineWidth(float lineWidth) {
        penWidth = (int)lineWidth;
    }

    public float GetDrawLineWidth() {
        return penWidth;
    }

    public void SetDrawTransparency(float transparency) {
        Color c = penColor;
        c.a = transparency;
        penColor = c;
    }

    public float GetDrawTransparency() {
        return penColor.a;
    }
}
