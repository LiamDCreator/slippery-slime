using UnityEngine;
using System.Collections;

public class orcCaptainMovement : MonoBehaviour
{
    private float timer;
    private float standStillRate;
    public straightEnemy straightEnemy; // Reference to the straightEnemy script



    private bool isStandingStill = false;

    private bool leftOrRight;
    [SerializeField] private float standStillDelay;

    [SerializeField] private float minimumWalkTime;
    [SerializeField] private float maximumWalkTime;
    [SerializeField] private float minimumStandStillTime = 1f; // Minimum time to stand still
    [SerializeField] private float maximumStandStillTime = 3f; // Maximum time to stand still
    [SerializeField] private Color prepareStandStillColor = Color.yellow; // Color during prepare to stand still
    private Color originalColor; // To store the original color of the GameObject
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

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

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Store the original color
        }
    }

    void Update()
    {
        if (!isStandingStill) // Only move if not standing still
        {


            timer += Time.deltaTime;

            if (timer >= standStillRate)
            {
                StartCoroutine(RandomlyStandStill());
            }
        }
    }



    private IEnumerator RandomlyStandStill()
    {


        // Change the color of the GameObject to indicate preparing to stand still
        if (spriteRenderer != null)
        {
            spriteRenderer.color = prepareStandStillColor;
        }

        // Wait for 0.5 seconds before standing still
        Debug.Log("Preparing to stand still...");
        yield return new WaitForSeconds(standStillDelay);
        isStandingStill = true;

        straightEnemy.moveSpeed = 0;
        // Log standing still and wait for a random amount of time
        Debug.Log("Standing still...");
        float standStillTime = Random.Range(minimumStandStillTime, maximumStandStillTime);
        yield return new WaitForSeconds(standStillTime);

        // Reset timer and resume movement
        Debug.Log("Resuming movement...");
        straightEnemy.moveSpeed = straightEnemy.originalmovespeed;
        timer = 0f;
        standStillRate = Random.Range(minimumWalkTime, maximumWalkTime);
        isStandingStill = false;

        // Reset the color back to the original
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}