using UnityEngine;
using System.Collections;

public class powerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnablePowerUps;
    [SerializeField] private float minimumSpawnrate;
    [SerializeField] private float maximumSpawnrate;
    [SerializeField] private float timeBeforeSpawn;
    private float leftSpawnChance;
    private float leftSpawnOffset;

    void Start()
    {
        StartCoroutine(powerupSpawnRoutine());
    }

    private IEnumerator powerupSpawnRoutine()
    {
        yield return new WaitForSeconds(timeBeforeSpawn);
        spawnPowerUps();
        while (true)
        {

            float spawnrate = Random.Range(minimumSpawnrate, maximumSpawnrate);
            yield return new WaitForSeconds(spawnrate);
            spawnPowerUps();
        }
    }

    void spawnPowerUps()
    {
        if (spawnablePowerUps.Length > 0)
        {
            Vector3 spawnPos = transform.position;

            // 50/50 chance to spawn at original position or 32 units to the right
            if (Random.Range(0f, 1f) > 0.5f)
            {
                spawnPos.x += 32f;
            }

            Instantiate(
                spawnablePowerUps[Random.Range(0, spawnablePowerUps.Length)],
                spawnPos,
                transform.rotation
            );
        }
    }
}
