using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class SickleController : MonoBehaviour
{
    [Header("Настройки серпа")]
    public float attackRange = .75f;
    public float attackRadius = 0.25f;
    public int damage = 1;
    public float attackCooldown = 0.5f;

    [Inject] private GameManager gameManager;

    [SerializeField] private Transform weapons;

    private int currentWeapon = 0;

    private void Awake() => Init();

    private void Init() => SetWeapon(0);

    private void OnEnable() => gameManager.OnGameStateChanged += GameStateChanged;

    private void OnDisable() => gameManager.OnGameStateChanged -= GameStateChanged;

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.upgradeSickle)
        {
            UpgradeSickle();
        }
    }

    private void UpgradeSickle()
    {
        attackRange += .25f;
        attackRadius += .25f;
        currentWeapon++;

        SetWeapon(currentWeapon);

        var fxPos = new Vector3(transform.position.x, 1f, transform.position.z);

        FxManager.PlayFx(FxType.upgrade, fxPos, Vector3.zero, transform);
        SoundManager.PlaySound(SoundType.upgrade);
    }

    private void SetWeapon(int index)
    {
        if (index > weapons.childCount - 1)
            return;

        for (int i = 0; i < weapons.childCount; i++)
        {
            weapons.GetChild(i).gameObject.SetActive(false);
        }

        weapons.GetChild(index).gameObject.SetActive(true);
    }

    public void Attack()
    {
        CheckGrassHit();
    }

    private async void CheckGrassHit()
    {
        // Получаем позицию перед персонажем
        Vector3 attackPosition = transform.position + transform.forward * attackRange;

        // Ищем всю траву в радиусе атаки
        Collider[] hitColliders = Physics.OverlapSphere(attackPosition, attackRadius);

        foreach (var hitCollider in hitColliders)
        {
            IMinable minable = hitCollider.GetComponent<IMinable>();

            if (minable != null && minable.IsReady())
            {
                minable.Mining();

                SoundManager.PlaySound(SoundType.cut);
                FxManager.PlayFx(FxType.cut, hitCollider.transform.position, Vector3.zero);
            }

            await UniTask.Delay(50);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackPosition = transform.position + transform.forward * attackRange;
        Gizmos.DrawWireSphere(attackPosition, attackRadius);
    }


}