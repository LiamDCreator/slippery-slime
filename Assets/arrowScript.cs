using UnityEngine;

public class arrowScript : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(Vector2.right * moveSpeed, ForceMode2D.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the arrow to match its velocity direction
        if (rb.linearVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        Destroy(gameObject);
    }
}
