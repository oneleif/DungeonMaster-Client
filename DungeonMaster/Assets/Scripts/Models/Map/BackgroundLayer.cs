using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLayer
{
    public Sprite image;
    public Color color;
    public int rows;
    public int columns;

    public BackgroundLayer() {
        color = new Color(1, 1, 1);
        rows = 0;
        columns = 0;
    }
}
