using UnityEngine;
using Zenject;

public class InputHandler : MonoBehaviour
{
    private Joystick joystick;

    [Inject] private InputManager inputManager;

    private void Awake() => joystick = GetComponent<Joystick>();

    private void Update()
    {
        inputManager.UpdatePlayerMoveChanged(joystick.Direction);
    }
}
