using UnityEngine;
using System.Collections;

public class orcCaptainMovement : MonoBehaviour
{
    private float timer;
    private float standStillRate;

    private bool isStandingStill = false;

    private bool leftOrRight;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minimumWalkTime;
    [SerializeField] private float maximumWalkTime;
    [SerializeField] private float minimumStandStillTime = 1f; // Minimum time to stand still
    [SerializeField] private float maximumStandStillTime = 3f; // Maximum time to stand still

    void Start()
    {
        if (transform.position.x > 0)
        {
            leftOrRight = true;
        }
        else if (transform.position.x < 0)
        {
            leftOrRight = false;
        }
        standStillRate = Random.Range(minimumWalkTime, maximumWalkTime);
    }

    void Update()
    {
        if (!isStandingStill) // Only move if not standing still
        {
            move();

            timer += Time.deltaTime;

            if (timer >= standStillRate)
            {
                StartCoroutine(RandomlyStandStill());
            }
        }
    }

    private void move()
    {
        if (leftOrRight)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        if (transform.position.x < -30 || transform.position.x > 30)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
    }

    private IEnumerator RandomlyStandStill()
    {
        isStandingStill = true;

        // Log standing still and wait for a random amount of time
        Debug.Log("Standing still...");
        float standStillTime = Random.Range(minimumStandStillTime, maximumStandStillTime);
        yield return new WaitForSeconds(standStillTime);

        // Reset timer and resume movement
        Debug.Log("Resuming movement...");
        timer = 0f;
        standStillRate = Random.Range(minimumWalkTime, maximumWalkTime);
        isStandingStill = false;
    }
}