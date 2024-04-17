using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public GameObject DamagePopUp;
    public string UnitName;
    public Tile OccupiedTile;
    public Faction Faction;
    public Role role;
    public DMGtype type;
    public int actionCount = 1;

    [SerializeField]  public int HP;
    //[SerializeField] public int DMG;
    [SerializeField] public int STR;
    [SerializeField] public int DEF;
    [SerializeField] public int INT;
    [SerializeField] public int RES;
    //[SerializeField] public int DEX;
    //[SerializeField] public int MV;
}