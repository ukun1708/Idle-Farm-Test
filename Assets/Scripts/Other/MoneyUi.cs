using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using Zenject;

public class MoneyUi : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    [Inject] private MoneyManager moneyManager;

    private Tween scaleTw;

    private void OnEnable()
    {
        moneyManager.OnMoneyChanged += MoneyChanged;
        moneyManager.notEnough += Shake;
    }

    private void OnDisable()
    {
        moneyManager.OnMoneyChanged -= MoneyChanged;
        moneyManager.notEnough -= Shake;
    }

    private void Shake()
    {
        if (scaleTw == null)
        {
            scaleTw = moneyText.transform.DOShakeScale(.25f, .25f).OnComplete(() =>
            {
                moneyText.transform.localScale = Vector3.one;
                scaleTw = null;
            });
        }        
    }

    private void MoneyChanged(int moneyCount)
    {
        moneyText.text = moneyCount.ToString();
        Shake();
    }
}
