using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHierarchyViewModel
{
    MapLayer mapLayers;

    public MapHierarchyViewModel() {
    }

    public void UpdateLayers(WorldMap worldMap) {
        mapLayers = new MapLayer(worldMap.instance.name);
        if (worldMap.instance.regions != null && worldMap.instance.regions.Count > 0) {
            mapLayers.children = SetHierarchyRecursively(worldMap.instance.regions, 1);
        }
    }

    List<MapLayer> SetHierarchyRecursively(List<InstanceMap> maps, int level) {
        List<MapLayer> children = new List<MapLayer>();

        foreach (InstanceMap map in maps) {
            MapLayer child = new MapLayer(new string('>', level) + map.name);
            if(map.regions != null && map.regions.Count > 0) {
                child.children = SetHierarchyRecursively(map.regions, level++);
            }
            children.Add(child);
        }

        return children;
    }

    public List<string> GetHierarchy() {
        List<string> layers = new List<string>();
        layers.Add(mapLayers.layerName);
        if (mapLayers.children != null && mapLayers.children.Count > 0) {
            foreach (MapLayer layer in mapLayers.children) {
                layers.AddRange(GetHierarchyRecursively(layer));
            }
        }
        

        return layers;
    }

    List<string> GetHierarchyRecursively(MapLayer layer) {
        List<string> layers = new List<string>();

        foreach (MapLayer child in layer.children) {
            layers.Add(child.layerName);
            if (child.children != null && child.children.Count > 0) {
                layers.AddRange(GetHierarchyRecursively(child));
            }
        }

        return layers;
    }
}
