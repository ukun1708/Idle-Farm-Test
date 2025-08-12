using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private GrassManager grassManager;

    public override void InstallBindings()
    {
        Container.BindInstance(gameManager);
        Container.BindInstance(inputManager);
        Container.BindInstance(moneyManager);
        Container.BindInstance(grassManager);
    }
}