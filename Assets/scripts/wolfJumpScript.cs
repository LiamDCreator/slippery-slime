using UnityEngine;

public class wolfJumpScript : MonoBehaviour
{
    private Rigidbody2D rb;

    private float timer;
    private float jumpRate;
    [SerializeField] private float jumpForce;
    [SerializeField] private float minimumJumpRate;
    [SerializeField] private float maximumJumpRate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpRate = Random.Range(minimumJumpRate, maximumJumpRate);
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= jumpRate)
        {
            jump();

            timer = 0;
        }

    }
    private void jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpRate = Random.Range(minimumJumpRate, maximumJumpRate);


    }
}
