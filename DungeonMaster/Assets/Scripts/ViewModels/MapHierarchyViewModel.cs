using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHierarchyViewModel {
    MapLayer mapLayers;

    public MapHierarchyViewModel() {
    }

    public List<MapLayer> GetHierarchy(WorldMap worldMap) {
        List<MapLayer> hierarchy = new List<MapLayer>();
        hierarchy.Add(new MapLayer(0, worldMap.instance));
        hierarchy.AddRange(GetHierarchyRecursively(worldMap.instance, 0));

        return hierarchy;
    }

    List<MapLayer> GetHierarchyRecursively(InstanceMap instance, int level) {
        List<MapLayer> layers = new List<MapLayer>();

        foreach (InstanceMap child in instance.regions) {
            layers.Add(new MapLayer(level + 1, child));
            if (child.regions != null && child.regions.Count > 0) {
                layers.AddRange(GetHierarchyRecursively(child, level + 1));
            }
        }

        return layers;
    }
}
