using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : Enemy
{
    NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        Move();
    }

    private void Move()
    {
        if (isCanMove)
        {
            if (health < 50)
                agent.destination = FindNearestEnemy();
            else
                agent.destination = Player.GetPlayer.transform.position;

            agent.isStopped = false;

            isMove = true;

            distanceTraveled += moveSpeed * Time.deltaTime;
            if (distanceTraveled >= moveDistance || PlayerVector().magnitude < 10)
            {
                agent.isStopped = true;
                distanceTraveled = 0;
                rigibody.velocity = Vector3.zero;
                isMove = false;
                StartCoroutine(Wait());
            }
        }
    }

    /// <summary>
    /// Поиск ближайшего врага
    /// </summary>
    private Vector3 FindNearestEnemy()
    {
        if (GetLiveEnemies.Count > 1)
        {
            List<(Enemy enemy, float distance)> enemies = new List<(Enemy, float)>();

            foreach (var enemy in GetLiveEnemies)
            {
                var distance = enemy.transform.position - transform.position;

                if (enemy != this)
                    enemies.Add((enemy, distance.magnitude));
            }

            enemies.Sort((a, b) => { return a.distance > b.distance ? 1 : -1; });

            return enemies[0].enemy.transform.position;
        }
        else
            // если остался один
            return Player.GetPlayer.transform.position;
    }

    protected override void Death()
    {
        agent.enabled = false;
        base.Death();
    }
}