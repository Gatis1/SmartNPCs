using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Unit")]
public class ScriptableUnit : ScriptableObject
{
    public Faction Faction;
    public Role role;
    public _Key key;
    public BaseUnit UnitPrefab;
}

public enum Faction
{
    NPC = 0,
    Puzzle = 1
}

public enum Role
{
    NPC,
    Puzzle
}

public enum _Key
{
    key1,
    key2
}