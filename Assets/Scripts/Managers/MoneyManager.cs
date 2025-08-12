using System;
using UnityEngine;
using Zenject;

public class MoneyManager : MonoBehaviour
{
    [Inject] private GameManager gameManager;

    [SerializeField] private int _currentMoney = 0;
    public event Action<int> OnMoneyChanged;
    public event Action<int> OnMoneyAmount;

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
            RefreshMoney();
        }
    }

    public void RefreshMoney()
    {
        OnMoneyChanged?.Invoke(_currentMoney);
    }

    public void AddMoney(int amount)
    {
        _currentMoney += amount;
        OnMoneyChanged?.Invoke(_currentMoney);

        OnMoneyAmount?.Invoke(amount);

        //SoundManager.PlaySound(SoundType.money);
    }

    public void SpendMoney(int amount)
    {
        if (_currentMoney >= amount)
        {
            _currentMoney -= amount;
            OnMoneyChanged?.Invoke(_currentMoney);
        }
    }

    public bool HasEnoughMoney(int amount)
    {
        return _currentMoney >= amount;
    }

    public int GetCurrentMoney()
    {
        return _currentMoney;
    }

    public void InvokeNotEnough() => notEnough?.Invoke();
}
