using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Obsolete("This will be replaced by the WeaponData class. ")]
[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject prefab;
    public GameObject Prefab { get => prefab; private set => prefab = value; }

    //Base stats for the weapon
    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }

    [SerializeField]
    float speed;
    public float Speed { get => speed; private set => speed = value; }

    [SerializeField]
    float cooldownDuration;
    public float CooldownDuration { get => cooldownDuration; private set => cooldownDuration = value; }

    [SerializeField]
    int pierce;
    public int Pierce { get => pierce; private set => pierce = value; }

    [SerializeField]
    int level;
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefabs;
    public GameObject NextLevelPrefabs { get => nextLevelPrefabs; private set => nextLevelPrefabs = value; }

    [SerializeField]
    new string name;
    public string Name { get => name;private set => name = value; }

    [SerializeField]
    string description;//What is the description of this weapon? [if this weapon is an upgrade, place the description of the upgrades]
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    Sprite icon; // Not mean modified in game [Only in editor]
    public Sprite Icon { get => icon; private set => icon = value; }


    [SerializeField]
    int evolveUpdateToRemove; // Not mean modified in game [Only in editor]
    public int EvolveUpdateToRemove { get => evolveUpdateToRemove; private set => evolveUpdateToRemove = value; }
}