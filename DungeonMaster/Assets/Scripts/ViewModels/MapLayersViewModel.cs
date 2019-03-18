using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayersViewModel
{
    MapLayer[] mapLayers;

    public MapLayersViewModel() {
        mapLayers = new MapLayer[0];
    }

    public void SetLayers(MapLayer[] layersToSet)
    {
        SetLayerText(layersToSet, 0);
        mapLayers = layersToSet;
    }

    void SetLayerText(MapLayer[] layers, int level)
    {
        foreach (MapLayer layer in layers)
        {
            layer.layerName = new string('>', level);
            if (layer.children != null)
            {
                SetLayerText(layer.children, level++);
            }
        }
    }

    public List<string> GetLayers()
    {
        List<string> layers = new List<string>();
        foreach (MapLayer layer in mapLayers)
        {
            layers.AddRange(GetLayersRecursively(layer));
        }

        return layers;
    }

    public List<string> GetLayersRecursively(MapLayer parentLayer)
    {
        List<string> layers = new List<string>();
        layers.Add(parentLayer.layerName);

        foreach (MapLayer layer in parentLayer.children)
        {
            layers.Add(layer.layerName);
            if (layer.children != null)
            {
                layers.AddRange(GetLayersRecursively(layer));
            }
        }

        return layers;
    }

}
