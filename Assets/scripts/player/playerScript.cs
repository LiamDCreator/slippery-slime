using UnityEngine;

public class playerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded;
    private bool canfastFall;
    public int jumpsRemaining = 2;

    [SerializeField] private float forceAmount = 10f;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private gameOverScreenScript gameOverScreen;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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

        // Reset jumps only when landing
        if (!wasGrounded && isGrounded)
        {
            jumpsRemaining = 2;
            canfastFall = true;
        }
    }

    private void playerJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // fixed to velocity
            jumpsRemaining--;

            audioManager.Instance.PlaySFX(audioManager.Instance.jumpSFX);
        }
    }

    private void playerDash()
    {
        if (jumpsRemaining <= 1 && jumpsRemaining > 0)
        {
            Vector2 dashDirection = Vector2.zero;

            if (Input.GetKeyDown(KeyCode.D))
                dashDirection = Vector2.right;
            else if (Input.GetKeyDown(KeyCode.A))
                dashDirection = Vector2.left;
            else if (Input.GetKeyDown(KeyCode.Q))
                dashDirection = (Vector2.left + Vector2.up).normalized;
            else if (Input.GetKeyDown(KeyCode.E))
                dashDirection = (Vector2.right + Vector2.up).normalized;

            if (dashDirection != Vector2.zero)
            {
                rb.AddForce(dashDirection * forceAmount, ForceMode2D.Impulse);
                jumpsRemaining--;
                audioManager.Instance.PlaySFX(audioManager.Instance.dashSFX);
            }
        }
    }
    private void playerFastFall()
    {

        if (Input.GetKeyDown(KeyCode.S) && canfastFall == true)
        {
            rb.AddForce((Vector2.down).normalized * forceAmount, ForceMode2D.Impulse);
            canfastFall = false;
            Debug.Log("Yuuuup");
            audioManager.Instance.PlaySFX(audioManager.Instance.downDashSFX);

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            gameOverScreen.gameOver();

        }
    }

}