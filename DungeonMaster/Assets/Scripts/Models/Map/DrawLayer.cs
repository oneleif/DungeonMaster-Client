using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLayer {
    public Texture2D texture;
    public float lineWidth;
    public float transparency;

    public DrawLayer() {
        texture = new Texture2D(1024, 1024);
        lineWidth = 1;
        transparency = 1;
    }
}
