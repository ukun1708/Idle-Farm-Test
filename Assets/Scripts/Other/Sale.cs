using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Sale : MonoBehaviour, ITargetable
{
    [SerializeField] private Transform salePoint;
    [SerializeField] private SoldGrass grassPrefab;
    [SerializeField] private float offsetY = .5f;
    [SerializeField] private QueueSystem queueSystem;

    [Inject] private GrassManager grassManager;

    private void Update()
    {
        if (queueSystem.PurchaseReady() && salePoint.childCount > 0)
        {  
            queueSystem.ProcessPurchase(salePoint.GetChild(salePoint.childCount - 1));
        }
    }

    public async void TargetEnter(Transform user)
    {
        float pitch = 1;

        while (grassManager.GetCurrentGrass() > 0)
        {
            grassManager.SpendGrass(1);

            var rot = new Vector3(0f, Random.Range(-10f, 10f), 0f);
            SoldGrass grass = Instantiate(grassPrefab, user.position + Vector3.up, Quaternion.identity);
            grass.transform.eulerAngles = rot;
            var pos = Vector3.zero;

            if (salePoint.childCount > 0)
            {
                //pos = salePoint.GetChild(salePoint.childCount - 1).localPosition + Vector3.up * offsetY;
                pos = new Vector3(0, salePoint.childCount, 0) * offsetY;
            }

            grass.Jump(salePoint, pos, .25f, () =>
            {
                SoundManager.PlaySound(SoundType.grass, 1f, pitch);
                pitch += .1f;
            });

            await UniTask.Delay(100);
        }
    }

    public void TargetLeave()
    {
        
    }
}
