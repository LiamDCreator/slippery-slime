using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fightingCloudScript : MonoBehaviour
{
    public List<EnemyBase> monsters = new List<EnemyBase>();
    public List<EnemyBase> humans = new List<EnemyBase>();
    [SerializeField] private int totalMonsterStrength = 0;
    [SerializeField] private int totalHumanStrength = 0;
    [SerializeField] private float TimeToFight = 0;
    [SerializeField] private float timeBetweenCombat = 0;


    void Start()
    {
        StartCoroutine(fightingLogic());
    }
    void Update()
    {



    }
    public void AddFighter(EnemyBase enemy)
    {
        if (enemy.faction == Faction.Monster && !monsters.Contains(enemy))
        {
            monsters.Add(enemy);
            totalMonsterStrength += enemy.strength; // Update total immediately
        }
        else if (enemy.faction == Faction.Human && !humans.Contains(enemy))
        {
            humans.Add(enemy);
            totalHumanStrength += enemy.strength; // Update total immediately
        }

        enemy.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                AddFighter(enemy);
            }
        }
    }
    private IEnumerator fightingLogic()
    {
        yield return new WaitForSeconds(TimeToFight);
        while (humans.Count > 0 && monsters.Count > 0)
        {
            DistributeAndSubtractStrength(monsters, totalMonsterStrength, humans);
            DistributeAndSubtractStrength(humans, totalHumanStrength, monsters);
            checkIfFactionDied();

            yield return new WaitForSeconds(timeBetweenCombat);
        }
    }

    private void DistributeAndSubtractStrength(List<EnemyBase> fromFaction, int totalStrength, List<EnemyBase> toFaction)
    {
        if (toFaction.Count == 0) return;

        // Step 1: Generate random weights
        List<float> weights = new List<float>();
        float sum = 0f;
        foreach (var enemy in toFaction)
        {
            float w = Random.value;
            weights.Add(w);
            sum += w;
        }

        // Step 2: Normalize weights and subtract strength using a backwards for loop (Option 2)
        for (int i = toFaction.Count - 1; i >= 0; i--)
        {
            var enemy = toFaction[i];
            float percent = weights[i] / sum;
            int takenStrength = Mathf.RoundToInt(percent * totalStrength);
            enemy.strength -= takenStrength; // Subtract from current strength
            if (enemy.strength <= 0)
            {
                Destroy(enemy.gameObject);
                toFaction.RemoveAt(i); // Remove from list immediately
            }
        }
    }

    private void checkIfFactionDied()
    {
        if (humans.Count == 0 && monsters.Count > 0)
        {
            foreach (var enemy in monsters)
            {
                enemy.gameObject.SetActive(true);
            }
            Destroy(gameObject);
        }
        else if (monsters.Count == 0 && humans.Count > 0)
        {
            foreach (var enemy in humans)
            {
                enemy.gameObject.SetActive(true);
            }
            Destroy(gameObject);
        }
        if (humans.Count == 0 && monsters.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}