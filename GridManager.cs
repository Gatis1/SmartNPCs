using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Tilemap obstacleTilemap; // Tilemap for obstacles
    public Tilemap spawnPointTilemap; // Tilemap for spawn points

    public Vector2Int gridSize; // Size of the grid

    private Vector3Int[] obstaclePositions; // Array to store obstacle positions

    // Start is called before the first frame update
    void Start()
    {
        InitializeGrid();
        FindObstaclePositions();
        SetSpawnPoints();
    }

    void InitializeGrid()
    {
        // Initialize the grid size based on the tilemap size
        gridSize = new Vector2Int(obstacleTilemap.size.x, obstacleTilemap.size.y);
    }

    void FindObstaclePositions()
    {
        // Find obstacle positions by iterating through the obstacle tilemap
        List<Vector3Int> positions = new List<Vector3Int>();
        foreach (Vector3Int pos in obstacleTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (obstacleTilemap.HasTile(localPlace))
            {
                positions.Add(localPlace);
            }
        }
        obstaclePositions = positions.ToArray();
    }

    void SetSpawnPoints()
    {
        // Set spawn points based on the spawn point tilemap
        foreach (Vector3Int pos in spawnPointTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (spawnPointTilemap.HasTile(localPlace))
            {
                Vector3 spawnPosition = spawnPointTilemap.CellToWorld(localPlace);
                // Set spawn point at spawnPosition
            }
        }
    }

    public bool IsObstacle(Vector3Int gridPosition)
    {
        // Check if the grid position contains an obstacle
        foreach (Vector3Int obstaclePos in obstaclePositions)
        {
            if (gridPosition.Equals(obstaclePos))
            {
                return true;
            }
        }
        return false;
    }

    public Vector3Int WorldToGrid(Vector3 worldPosition)
    {
        // Convert world position to grid position
        return obstacleTilemap.WorldToCell(worldPosition);
    }

    public Vector3 GridToWorld(Vector3Int gridPosition)
    {
        // Convert grid position to world position
        return obstacleTilemap.CellToWorld(gridPosition);
    }
}
