using UnityEngine;
using DG.Tweening;
using System;

public class SoldGrass : MonoBehaviour
{
    private Tween jumpTw;

    public void Jump(Transform parent, Vector3 jumpPos, float speed, Action action)
    {
        jumpTw.Kill();

        transform.SetParent(parent);
        jumpTw = transform.DOLocalJump(jumpPos, 2f, 1, speed).OnComplete(() =>
        {
            action();
        });
    }
}
