using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayer {
    public int level;
    public InstanceMap instance;

    public MapLayer(int level, InstanceMap instance) {
        this.level = level;
        this.instance = instance;
    }
}
