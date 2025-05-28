using UnityEngine;

public class godAdventurerMovement : MonoBehaviour
{
    public straightEnemy straightEnemy;
    public float interval = 3f; // Seconds between speed increases
    public float speedIncrease = 1f;

    void Start()
    {
        StartCoroutine(IncreaseSpeedRoutine());
    }

    private System.Collections.IEnumerator IncreaseSpeedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            if (straightEnemy != null)
            {
                straightEnemy.originalmovespeed += speedIncrease;
            }
        }
    }
}
