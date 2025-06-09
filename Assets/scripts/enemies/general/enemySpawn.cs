using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnableGroundObjects;
    [SerializeField] private GameObject[] spawnableFlyingObjects;
    [SerializeField] public float groundSpawnRate = 1f;
    [SerializeField] public float flyingSpawnRate = 2f;
    [SerializeField] private float minGroundSpawnRate = 0.2f;
    [SerializeField] private float minFlyingSpawnRate = 0.5f;
    [SerializeField] private float groundSpawnRateDecrease = 0.01f;
    [SerializeField] private float flyingSpawnRateDecrease = 0.01f;

    void Start()
    {
        StartCoroutine(GroundSpawnRoutine());
        StartCoroutine(FlyingSpawnRoutine());
    }

    private System.Collections.IEnumerator GroundSpawnRoutine()
    {
        while (true)
        {
            SpawnGroundObject();
            yield return new WaitForSeconds(groundSpawnRate);
            groundSpawnRate = Mathf.Max(minGroundSpawnRate, groundSpawnRate - groundSpawnRateDecrease);
        }
    }

    private System.Collections.IEnumerator FlyingSpawnRoutine()
    {
        while (true)
        {
            SpawnFlyingObject();
            yield return new WaitForSeconds(flyingSpawnRate);
            flyingSpawnRate = Mathf.Max(minFlyingSpawnRate, flyingSpawnRate - flyingSpawnRateDecrease);
        }
    }

    void SpawnGroundObject()
    {
        if (spawnableGroundObjects.Length > 0)
        {
            Instantiate(
                spawnableGroundObjects[Random.Range(0, spawnableGroundObjects.Length)],
                transform.position,
                transform.rotation
            );
        }
    }

    void SpawnFlyingObject()
    {
        if (spawnableFlyingObjects.Length > 0)
        {
            Instantiate(
                spawnableFlyingObjects[Random.Range(0, spawnableFlyingObjects.Length)],
                transform.position,
                transform.rotation
            );
        }
    }
}