using UnityEngine;
using DG.Tweening;
using Zenject;

public class Grass : MonoBehaviour, IMinable
{
    public float regrowTime = 30f;

    private bool isCut = false;

    [SerializeField] private Transform model;

    [Inject] private GrassManager grassManager;

    public bool IsReady()
    {
        return !isCut;
    }

    public void Mining()
    {
        CutGrass();
    }

    private void CutGrass()
    {
        isCut = true;

        model.transform.localScale = new Vector3(1f, .05f, 1f);

        Invoke("RegrowGrass", regrowTime);

        grassManager.AddGrass(1);
    }

    private void RegrowGrass()
    {
        model.transform.DOScale(Vector3.one, .25f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isCut = false;
        });
    }
}
