using UnityEngine;
using System.Collections;

public class enemySpawn : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableEntry
    {
        public GameObject prefab;
        [Tooltip("Relative chance for this entry to be chosen. Higher = more likely.")]
        public float weight = 1f;

        [Tooltip("Amount added to weight each update (can be negative). Applied and then clamped between per-entry min and max.")]
        public float weightIncrease = 0f;

        [Tooltip("Minimum allowed weight for this entry.")]
        public float weightMinimum = 0.01f;

        [Tooltip("Maximum allowed weight for this entry.")]
        public float weightMaximum = 10f;
    }

    [Header("Spawnable Objects")]
    [SerializeField] private SpawnableEntry[] spawnableGroundObjects;
    [SerializeField] private SpawnableEntry[] spawnableWaves;
    [SerializeField] private SpawnableEntry[] spawnableFlyingObjects;

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

    [Header("Weight control")]
    [Tooltip("Global minimum weight any entry can have (prevents entries dropping to zero and disappearing).")]
    [SerializeField] private float minEntryWeight = 0.01f;

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

                // Slightly adjust weights for ground entries after each ground spawn
                UpdateWeights(spawnableGroundObjects);

                yield return new WaitForSeconds(groundSpawnRate);
                groundSpawnRate = Mathf.Max(minGroundSpawnRate, groundSpawnRate - groundSpawnRateDecrease);
                spawnWaveChance += spawnWaveChanceIncrease;
            }
            else
            {
                SpawnWaves();

                // Slightly adjust wave entry weights after a wave spawn
                UpdateWeights(spawnableWaves);

                yield return new WaitForSeconds(groundSpawnRate);
                groundSpawnRate = Mathf.Max(minGroundSpawnRate, groundSpawnRate - groundSpawnRateDecrease);
                spawnWaveChance -= spawnWaveChanceDecrease;
            }

            // Keep spawnWaveChance in a safe range
            spawnWaveChance = Mathf.Clamp(spawnWaveChance, 0, 99);
        }
    }

    private IEnumerator FlyingSpawnRoutine()
    {
        while (true)
        {
            SpawnFlyingObject();

            // Slightly adjust flying entries weights after each flying spawn
            UpdateWeights(spawnableFlyingObjects);

            yield return new WaitForSeconds(flyingSpawnRate);
            flyingSpawnRate = Mathf.Max(minFlyingSpawnRate, flyingSpawnRate - flyingSpawnRateDecrease);
        }
    }

    void SpawnGroundObject()
    {
        SpawnByWeight(spawnableGroundObjects);
    }

    void SpawnWaves()
    {
        SpawnByWeight(spawnableWaves);
    }

    void SpawnFlyingObject()
    {
        SpawnByWeight(spawnableFlyingObjects);
    }

    // Helper: choose an entry based on its weight and instantiate it at this transform
    private void SpawnByWeight(SpawnableEntry[] entries)
    {
        if (entries == null || entries.Length == 0)
            return;

        float totalWeight = 0f;
        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i] != null && entries[i].prefab != null)
                totalWeight += Mathf.Max(0f, entries[i].weight);
        }

        // If all weights are zero or invalid, fallback to uniform random among valid prefabs
        if (totalWeight <= 0f)
        {
            int tries = 0;
            while (tries < entries.Length)
            {
                int idx = Random.Range(0, entries.Length);
                if (entries[idx] != null && entries[idx].prefab != null)
                {
                    Instantiate(entries[idx].prefab, transform.position, transform.rotation);
                    return;
                }
                tries++;
            }
            return;
        }

        float r = Random.Range(0f, totalWeight);
        float accum = 0f;
        for (int i = 0; i < entries.Length; i++)
        {
            var e = entries[i];
            if (e == null || e.prefab == null)
                continue;

            accum += Mathf.Max(0f, e.weight);
            if (r <= accum)
            {
                Instantiate(e.prefab, transform.position, transform.rotation);
                return;
            }
        }
    }

    // Update weights for a set of entries: apply weightIncrease and clamp between per-entry min and max
    private void UpdateWeights(SpawnableEntry[] entries)
    {
        if (entries == null || entries.Length == 0)
            return;

        for (int i = 0; i < entries.Length; i++)
        {
            var e = entries[i];
            if (e == null)
                continue;

            // Apply incremental change
            float newWeight = e.weight + e.weightIncrease;

            // Determine per-entry min and max, ensure sensible ordering and enforce global minEntryWeight
            float perEntryMin = Mathf.Max(e.weightMinimum, minEntryWeight);
            float perEntryMax = Mathf.Max(e.weightMaximum, perEntryMin);

            e.weight = Mathf.Clamp(newWeight, perEntryMin, perEntryMax);
        }
    }
}