using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    [SerializeField] private CollectableItems[] itemPrefab;
    [SerializeField] private List<SpawnPoint> spawnPoints;

    private void Start()
    {
        StartCoroutine(SpawnItemsCoroutine());
    }

    private IEnumerator SpawnItemsCoroutine()
    {
        while (true)
        {
            SpawnPoint point = spawnPoints[Random.Range(0, spawnPoints.Count)];
            if (point.TrySpawnObject(itemPrefab[Random.Range(0, itemPrefab.Length)]))
            {
                yield return new WaitForSeconds(spawnInterval);
            }
            yield return null;
        }
    }
}
