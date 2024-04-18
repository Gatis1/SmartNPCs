using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour //handles spawing heroes on the map.
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _NPC;
    private List<ScriptableUnit> _NPCcopy;

    void Awake()
    {
        Instance = this;
        _NPC = Resources.LoadAll<ScriptableUnit>("NPC").ToList();//reads all character units and turns it into a list.
        _NPCcopy = new List<ScriptableUnit>(_NPC);
    }
    //spawn a set amount of hero units.
    public void SpawnNPC()
    {
        var heroCount = 2;

        for (int i = 0; i < heroCount; i++)
        {
            var heroPrefab = GetUnits<BaseNPC>(Faction.NPC);
            var spawnedHero = Instantiate(heroPrefab);
            var randomSpawnTile = GridManager.Instance.GetNPCSpawnTile();

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

    private GetUnits(Faction faction)
    {
        if (!_NPCcopy.Any()) // reset the copy if all units have been selected
        {
            _NPCcopy = new List<ScriptableUnit>(_NPC);
        }
        var selectedUnit = _NPCcopy.Where(u => u.Faction == faction).OrderBy(o => UnityEngine.Random.value).First();
        _NPCcopy.Remove(selectedUnit); // remove the selected unit from the copy
        return selectedUnit.UnitPrefab;
    }
}
