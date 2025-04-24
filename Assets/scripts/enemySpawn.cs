using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnableObjects; // Array of spawnable objects
    [SerializeField] private float spawnRate = 1f; // Time between spawns
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    void SpawnObject()
    {
        if (spawnableObjects.Length > 0)
        {
            // Spawn a random object from the array
            Instantiate(
                spawnableObjects[Random.Range(0, spawnableObjects.Length)],
                transform.position,
                transform.rotation
            );
        }
    }
}