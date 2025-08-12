using UnityEngine;
using Zenject;

public class GrassesGridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private int gridSize = 10;
    [SerializeField] private float cellSize = 1f;

    [Inject] private DiContainer container;

    void Start()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 spawnPos = new Vector3(x * cellSize, 0, z * cellSize);
                GameObject grass = container.InstantiatePrefab(grassPrefab);
                grass.transform.SetParent(transform);
                grass.transform.localPosition = spawnPos;
                grass.transform.localEulerAngles = Vector3.zero;
            }
        }
    }
}
