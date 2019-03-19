using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap
{
    public InstanceMap instance;
    public string partyPosition;

    public WorldMap(string name) {
        instance = new InstanceMap();
        instance.name = name;
        instance.map.backgroundLayer.image = Resources.Load<Sprite>("WorldMaps/fantasy_map_1");
    }
}
