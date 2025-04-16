using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject adventurer;
    [SerializeField] private GameObject goblin;
    [SerializeField] private GameObject bird;
    [SerializeField] private float spawnRate = 1;
    private float timer = 10;
    private GameObject[] spawnableObjects;

     void Start(){
   spawnableObjects = new GameObject[] { adventurer, goblin, bird };
    }
    

    // Update is called once per frame
        void Update()
    {
         if(timer < spawnRate){

            timer = timer + Time.deltaTime;
        } else {
           spawnObject();
            timer = 0;
        }
    }
      void spawnObject()
    {
        Instantiate(spawnableObjects[Random.Range(0, spawnableObjects.Length)], transform.position, transform.rotation);

    }
}
