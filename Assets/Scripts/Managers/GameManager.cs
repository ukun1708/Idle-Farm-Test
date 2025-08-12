using System;
using UnityEngine;

public enum GameState
{
    startGame,
    upgradeSickle
}

public class GameManager : MonoBehaviour
{
    public GameState State;

    public event Action<GameState> OnGameStateChanged;

    private void Start()
    {
        UpdateGameState(GameState.startGame);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        OnGameStateChanged?.Invoke(newState);
    }
}
