using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fightingCloudScript : MonoBehaviour
{
    public List<EnemyBase> monsters = new List<EnemyBase>();
    public List<EnemyBase> humans = new List<EnemyBase>();
    [SerializeField] private int totalMonsterStrength = 0;
    [SerializeField] private int totalHumanStrength = 0;

    void Start()
    {

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
        // Reset totals before calculation
        totalMonsterStrength = 0;
        totalHumanStrength = 0;

        // Sum strengths for monsters
        foreach (var enemy in monsters)
            totalMonsterStrength += enemy.strength;

        // Sum strengths for humans
        foreach (var enemy in humans)
            totalHumanStrength += enemy.strength;

        yield return null; // If you want to yield for coroutine compatibility
    }
}