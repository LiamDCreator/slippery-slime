using UnityEngine;
using System.Collections;

public enum Faction
{
    Monster,
    Human
}

public class EnemyBase : MonoBehaviour
{
    public straightEnemy straightEnemy; // Reference to the straightEnemy script
    public Faction faction;
    public int strength;
    public bool isFighting = false;
    [SerializeField] private float minimumFightDuration;
    [SerializeField] private float maximumFightDuration;

    public GameObject fightingCloudPrefab; // Assign this in the Inspector

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyBase other = collision.gameObject.GetComponent<EnemyBase>();

        // Ignore collisions with enemies of the same faction
        if (other != null && other.faction == faction)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
            return; // Exit the method to avoid further processing
        }

        // Start a fight if the factions are different and neither is already fighting
        if (other != null && other.faction != faction && !isFighting && !other.isFighting)
        {
            StartCoroutine(HandleFight(other));
        }
    }

    private IEnumerator HandleFight(EnemyBase other)
    {
        isFighting = true;
        other.isFighting = true;

        // Stop movement for both enemies at the start of the fight
        if (straightEnemy != null)
        {
            straightEnemy.stopEnemy();
        }
        if (other.straightEnemy != null)
        {
            other.straightEnemy.stopEnemy();
        }

        Collider2D thisCollider = GetComponent<Collider2D>();
        Collider2D otherCollider = other.GetComponent<Collider2D>();

        Rigidbody2D thisRigidbody = GetComponent<Rigidbody2D>();
        Rigidbody2D otherRigidbody = other.GetComponent<Rigidbody2D>();

        // Disable colliders and set gravity scale to 0
        if (thisCollider != null)
        {
            thisCollider.enabled = false;
        }
        if (otherCollider != null)
        {
            otherCollider.enabled = false;
        }
        if (thisRigidbody != null)
        {
            thisRigidbody.gravityScale = 0;
        }
        if (otherRigidbody != null)
        {
            otherRigidbody.gravityScale = 0;
        }

        // Instantiate fighting cloud above the two fighters
        GameObject cloudInstance = null;
        if (fightingCloudPrefab != null)
        {
            // Try to use the Renderer bounds for both enemies
            Renderer rendA = GetComponentInChildren<Renderer>();
            Renderer rendB = other.GetComponentInChildren<Renderer>();

            if (rendA != null && rendB != null)
            {
                // Calculate the combined bounds
                Bounds combinedBounds = rendA.bounds;
                combinedBounds.Encapsulate(rendB.bounds);

                // Center position for the cloud
                Vector3 cloudPos = combinedBounds.center + Vector3.up * 0.1f; // Slightly above center if needed

                // Instantiate the cloud
                cloudInstance = Instantiate(fightingCloudPrefab, cloudPos, Quaternion.identity);

                // Scale the cloud to cover both enemies, with padding
                float padding = 1.2f; // Increase for more coverage
                Vector3 newScale = new Vector3(
                    combinedBounds.size.x * padding,
                    combinedBounds.size.y * padding,
                    1f
                );
                cloudInstance.transform.localScale = newScale;
            }
            else
            {
                // Fallback: use the old method if no renderer is found
                Vector3 center = (transform.position + other.transform.position) / 2f;
                Vector3 cloudPos = center + Vector3.up * 0.5f;
                cloudInstance = Instantiate(fightingCloudPrefab, cloudPos, Quaternion.identity);
            }
        }

        // Start fight animation here
        PlayFightAnimation();
        float fightDuration = Random.Range(minimumFightDuration, maximumFightDuration);

        yield return new WaitForSeconds(fightDuration);

        // Destroy the cloud after the fight
        if (cloudInstance != null)
        {
            Destroy(cloudInstance);
        }

        // Determine the outcome of the fight
        if (strength >= other.strength)
        {
            // Winner is 'this', loser is 'other'
            strength -= other.strength;
            Destroy(other.gameObject);

            if (strength <= 0)
            {
                Destroy(this.gameObject); // Winner also dies if strength is 0 or less
            }
            else
            {
                isFighting = false; // Reset isFighting for the winner
            }
        }
        else
        {
            // Winner is 'other', loser is 'this'
            other.strength -= strength;
            Destroy(this.gameObject);

            if (other.strength <= 0)
            {
                Destroy(other.gameObject); // Winner also dies if strength is 0 or less
            }
            else
            {
                other.isFighting = false; // Reset isFighting for the winner
            }
        }

        // Re-enable colliders and restore gravity scale
        if (thisCollider != null)
        {
            thisCollider.enabled = true;
        }
        if (otherCollider != null)
        {
            otherCollider.enabled = true;
        }
        if (thisRigidbody != null)
        {
            thisRigidbody.gravityScale = 1;
        }
        if (otherRigidbody != null)
        {
            otherRigidbody.gravityScale = 1;
        }

        // Reset movement speed for the winner
        if (straightEnemy != null)
        {
            straightEnemy.moveSpeed = straightEnemy.originalmovespeed;
        }
        if (other.straightEnemy != null)
        {
            other.straightEnemy.moveSpeed = other.straightEnemy.originalmovespeed;
        }
    }

    private void PlayFightAnimation()
    {
        // Placeholder for fight animation logic
        Debug.Log("Fight animation plays here.");
    }
}