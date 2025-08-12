using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event Action<Vector2> OnPlayerMoveChanged;
    public event Action OnPointerJoystickUpChanged;
    public event Action OnPointerJoystickDownChanged;

    public void UpdatePlayerMoveChanged(Vector2 dir) => OnPlayerMoveChanged?.Invoke(dir);

    public void PointerJoystickUpChangedInvoke() => OnPointerJoystickUpChanged?.Invoke();

    public void PointerJoystickDownChangedInvoke() => OnPointerJoystickDownChanged?.Invoke();

}
