using UnityEngine;

public class straightEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private bool leftOrRight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }
    private void move()
    {
        if (leftOrRight == true)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else if (leftOrRight == false)
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
}
