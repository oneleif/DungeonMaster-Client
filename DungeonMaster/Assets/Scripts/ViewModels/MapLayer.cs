using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayer
{
    public string layerName;
    public List<MapLayer> children;

    public MapLayer(string name) {
        this.layerName = name;
        children = new List<MapLayer>();
    }
}
