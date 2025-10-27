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
    [SerializeField] private SpawnableEntry[] spawnableEvents;
    [SerializeField] private SpawnableEntry[] spawnableFlyingObjects;

    [Header("Ground")]
    [SerializeField] public float groundSpawnRate = 1f;
    [SerializeField] private float minGroundSpawnRate = 0.2f;
    [SerializeField] private float groundSpawnRateDecrease = 0.01f;

    [Header("Flying")]
    [SerializeField] public float flyingSpawnRate = 2f;
    [SerializeField] private float minFlyingSpawnRate = 0.5f;
    [SerializeField] private float flyingSpawnRateDecrease = 0.01f;

    [Header("Waves / Events gating")]
    [SerializeField] private int pointsBeforeWaves = 0;
    [SerializeField] private int pointsBeforeEvents = 0;

    [Header("Spawn Ground Chances ")]
    [Tooltip("Chance (0-100) for ground spawn.")]
    [SerializeField] private int spawnGroundChance = 60;
    [Tooltip("Per-spawn change applied to ground chance (can be negative).")]
    [SerializeField] private int spawnGroundChangeChance = -3;
    [SerializeField] private int spawnGroundMinChance = 0;
    [SerializeField] private int spawnGroundMaxChance = 100;
    [Header("Spawn Wave Chances ")]
    [Tooltip("Chance (0-100) for wave spawn.")]
    [SerializeField] private int spawnWaveChance = 30;
    [Tooltip("Per-spawn change applied to wave chance (can be negative).")]
    [SerializeField] private int spawnWaveChangeChance = 2;
    [SerializeField] private int spawnWaveMinChance = 0;
    [SerializeField] private int spawnWaveMaxChance = 100;
    [Header("Spawn Event Chances ")]

    [SerializeField] private int spawnEventChance = 10;
    [Tooltip("Per-spawn change applied to event chance (can be negative).")]
    [SerializeField] private int spawnEventChangeChance = 1;
    [SerializeField] private int spawnEventMinChance = 0;
    [SerializeField] private int spawnEventMaxChance = 100;





    [Header("Weight control")]
    [Tooltip("Global minimum weight any entry can have (prevents entries dropping to zero and disappearing).")]
    [SerializeField] private float minEntryWeight = 0.01f;

    [Header("References")]
    [Tooltip("Optional: assign a points source so gating uses player progress. If left empty, gating uses the pointsBefore* fields only (<=0 means allowed immediately).")]
    [SerializeField] private gameManager gameManager; // optional, set in inspector if available

    void Start()
    {
        StartCoroutine(GroundSpawnRoutine());
        StartCoroutine(FlyingSpawnRoutine());
    }

    private IEnumerator GroundSpawnRoutine()
    {
        while (true)
        {
            // roll 0..(total-1) where total is sum of effective chances
            // determine whether waves/events are allowed separately
            bool wavesAllowed = pointsBeforeWaves <= 0 || (gameManager != null && gameManager.score >= pointsBeforeWaves);
            bool eventsAllowed = pointsBeforeEvents <= 0 || (gameManager != null && gameManager.score >= pointsBeforeEvents);

            int effectiveEventChance = eventsAllowed ? spawnEventChance : 0;
            int effectiveWaveChance = wavesAllowed ? spawnWaveChance : 0;
            int effectiveGroundChance = spawnGroundChance; // ground always allowed

            int sumEffective = Mathf.Clamp(effectiveEventChance, 0, 100) + Mathf.Clamp(effectiveWaveChance, 0, 100) + Mathf.Clamp(effectiveGroundChance, 0, 100);

            if (sumEffective <= 0)
            {
                // fallback to ground spawn if no effective chances
                SpawnGroundObject();
                UpdateWeights(spawnableGroundObjects);

                // still apply per-spawn deltas so chances evolve toward allowed ranges
                ApplySpawnChanceDeltas();
            }
            else
            {
                int roll = Random.Range(0, sumEffective);
                if (roll < effectiveEventChance)
                {
                    SpawnEvent();
                    UpdateWeights(spawnableEvents);
                }
                else if (roll < effectiveEventChance + effectiveWaveChance)
                {
                    SpawnWaves();
                    UpdateWeights(spawnableWaves);
                }
                else
                {
                    SpawnGroundObject();
                    UpdateWeights(spawnableGroundObjects);
                }

                // After any spawn, apply the configured deltas to all three chances and clamp
                ApplySpawnChanceDeltas();
            }

            // Wait and decay spawn rate
            yield return new WaitForSeconds(groundSpawnRate);
            groundSpawnRate = Mathf.Max(minGroundSpawnRate, groundSpawnRate - groundSpawnRateDecrease);
        }
    }

    private void ApplySpawnChanceDeltas()
    {
        spawnEventChance = Mathf.Clamp(spawnEventChance + spawnEventChangeChance, spawnEventMinChance, spawnEventMaxChance);
        spawnWaveChance = Mathf.Clamp(spawnWaveChance + spawnWaveChangeChance, spawnWaveMinChance, spawnWaveMaxChance);
        spawnGroundChance = Mathf.Clamp(spawnGroundChance + spawnGroundChangeChance, spawnGroundMinChance, spawnGroundMaxChance);
    }

    private IEnumerator FlyingSpawnRoutine()
    {
        while (true)
        {
            SpawnFlyingObject();
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
        // For multi-enemy waves you can call SpawnByWeight multiple times here
        SpawnByWeight(spawnableWaves);
    }

    void SpawnFlyingObject()
    {
        SpawnByWeight(spawnableFlyingObjects);
    }

    void SpawnEvent()
    {
        SpawnByWeight(spawnableEvents);
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

        // Fallback to uniform pick if all weights are <= 0
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
            if (e == null || e.prefab == null) continue;

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
        if (entries == null || entries.Length == 0) return;

        for (int i = 0; i < entries.Length; i++)
        {
            var e = entries[i];
            if (e == null) continue;

            float newWeight = e.weight + e.weightIncrease;

            float perEntryMin = Mathf.Max(e.weightMinimum, minEntryWeight);
            float perEntryMax = Mathf.Max(e.weightMaximum, perEntryMin);

            e.weight = Mathf.Clamp(newWeight, perEntryMin, perEntryMax);
        }
    }
}