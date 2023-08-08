using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private static Player player;
    public static Player GetPlayer { get { return player; } }

    private static new int level;

    [SerializeField]
    private int coinToLevelUp = 5;

    public event Action<int> SpendCoin;

    protected override void Awake()
    {
        base.Awake();

        player = this;
    }

    protected override void Start()
    {
        base.Start();
        LevelUP();
        StartCoroutine(FindNearestEnemy());
    }

    private void LevelUP()
    {
        if (Game.GetCoins >= coinToLevelUp)
        {
            var addLevel = Mathf.FloorToInt(Game.GetCoins / coinToLevelUp);
            SpendCoin?.Invoke(addLevel * coinToLevelUp);
            level += addLevel;
        };

        health = health + (level * 20); // повышение здоровья от изночального

        TakeDamage(0); // обновляет здоровье в UI
    }

    private void Update()
    {
        var vector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Move(vector.normalized);
    }
    

    /// <summary>
    /// Поиск ближайшего врага
    /// </summary>
    private IEnumerator FindNearestEnemy()
    {
        var tempTarget = target;

        while (true)
        {
            if (Enemy.GetLiveEnemies.Count == 0) // прервать поиск если нет врагов
            {
                islookAtTarget = false;
                target = null;
                yield break;
            }

            List<(Enemy enemy, float distance)> enemies = new List<(Enemy, float)>();

            foreach (var enemy in Enemy.GetLiveEnemies)
            {
                var distance = enemy.transform.position - transform.position;
                enemies.Add((enemy, distance.magnitude));
            }

            enemies.Sort((a, b) => { return a.distance > b.distance ? 1 : -1; });

            target = enemies[0].enemy;

            if (tempTarget != target) // если найдена новая цель
                islookAtTarget = false;

            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void Death()
    {
        level = 0;
        base.Death();
    }
}
