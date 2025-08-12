using System;
using UnityEngine;
using Zenject;

public class GrassManager : MonoBehaviour
{
    [Inject] private GameManager gameManager;

    private int _currentGrass = 0;
    public event Action<int> OnGrassChanged;
    public event Action<int> OnGrassAmount;

    public event Action notEnough;

    private void OnEnable()
    {
        gameManager.OnGameStateChanged += OnGameStateChanged;
    }
    private void OnDisable()
    {
        gameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.startGame)
        {
            RefreshGrass();
        }
    }

    public void RefreshGrass()
    {
        OnGrassChanged?.Invoke(_currentGrass);
    }

    public void AddGrass(int amount)
    {
        _currentGrass += amount;
        OnGrassChanged?.Invoke(_currentGrass);

        OnGrassAmount?.Invoke(amount);

        //SoundManager.PlaySound(SoundType.money);
    }

    public void SpendGrass(int amount)
    {
        if (_currentGrass >= amount)
        {
            _currentGrass -= amount;
            OnGrassChanged?.Invoke(_currentGrass);
        }
    }

    public bool HasEnoughGrass(int amount)
    {
        return _currentGrass >= amount;
    }

    public int GetCurrentGrass()
    {
        return _currentGrass;
    }

    public void InvokeNotEnough() => notEnough?.Invoke();
}
