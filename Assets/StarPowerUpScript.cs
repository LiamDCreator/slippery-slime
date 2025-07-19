using UnityEngine;
using System.Collections;

public class StarPowerUpScript : MonoBehaviour
{
    public PlayerPowerUps playerPowerUps;

    private bool goUp = true;

    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float moveUPSpeed = 1.0f;
    [SerializeField] private float minimumTime = 0.5f;
    [SerializeField] private float maximumTime = 2.0f;

    private void Start()
    {
        StartCoroutine(SwitchDirectionRoutine());
        if (transform.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
        }
    }

    private void Update()
    {
        Move();

        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // Destroy the enemy if it moves out of bounds
        if (transform.position.x < -30 || transform.position.x > 30)
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        Vector3 direction = goUp ? Vector3.up : Vector3.down;
        transform.Translate(direction * moveUPSpeed * Time.deltaTime);
    }

    private IEnumerator SwitchDirectionRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minimumTime, maximumTime);
            yield return new WaitForSeconds(waitTime);
            goUp = !goUp;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerPowerUps.StarPower();
            Destroy(gameObject);
        }
    }
}