using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState;

    public static event Action<GameState> OnGameStateChange;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState) //sets states to the game to make the map, spawn units, and turn base.
    {
        gameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnNPC:
                UnitManager.Instance.SpawnNPC();
                break;
            case GameState.SpawnPuzzle:
                UnitManager.Instance.SpawnPuzzle();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChange?.Invoke(newState);
    }


    public enum GameState
    {
        GenerateGrid,
        SpawnNPC,
        SpawnPuzzle,
        Solved,
    }
}
