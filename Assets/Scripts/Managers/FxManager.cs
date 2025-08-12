using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FxType
{
    cut,
    coin,
    grass,
    upgrade
}

[ExecuteInEditMode]
public class FxManager : MonoBehaviour
{
    [SerializeField] private FxList[] fxList;
    private static FxManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static void PlayFx(FxType fxType, Vector3 createPos, Vector3 rotation, Transform parent = null)
    {
        GameObject[] particles = instance.fxList[(int)fxType].Particles;
        GameObject randomParticle = particles[UnityEngine.Random.Range(0, particles.Length)];

        GameObject particle = Instantiate(randomParticle, createPos, Quaternion.Euler(rotation), parent);

        Destroy(particle, 2f);
    }


#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(FxType));
        Array.Resize(ref fxList, names.Length);
        for (int i = 0; i < fxList.Length; i++)
        {
            fxList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct FxList
{
    public GameObject[] Particles { get => particles; }
    [HideInInspector] public string name;
    [SerializeField] private GameObject[] particles;
}
