﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item : Placeable
{
    public static Item defaultItem = new Item(null, "bag of holding", "holds a ton of stuff", "looks like a brown burlap sack", "image", 1, new Currency(1, 0, 0, 0, 0), 0);
    [SerializeField]
    string id;
    [SerializeField]
    string name;
    [SerializeField]
    string description;
    [SerializeField]
    string visualDescription;
    [SerializeField]
    string image;
    [SerializeField]
    int stackAmount;
    [SerializeField]
    Currency value;
    [SerializeField]
    int rarity;

    public Item(string id, string name, string description, string visualDescription, string image, int stackAmount, Currency value, int rarity) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.visualDescription = visualDescription;
        this.image = image;
        this.stackAmount = stackAmount;
        this.value = value;
        this.rarity = rarity;
        placeableType = PlaceableType.item;
    }
}

[Serializable]
public class ItemList {
    public Item[] items;
}

[Serializable]
public class Currency {
    [SerializeField]
    int pp;
    [SerializeField]
    int gp;
    [SerializeField]
    int ep;
    [SerializeField]
    int sp;
    [SerializeField]
    int cp;

    public Currency(int pp, int gp, int ep, int sp, int cp) {
        this.pp = pp;
        this.gp = gp;
        this.ep = ep;
        this.sp = sp;
        this.cp = cp;
    }
}
