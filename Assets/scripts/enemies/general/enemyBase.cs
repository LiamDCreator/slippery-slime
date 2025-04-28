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

        // Start fight animation here
        PlayFightAnimation();

        yield return new WaitForSeconds(1.5f); // Duration of the fight

        // Determine the outcome of the fight
        if (strength >= other.strength)
        {
            Destroy(other.gameObject);
            isFighting = false; // Reset isFighting for the winner
        }
        else
        {
            Destroy(this.gameObject);
            other.isFighting = false; // Reset isFighting for the winner
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