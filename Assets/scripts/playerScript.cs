using UnityEngine;

public class playerScript : MonoBehaviour
{ 
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded;
    public int jumpsRemaining = 2;
    
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private float forceAmount = 10f;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundMask;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        playerJump();  // input checking here is fine
        playerDash();
    }

    void FixedUpdate()
    {
        CheckGrounded(); // physics check in FixedUpdate
    }

    private void CheckGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        // Reset jumps only when landing
        if (!wasGrounded && isGrounded)
        {
            jumpsRemaining = 2;
        }
    }

    private void playerJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // fixed to velocity
            jumpsRemaining--;
        }
    }

    private void playerDash()
    {
        if (jumpsRemaining <= 1 && jumpsRemaining > 0)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                rb.AddForce(Vector2.right * forceAmount, ForceMode2D.Impulse);
                jumpsRemaining--;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                rb.AddForce(Vector2.left * forceAmount, ForceMode2D.Impulse);
                jumpsRemaining--;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                rb.AddForce((Vector2.left + Vector2.up).normalized * forceAmount, ForceMode2D.Impulse);
                jumpsRemaining--;
            }
             if (Input.GetKeyDown(KeyCode.E))
            {
                rb.AddForce((Vector2.right + Vector2.up).normalized * forceAmount, ForceMode2D.Impulse);
                jumpsRemaining--;
            }
        }
    }
          private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            gameOver();

        }
    }
    private void gameOver(){
    gameOverCanvas.gameObject.SetActive(true);

    }
    
    }