using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public BackgroundLayer backgroundLayer;
    public DrawLayer drawLayer;
    public PlaceLayer placeLayer;

    public Map() {
        backgroundLayer = new BackgroundLayer();
        drawLayer = new DrawLayer();
        placeLayer = new PlaceLayer();
    }
}
