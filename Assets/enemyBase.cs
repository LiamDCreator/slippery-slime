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

    private void OnTriggerEnter2D(Collider2D collision)
    {


        EnemyBase other = collision.GetComponent<EnemyBase>();

        if (other != null && other.faction == faction)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision);
            return; // Exit the method to avoid further processing
        }

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