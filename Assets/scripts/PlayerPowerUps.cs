using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerPowerUps : MonoBehaviour
{
    public playerScript playerScript;
    public gameManager gameManager;
    [SerializeField] private float starPowerDuration;
    [SerializeField] private float unlimitedJumpsPowerDuration;
    public ParticleSystem starpowerParticles;
    public ParticleSystem unlimitedJumpsParticles;


    public void UnlimitedJumpsPower()
    {
        StartCoroutine(playerHasUnlimitedJumps());
    }
    public void StarPower()
    {
        StartCoroutine(playerHasStarPower());

    }

    private IEnumerator playerHasUnlimitedJumps()
    {
        unlimitedJumpsParticles.Play();
        playerScript.jumpsRemaining = 100;
        playerScript.playerHasUnlimitedJumps = true;
        playerScript.canfastFall = true;
        yield return new WaitForSeconds(unlimitedJumpsPowerDuration);
        playerScript.jumpsRemaining = 2;
        playerScript.playerHasUnlimitedJumps = false;
        yield return null;
    }
    private IEnumerator playerHasStarPower()
    {
        playerScript.playerCanDie = false;
        starpowerParticles.Play();
        yield return new WaitForSeconds(starPowerDuration);
        playerScript.playerCanDie = true;
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerScript.playerCanDie == false)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);

                Destroy(collision.gameObject);

                gameManager.score += 1;
                gameManager.scoreText.text = "" + gameManager.score;

            }
        }
    }
}
