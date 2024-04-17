using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Unit")]
public class ScriptableUnit : ScriptableObject
{
    public Faction Faction;
    public Role role;
    public DMGtype type;
    public BaseUnit UnitPrefab;
}

public enum Faction
{
    Hero = 0,
    Enemy = 1
}

public enum Role
{
    Warrior,
    Rogue,
    Mage,
    Bandit,
    Beast,
    Fiend
}

public enum DMGtype
{
    PHYS,
    MAG
}