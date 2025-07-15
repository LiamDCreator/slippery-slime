using UnityEngine;
using System.Collections;

public enum Faction
{
    Monster,
    Human
}

public class EnemyBase : MonoBehaviour
{
    public straightEnemy straightEnemy;
    public Faction faction;
    public int strength;
    public int originalGravity;
    public bool isFighting = false;
    [SerializeField] private float minimumFightDuration;
    [SerializeField] private float maximumFightDuration;

    public GameObject fightingCloudPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyBase other = collision.gameObject.GetComponent<EnemyBase>();


        // Ignore collisions with enemies of the same faction
        if (other != null && other.faction == faction)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
            return; // Exit the method to avoid further processing
        }
        // Ignore collisions with enemies of the same faction or already fighting
        if (other == null || other.faction == faction || isFighting || other.isFighting)
            return;


        // Mark both as fighting
        isFighting = true;
        other.isFighting = true;

        // Calculate cloud position and size
        Renderer rendA = GetComponentInChildren<Renderer>();
        Renderer rendB = other.GetComponentInChildren<Renderer>();
        Vector3 cloudPos;
        Vector3 cloudScale = Vector3.one;
        if (rendA != null && rendB != null)
        {
            Bounds combinedBounds = rendA.bounds;
            combinedBounds.Encapsulate(rendB.bounds);
            cloudPos = combinedBounds.center + Vector3.up * 0.1f;

            // Scale the cloud to cover both enemies, with some padding
            float padding = 1.2f;
            cloudScale = new Vector3(
                combinedBounds.size.x * padding,
                combinedBounds.size.y * padding,
                1f
            );
        }
        else
        {
            cloudPos = (transform.position + other.transform.position) / 2f + Vector3.up * 0.5f;
        }

        // Instantiate the cloud
        GameObject cloudInstance = Instantiate(fightingCloudPrefab, cloudPos, Quaternion.identity);

        // Set the cloud's scale if calculated
        cloudInstance.transform.localScale = cloudScale;

        // Pass the two enemies to the cloud script
        fightingCloudScript cloudScript = cloudInstance.GetComponent<fightingCloudScript>();

    }

}