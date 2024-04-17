using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour //handles spawing heroes on the map.
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;
    public BaseHero SelectedHero;
    public BaseEnemy SelectedEnemy;

    void Awake()
    {
        Instance = this;
        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();//reads all character units and turns it into a list.
    }
    //spawn a set amount of hero units.
    public void SpawnHeroes()
    {
        var heroCount = 3;

        for (int i = 0; i < heroCount; i++)
        {
            var heroPrefab = GetUnits<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(heroPrefab);
            var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();

            randomSpawnTile.SetUnit(spawnedHero);
        }
        GameManager.Instance.ChangeState(GameManager.GameState.SpawnPuzzle);
    }
    //spawns a set amount of enemies from the list of enemy units.
    public void SpawnEnemies()
    {
        var enemyCount = 6;

        for (int i = 0; i < enemyCount; i++)
        {
            var enemyPrefab = GetUnits<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(enemyPrefab);
            var randomSpawnTile = GridManager.Instance.GetPuzzleSpawnTile();

            randomSpawnTile.SetUnit(spawnedEnemy);
        }

        // GameManager.Instance.ChangeState(GameManager.GameState.HeroesTurn); -- Prob dont need this code
    }
    //reads the list of units in per function based on which spawn function, selects units at random can get duplicates.
    private T GetUnits<T>(Faction faction) where T : BaseUnit
    {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => UnityEngine.Random.value).First().UnitPrefab;
    }
}
