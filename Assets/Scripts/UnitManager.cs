using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour //handles spawing heroes on the map.
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;

    void Start()
    {
        Instance = this;
        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();//reads all character units and turns it into a list.
    }
    //spawn a set amount of hero units.
    public void SpawnNPC()
    {
        var heroCount = 2;

        for (int i = 0; i < heroCount; i++)
        {
            var npcPrefab = GetUnits<BaseNPC>(Faction.NPC);
            var spawnNPC= Instantiate(npcPrefab);
            var randomSpawnTile = GridManager.Instance.GetNPCSpawnTile();

            randomSpawnTile.SetUnit(spawnNPC);
        }
        GameManager.Instance.ChangeState(GameManager.GameState.SpawnPuzzle);
    }
    //spawns a set amount of enemies from the list of enemy units.
    public void SpawnPuzzle()
    {
        var enemyCount = 2;

        for (int i = 0; i < enemyCount; i++)
        {
            var puzzlePrefab = GetUnits<BasePuzzle>(Faction.Puzzle);
            var spawnPuzzle = Instantiate(puzzlePrefab);
            var randomSpawnTile = GridManager.Instance.GetPuzzleSpawnTile();

            randomSpawnTile.SetUnit(spawnPuzzle);
        }
    }

    //reads the list of units based on their faction, selects units at random can get duplicates.
    private T GetUnits<T>(Faction faction) where T : BaseUnit
    {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => UnityEngine.Random.value).First().UnitPrefab;
    }
}
