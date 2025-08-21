using UnityEngine;
using System.Collections;


public class powerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnablePowerUps;
    [SerializeField] private float minimumSpawnrate;
    [SerializeField] private float maximumSpawnrate;


    void Start()
    {
        StartCoroutine(powerupSpawnRoutine());
    }

    private IEnumerator powerupSpawnRoutine()
    {
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
            Instantiate(
                spawnablePowerUps[Random.Range(0, spawnablePowerUps.Length)],
                transform.position,
                transform.rotation
            );
        }
    }
}
