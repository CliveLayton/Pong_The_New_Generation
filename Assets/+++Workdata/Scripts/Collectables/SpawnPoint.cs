using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float radius = 1;

    public bool TrySpawnObject(CollectableItems prefab)
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, Vector2.zero);

        if (hit.collider != null)
        {
            return false;
        }

        Instantiate(prefab, transform.position, Quaternion.identity);

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
