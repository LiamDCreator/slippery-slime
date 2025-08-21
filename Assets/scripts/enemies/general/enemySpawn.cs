using UnityEngine;
using System.Collections;
public class enemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnableGroundObjects;
    [SerializeField] private GameObject[] spawnableWaves;
    [SerializeField] private GameObject[] spawnableFlyingObjects;
    [Header("Ground")]
    [SerializeField] public float groundSpawnRate = 1f;
    [SerializeField] private float minGroundSpawnRate = 0.2f;
    [SerializeField] private float groundSpawnRateDecrease = 0.01f;


    [Header("Flying")]
    [SerializeField] public float flyingSpawnRate = 2f;
    [SerializeField] private float minFlyingSpawnRate = 0.5f;
    [SerializeField] private float flyingSpawnRateDecrease = 0.01f;

    [Header("Waves")]

    [SerializeField] private int pointsBeforeWaves;
    [SerializeField] private int spawnWaveChance;
    [SerializeField] private int spawnWaveNumber;
    [SerializeField] private int spawnWaveChanceIncrease;
    [SerializeField] private int spawnWaveChanceDecrease;



    void Start()
    {
        StartCoroutine(GroundSpawnRoutine());
        StartCoroutine(FlyingSpawnRoutine());
    }

    private IEnumerator GroundSpawnRoutine()
    {
        while (true)
        {
            spawnWaveNumber = Random.Range(0, 100);
            if (spawnWaveNumber > spawnWaveChance)
            {
                SpawnGroundObject();
                yield return new WaitForSeconds(groundSpawnRate);
                groundSpawnRate = Mathf.Max(minGroundSpawnRate, groundSpawnRate - groundSpawnRateDecrease);
                spawnWaveChance += spawnWaveChanceIncrease;
            }
            else
            {
                SpawnWaves();
                yield return new WaitForSeconds(groundSpawnRate);
                groundSpawnRate = Mathf.Max(minGroundSpawnRate, groundSpawnRate - groundSpawnRateDecrease);
                spawnWaveChance -= spawnWaveChanceDecrease;

            }
        }
    }

    private IEnumerator FlyingSpawnRoutine()
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
    void SpawnWaves()
    {
        if (spawnableWaves.Length > 0)
        {
            Instantiate(
                spawnableWaves[Random.Range(0, spawnableWaves.Length)],
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