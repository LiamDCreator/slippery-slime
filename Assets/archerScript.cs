using UnityEngine;
using System.Collections;

public class archerScript : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private float shootRate;
    [SerializeField] private float minimumShootRate;
    [SerializeField] private float maximumShootRate;

    void Start()
    {
        shootRate = Random.Range(minimumShootRate, maximumShootRate);

        StartCoroutine(ShootRoutine());


    }
    void Update()
    {

    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            // Wait for the prepare jump time
            yield return new WaitForSeconds(shootRate);

            // Check if the wolf is fighting before preparing to jump

            ShootArrow();

            // Wait for the jump delay

            shootRate = Random.Range(minimumShootRate, maximumShootRate);
        }
    }
    private void ShootArrow()
    {
        Instantiate(arrow, transform.position, transform.rotation);

    }
}
