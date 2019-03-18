using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap
{
    public string name;
    public Sprite image;
    public Color backgroundColor;
    public InstanceMap[] instances;
    public string partyPosition;

    public WorldMap(string name) {
        this.name = name;
        image = Resources.Load<Sprite>("WorldMaps/fantasy_map_1");
    }
}
