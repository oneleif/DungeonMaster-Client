using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : IPlaceable
{
    public Vector2 mapPosition;
    public PlaceableType placeableType;
}

public interface IPlaceable
{
    
}

public enum PlaceableType
{
    player, npc, monster, item
}
