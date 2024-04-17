using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour //handles actions done on tiles such as character controls.
{
    public string TileName;
    [SerializeField] protected SpriteRenderer render;
    [SerializeField] private GameObject _highlight, CharHighlight;
    [SerializeField] private bool _isWalkable;

    public static int heroes = 2;
    public static int eneimes = 2;

    public BaseUnit OccupiedUnit;

    public bool Walkable => _isWalkable && OccupiedUnit == null;

    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null)
        {
            unit.OccupiedTile.OccupiedUnit = null;
        }
        if (!_isWalkable) //units cannot be placed on non-walkable terrian.
        {
            return;
        }
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }

    //private void MoveUnit(BaseUnit unit)
    //{
    //    for (int x = 0; x < unit.MV; x++)
    //    {
    //        for (int y = 0; y < unit.MV; y++)
    //        {
    //            if (_isWalkable)
    //            {
    //                if (path.FindPath(unitX, unitY, x, y).Count() <= unit.MV)
    //                {
    //                    CharHighlight.SetActive(true);
    //                }
    //            }
    //        }
    //    }
    //}
}
