using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLayer
{
    public Texture2D redTexture;
    public Texture2D greenTexture;
    public Texture2D blueTexture;
    public float lineWidth;
    public float transparency;

    public DrawLayer() {
        redTexture = new Texture2D(1024, 1024);
        greenTexture = new Texture2D(1024, 1024);
        blueTexture = new Texture2D(1024, 1024);
        lineWidth = 1;
        transparency = 1;
    }
}
