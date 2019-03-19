using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceLayer
{
    public Vector2 gridPosition;
    public List<Placeable> placeables;

    public PlaceLayer() {
        gridPosition = new Vector2(0, 0);
        placeables = new List<Placeable>();
    }
}
