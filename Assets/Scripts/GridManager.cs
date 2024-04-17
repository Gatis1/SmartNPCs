using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] public int width, height;
    [SerializeField] private Tile _grassTile, _forestTile;
    [SerializeField] private Transform _camera;

    private Dictionary<Vector2, Tile> _tiles;

    void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid() //generates the map/grid.
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var randomTile = UnityEngine.Random.Range(0, 6) == 3 ? _forestTile : _grassTile; //randomly sets tiles to a different tile type for variety.
                var spawnedTile = Instantiate(randomTile, new Vector3(x, y, 0), Quaternion.identity);


                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        _camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);//changes camera position to view the whole grid, designed for 16:9.

        GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public Tile GetHeroSpawnTile()
    {
        int spawnAreaSize = 3; // size of the spawn area
        return _tiles.Where(t => t.Key.x < spawnAreaSize && t.Key.y < spawnAreaSize && t.Value.Walkable).OrderBy(t => UnityEngine.Random.value).First().Value;
    }

    public Tile GetPuzzleSpawnTile()
    {
        return _tiles.Where(t => t.Key.x > width / 2 && t.Value.Walkable).OrderBy(t => UnityEngine.Random.value).First().Value;//spawns enemies on right side of map.
    }
}
