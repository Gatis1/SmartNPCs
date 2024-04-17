using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour //handles spawing heroes on the map.
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _NPC;

    void Awake()
    {
        Instance = this;
        _NPC = Resources.LoadAll<ScriptableUnit>("NPC").ToList();//reads all character units and turns it into a list.
    }
    //spawn a set amount of hero units.
    public void SpawnNPC()
    {
        var heroCount = 2;

        for (int i = 0; i < heroCount; i++)
        {
            var heroPrefab = GetUnits<BaseNPC>(Faction.NPC);
            var spawnedHero = Instantiate(heroPrefab);
            var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();

            randomSpawnTile.SetUnit(spawnedHero);
        }
        GameManager.Instance.ChangeState(GameManager.GameState.SpawnPuzzle);
    }
    //spawns a set amount of enemies from the list of enemy units.
    public void SpawnPuzzle()
    {
        var enemyCount = 2;

        for (int i = 0; i < enemyCount; i++)
        {
            var enemyPrefab = GetUnits<BasePuzzle>(Faction.Puzzle);
            var spawnedEnemy = Instantiate(enemyPrefab);
            var randomSpawnTile = GridManager.Instance.GetPuzzleSpawnTile();

            randomSpawnTile.SetUnit(spawnedEnemy);
        }
    }
    //reads the list of units in per function based on which spawn function, selects units at random can get duplicates.
    private T GetUnits<T>(Faction faction) where T : BaseUnit
    {
        return (T)_NPC.Where(u=>u.Faction == faction).OrderBy(o => UnityEngine.Random.value).First().UnitPrefab;
    }
}
