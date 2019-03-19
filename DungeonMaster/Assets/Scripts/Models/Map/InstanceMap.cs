using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMap
{
    public string name;
    public List<InstanceMap> regions;
    public Vector2 position;
    public Map map;

    public InstanceMap() {
        name = "new map";
        regions = new List<InstanceMap>();
        position = new Vector2(0, 0);
        map = new Map();
    }
}
