using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class GrassUi : MonoBehaviour
{
    [SerializeField] private TMP_Text grassText;

    [Inject] private GrassManager grassManager;

    private Tween scaleTw;

    private void OnEnable()
    {
        grassManager.OnGrassChanged += MoneyChanged;
        grassManager.notEnough += Shake;
    }

    private void OnDisable()
    {
        grassManager.OnGrassChanged -= MoneyChanged;
        grassManager.notEnough -= Shake;
    }

    private void Shake()
    {
        if (scaleTw == null)
        {
            scaleTw = grassText.transform.DOShakeScale(.25f, .25f).OnComplete(() =>
            {
                grassText.transform.localScale = Vector3.one;
                scaleTw = null;
            });
        }        
    }

    private void MoneyChanged(int moneyCount)
    {
        grassText.text = moneyCount.ToString();
        Shake();
    }
}
