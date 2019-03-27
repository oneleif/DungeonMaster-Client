using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLayer {
    public Texture2D texture;
    public float lineWidth;
    public float transparency;

    public DrawLayer() {
        texture = new Texture2D(1024, 1024);
        Color32[] resetColorsArray = new Color32[1024 * 1024];

        for (int x = 0; x < resetColorsArray.Length; x++)
            resetColorsArray[x] = new Color32(0, 0, 0, 0);
        texture.SetPixels32(resetColorsArray);
        lineWidth = 1;
        transparency = 1;
    }
}
