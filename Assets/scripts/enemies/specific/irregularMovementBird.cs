using UnityEngine;
using System.Collections;


public class irregularMovementBird : MonoBehaviour
{
    private bool goUp = true;

    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float minimumTime = 0.5f;
    [SerializeField] private float maximumTime = 2.0f;

    private void Start()
    {
        StartCoroutine(SwitchDirectionRoutine());
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 direction = goUp ? Vector3.up : Vector3.down;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
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
}