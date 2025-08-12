using UnityEngine;
using System.Collections;

public class straightEnemy : MonoBehaviour
{
    public float moveSpeed;
    public float originalmovespeed;
    [SerializeField] private float TimeBeforeBehaviours;
    [Header("change Directions")]
    [SerializeField] private bool doesEnemyChangeDirection;
    [SerializeField] private float changeDirectionMinimunInterval;
    [SerializeField] private float changeDirectionMaximumInterval;
    [Header("Stand still")]

    [SerializeField] private bool doesEnemyStandStill;


    [SerializeField] private float minimumWalkTime;
    [SerializeField] private float maximumwalktime;
    [SerializeField] private float minimumstandStillTime;
    [SerializeField] private float maximumStandStillTime;
    private bool movingRight = true;

    void Start()
    {
        // Rotate the enemy to face the correct direction based on its spawn position
        if (transform.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
            movingRight = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
            movingRight = true;
        }
        originalmovespeed = moveSpeed;

        if (doesEnemyChangeDirection)
        {
            StartCoroutine(ChangeDirectionRoutine());
        }
        if (doesEnemyStandStill)
        {
            StartCoroutine(StandStillRoutine());
        }
    }

    void Update()
    {
        // Move the enemy forward based on its current rotation
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // Destroy the enemy if it moves out of bounds
        if (transform.position.x < -30 || transform.position.x > 30)
        {
            Destroy(gameObject);
        }
    }


    private IEnumerator ChangeDirectionRoutine()
    {
        yield return new WaitForSeconds(TimeBeforeBehaviours);
        while (true)
        {
            float waitTime = Random.Range(changeDirectionMinimunInterval, changeDirectionMaximumInterval);
            yield return new WaitForSeconds(waitTime);
            movingRight = !movingRight;
            // Flip the enemy visually
            transform.rotation = Quaternion.Euler(0, movingRight ? 0 : 180, 0);
        }
    }
    private IEnumerator StandStillRoutine()
    {
        yield return new WaitForSeconds(TimeBeforeBehaviours);
        while (true)
        {
            float walkingTime = Random.Range(minimumWalkTime, maximumwalktime);
            yield return new WaitForSeconds(walkingTime);
            moveSpeed = 0;
            float standStillTime = Random.Range(minimumstandStillTime, maximumStandStillTime);
            yield return new WaitForSeconds(standStillTime);
            moveSpeed = originalmovespeed;
        }
    }
}