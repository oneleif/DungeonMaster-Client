using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : IPlaceable
{
    public PlaceableType placeableType;
}

public interface IPlaceable
{
    
}

public enum PlaceableType
{
    player, npc, monster, item
}
