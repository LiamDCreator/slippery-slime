using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded;
    public bool canfastFall;
    public bool playerCanDie = true;
    public bool playerHasUnlimitedJumps = false;
    public int jumpsRemaining = 2;

    [SerializeField] private float forceAmount = 10f;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private gameOverScreenScript gameOverScreen;
    [SerializeField] private Animator animator;
    public PlayerPowerUps PlayerPowerUps;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        playerJump();
        playerDash();
        playerFastFall();
    }

    void FixedUpdate()
    {
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        if (isGrounded)
        {
            canfastFall = true;
        }
        // Reset jumps only when landing
        if (!wasGrounded && isGrounded)
        {
            jumpsRemaining = 2;

            PlayerPowerUps.unlimitedJumpsParticles.Stop();

        }
    }

    private void playerJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // fixed to velocity
            jumpsRemaining--;

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
            audioManager.Instance.PlaySFX(audioManager.Instance.jumpSFX);
        }
    }

    private void playerDash()
    {
        if (jumpsRemaining <= 1 && jumpsRemaining > 0 || jumpsRemaining >= 5 && jumpsRemaining > 0)
        {
            Vector2 dashDirection = Vector2.zero;
            float targetRotation = 0f;
            if (Input.GetKeyDown(KeyCode.D))
            {
                dashDirection = Vector2.right;
                targetRotation = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                dashDirection = Vector2.left;
                targetRotation = 180f;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                dashDirection = (Vector2.left + Vector2.up).normalized;
                targetRotation = 180f;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                dashDirection = (Vector2.right + Vector2.up).normalized;
                targetRotation = 0f;
            }
            if (dashDirection != Vector2.zero)
            {
                rb.AddForce(dashDirection * forceAmount, ForceMode2D.Impulse);
                jumpsRemaining--;
                audioManager.Instance.PlaySFX(audioManager.Instance.dashSFX);
                transform.rotation = Quaternion.Euler(0f, targetRotation, 0f);
            }
        }
    }

    private void playerFastFall()
    {

        if (Input.GetKeyDown(KeyCode.S) && canfastFall == true)
        {
            rb.AddForce((Vector2.down).normalized * forceAmount, ForceMode2D.Impulse);
            if (playerHasUnlimitedJumps == false)
            {
                canfastFall = false;
            }

            Debug.Log("Yuuuup");
            audioManager.Instance.PlaySFX(audioManager.Instance.downDashSFX);

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerCanDie == true)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
                gameOverScreen.gameOver();

            }
        }
    }

}