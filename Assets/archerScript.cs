using UnityEngine;
using System.Collections;

public class archerScript : MonoBehaviour
{
    public straightEnemy straightEnemy;
    [SerializeField] private GameObject arrow;
    [SerializeField] private float shootRate;
    [SerializeField] private float standStillTime;
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

            yield return new WaitForSeconds(shootRate);


            straightEnemy.moveSpeed = 0;


            shootRate = Random.Range(minimumShootRate, maximumShootRate);

            yield return new WaitForSeconds(standStillTime);
            ShootArrow();
            straightEnemy.moveSpeed = straightEnemy.originalmovespeed;
        }
    }
    private void ShootArrow()
    {
        Instantiate(arrow, transform.position, transform.rotation);

    }
}
