using UnityEngine;
using System.Collections;

public class SpawnerDebug : MonoBehaviour 
{
    GuardSpawner spawner;

    void Start()
    {
    }

    void OnDrawGizmosSelected()
    {
        spawner = GetComponent<GuardSpawner>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawner.spawnDistanceStart);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawner.spawnDistanceEnd);
    }
}
