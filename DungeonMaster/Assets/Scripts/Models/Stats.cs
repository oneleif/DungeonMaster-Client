using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stats
{
    [SerializeField]
    int strength;
    [SerializeField]
    int dexterity;
    [SerializeField]
    int constitution;
    [SerializeField]
    int intelligence;
    [SerializeField]
    int wisdom;
    [SerializeField]
    int charisma;

    public Stats(int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma) {
        this.strength = strength;
        this.dexterity = dexterity;
        this.constitution = constitution;
        this.intelligence = intelligence;
        this.wisdom = wisdom;
        this.charisma = charisma;
    }
}
