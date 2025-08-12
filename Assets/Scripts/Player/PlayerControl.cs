using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float maximumSpeed;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private TargetHandler targetHandler;
    [SerializeField] private PlayerAnimationHandler playerAnimationHandler;

    private float ySpeed;
    private CharacterController characterController;
    private Tween rotateTw;

    [Inject] private InputManager inputManager;

    private void Awake() => characterController = GetComponent<CharacterController>();

    private void OnEnable()
    {
        inputManager.OnPlayerMoveChanged += Move;
        inputManager.OnPointerJoystickDownChanged += JoystickDownChanged;
    }

    private void OnDisable()
    {
        inputManager.OnPlayerMoveChanged -= Move;
        inputManager.OnPointerJoystickDownChanged -= JoystickDownChanged;
    }

    private void JoystickDownChanged()
    {
        rotateTw.Kill();
        rotateTw = null;
    }

    private void Move(Vector2 dir)
    {
        Vector3 movedirection = new(dir.x, 0f, dir.y);

        if (dir.sqrMagnitude != 0)
        {
            rotateTw.Kill();
            rotateTw = null;
        }

        float inputMagnitude = Mathf.Clamp01(movedirection.magnitude);

        var speed = inputMagnitude * maximumSpeed;

        movedirection.y = 0f;
        movedirection.Normalize();

        float gravity = Physics.gravity.y * gravityMultiplier;

        if (ySpeed > -70)
        {
            ySpeed += gravity * Time.deltaTime;
        }

        Vector3 velocity = movedirection * speed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (movedirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movedirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 850 * Time.deltaTime);
        }
    }

    public void DoRotate(Vector3 targetPos)
    {
        rotateTw.Kill();
        rotateTw = null;

        Vector3 dir = targetPos - transform.position;
        dir.y = 0;
        dir.Normalize();
        Quaternion toRotation = Quaternion.LookRotation(dir, Vector3.up);

        rotateTw = transform.DORotateQuaternion(toRotation, .25f).OnComplete(() =>
        {
            rotateTw.Kill();
            rotateTw = null;

            playerAnimationHandler.Slash();
        });
    }
}
