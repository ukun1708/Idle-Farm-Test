using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class Workshop : MonoBehaviour, ITargetable
{
    [Inject] private MoneyManager moneyManager;
    [Inject] private GameManager gameManager;

    [SerializeField] private Transform salePoint;
    [SerializeField] private Transform coinPrefab;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private int price = 50;

    private int startPrice;

    private void Start()
    {
        SetText();
        startPrice = price;
    }

    private void SetText() => priceText.text = price.ToString();

    public async void TargetEnter(Transform user)
    {
        while (moneyManager.GetCurrentMoney() > 0)
        {
            moneyManager.SpendMoney(1);

            var rot = new Vector3(0f, Random.Range(-10f, 10f), 0f);
            Transform coin = Instantiate(coinPrefab, user.position + Vector3.up, Quaternion.identity);
            coin.eulerAngles = rot;
            var pos = Vector3.zero;

            coin.SetParent(salePoint);
            coin.DOLocalJump(pos, 2f, 1, .2f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                coin.Rotate(Vector3.up * 360 * Time.deltaTime);

            }).OnComplete(() =>
            {
                price--;

                if (price <= 0)
                {
                    price = startPrice;
                    SetText();
                    gameManager.UpdateGameState(GameState.upgradeSickle);
                }

                SetText();

                FxManager.PlayFx(FxType.coin, coin.position, Vector3.zero);
                SoundManager.PlaySound(SoundType.coin);

                Destroy(coin.gameObject);
            });

            await UniTask.Delay(100);
        }
    }

    public void TargetLeave()
    {
        
    }
}
