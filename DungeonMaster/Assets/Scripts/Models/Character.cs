using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Character
{
    public static Character defaultCharacter = new Character(null, "leif", 0, "Dwarf", "Mine", 0, 0, 10, 0, 0, 0, 0, new Ability[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { }, Skills.defaultSkills, Stats.defaultStats, Stats.defaultStats);

    [SerializeField]
    string id;
    [SerializeField]
    string name;
    [SerializeField]
    int size;
    [SerializeField]
    string type;
    [SerializeField]
    string subType;
    [SerializeField]
    int alignment;
    [SerializeField]
    int ac;
    [SerializeField]
    int hp;
    [SerializeField]
    int speed;
    [SerializeField]
    int level;
    [SerializeField]
    int experience;
    [SerializeField]
    int characterType;
    [SerializeField]
    Ability[] abilities;
    [SerializeField]
    string[] languages;
    [SerializeField]
    string[] senses;
    [SerializeField]
    string[] invulnerabilities;
    [SerializeField]
    string[] resistances;
    [SerializeField]
    Skills skills;
    [SerializeField]
    Stats savingThrows;
    [SerializeField]
    Stats coreStats;

    public Character(string id, string name, int size, string type, string subType, int alignment, int ac, int hp, int speed, int level, int experience, int characterType, Ability[] abilities, string[] languages, string[] senses, string[] invulnerabilities, string[] resistances, Skills skills, Stats savingThrows, Stats coreStats) {
        this.id = id;
        this.name = name;
        this.size = size;
        this.type = type;
        this.subType = subType;
        this.alignment = alignment;
        this.ac = ac;
        this.hp = hp;
        this.speed = speed;
        this.level = level;
        this.experience = experience;
        this.characterType = characterType;
        this.abilities = abilities;
        this.languages = languages;
        this.senses = senses;
        this.invulnerabilities = invulnerabilities;
        this.resistances = resistances;
        this.skills = skills;
        this.savingThrows = savingThrows;
        this.coreStats = coreStats;
    }
}

[Serializable]
public class CharacterList {
    public Character[] characters;
}