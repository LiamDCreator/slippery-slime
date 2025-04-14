using UnityEngine;

public class birdScript : MonoBehaviour
{
    [SerializeField] private float LowestPoint;
    [SerializeField] private float highestPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
        // Get current position
        Vector2 newPos = transform.position;

        // Modify only the y value
        newPos.y = Random.Range(LowestPoint,highestPoint);

        // Apply the new position
        transform.position = newPos;
    }
}
