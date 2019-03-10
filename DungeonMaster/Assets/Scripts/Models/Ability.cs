using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Ability
{
    public static Ability defaultAbility = new Ability(0, "1d6", 0, "swing a sword", "slice");

    [SerializeField]
    int damageBonus;
    [SerializeField]
    string damageDice;
    [SerializeField]
    int attackBonus;
    [SerializeField]
    string description;
    [SerializeField]
    string name;

    public Ability(int damageBonus, string damageDice, int attackBonus, string description, string name) {
        this.damageBonus = damageBonus;
        this.damageDice = damageDice;
        this.attackBonus = attackBonus;
        this.description = description;
        this.name = name;
    }
}
