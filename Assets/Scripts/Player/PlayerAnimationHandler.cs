using UnityEngine;
using Zenject;

public class PlayerAnimationHandler : MonoBehaviour
{
    [Inject] private InputManager inputManager;

    [SerializeField] private SickleController sickleController;
    [SerializeField] private TargetHandler targetHandler;

    private Animator animator;

    private void Awake() => animator = GetComponent<Animator>();

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
        animator.SetBool("slash", false);
    }

    private void Move(Vector2 vector)
    {
        Vector3 movedirection = new(vector.x, 0f, vector.y);

        float inputMagnitude = Mathf.Clamp01(movedirection.magnitude);

        if (animator != null)
        {
            animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);
        }
    }

    public void Slash()
    {
        animator.SetBool("slash", true);
    }

    public void Attack()
    {
        sickleController.Attack();        
    }

    public void SlashEnd()
    {
        animator.SetBool("slash", false);
        targetHandler.JoystickUpChanged();
    }
}
