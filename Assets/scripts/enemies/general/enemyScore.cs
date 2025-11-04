using UnityEngine;

public class EnemyScore : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1; // The score value this enemy gives when defeated
    public bool HasBeenScored { get; set; } = false; // Tracks if the enemy has been scored

    public int ScoreValue => scoreValue; // Public property to access the score value
}