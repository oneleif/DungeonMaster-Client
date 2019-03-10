using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Skills
{
    [SerializeField]
    int acrobatics;
    [SerializeField]
    int animalHandling;
    [SerializeField]
    int arcana;
    [SerializeField]
    int athletics;
    [SerializeField]
    int deception;
    [SerializeField]
    int history;
    [SerializeField]
    int insight;
    [SerializeField]
    int intimidation;
    [SerializeField]
    int investigation;
    [SerializeField]
    int medicine;
    [SerializeField]
    int nature;
    [SerializeField]
    int perception;
    [SerializeField]
    int performance;
    [SerializeField]
    int persuasion;
    [SerializeField]
    int religion;
    [SerializeField]
    int sleightOfHand;
    [SerializeField]
    int stealth;
    [SerializeField]
    int survival;

    public Skills(int acrobatics, int animalHandling, int arcana, int athletics, int deception, int history, int insight, int intimidation, int investigation, int medicine, int nature, int perception, int performance, int persuasion, int religion, int sleightOfHand, int stealth, int survival) {
        this.acrobatics = acrobatics;
        this.animalHandling = animalHandling;
        this.arcana = arcana;
        this.athletics = athletics;
        this.deception = deception;
        this.history = history;
        this.insight = insight;
        this.intimidation = intimidation;
        this.investigation = investigation;
        this.medicine = medicine;
        this.nature = nature;
        this.perception = perception;
        this.performance = performance;
        this.persuasion = persuasion;
        this.religion = religion;
        this.sleightOfHand = sleightOfHand;
        this.stealth = stealth;
        this.survival = survival;
    }
}
