using UnityEngine;
using System.Collections;

public class wolfJumpScript : MonoBehaviour
{
    private Rigidbody2D rb;

    private float jumpRate;
    [SerializeField] private float jumpDelay;
    [SerializeField] private float jumpForce;
    [SerializeField] private float minimumJumpRate;
    [SerializeField] private float maximumJumpRate;
    [SerializeField] private Color prepareJumpColor = Color.red; // Color during prepareJump
    private Color originalColor; // To store the original color of the GameObject
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    void Start()
    {
        jumpRate = Random.Range(minimumJumpRate, maximumJumpRate);
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Store the original color
        }

        StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        while (true)
        {
            // Wait for the prepare jump time
            yield return new WaitForSeconds(jumpRate - jumpDelay);

            prepareJump();

            // Wait for the jump delay
            yield return new WaitForSeconds(jumpDelay);

            jump();

            // Reset the jump rate for the next jump
            jumpRate = Random.Range(minimumJumpRate, maximumJumpRate);
        }
    }

    private void prepareJump()
    {
        Debug.Log("Prepared to jump!");

        // Change the color of the GameObject to indicate jump preparation
        if (spriteRenderer != null)
        {
            spriteRenderer.color = prepareJumpColor;
        }
    }

    private void jump()
    {
        Debug.Log("Jumping!");

        // Apply the jump force
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // Reset the color back to the original
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}