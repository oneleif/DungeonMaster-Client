using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMap {
    public string name;
    public List<InstanceMap> regions;
    public Vector2 position;
    public Map map;

    public InstanceMap(string name = "new map") {
        this.name = name;
        regions = new List<InstanceMap>();
        position = new Vector2(0, 0);
        map = new Map();
    }
}
