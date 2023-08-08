
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [Header("Префабы")]
    [SerializeField]
    private List<GameObject> enemiesPrefab;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject shildPrefab;

    [SerializeField]
    private GameObject dropPrefab;

    [Space]
    [SerializeField]
    private Gate gate;

    [SerializeField]
    private BoxCollider enemySpawnZone;
    private (float left, float right, float up, float down) spawnZone;

    [SerializeField]
    private UI ui;

    /// <summary>
    /// текущий уровень
    /// </summary>
    private static int level = 1;

    public static int GetLevel { get { return level; } }

    /// <summary>
    /// собранные монеты
    /// </summary>
    private static int coins;

    public static int GetCoins { get { return coins; } }



    public static event Action ClearLevel;
    public static event Action<bool, gameState> Menu;
    public static event Action OpenGate;

    public enum gameState
    {
        start,
        win,
        end
    }
    private enum spawnType
    {
        player,
        enemy,
        shild
    }


    private void Awake()
    {
        SpawnZone();

        gate.Enter += () => { StartCoroutine(Win()); };
        ui.ResetLevel += ResetLevel;
    }

    private void SpawnZone()
    {
        var zone = enemySpawnZone.transform.position;
        var scale = enemySpawnZone.transform.localScale;

        var left = zone.x - scale.x / 2;
        var right = zone.x + scale.x / 2;

        var up = zone.z - scale.z / 2;
        var down = zone.z + scale.z / 2;

        spawnZone = (left, right, up, down);
    }

    private void Start()
    {
        Menu?.Invoke(true, gameState.start);
    }

    private void ResetLevel()
    {
        ClearLevel?.Invoke();
        ClearLevel = null;

        // врагов на уровне
        int graundEnemy = level + 1;
        int flyEnemy = graundEnemy - 1;

        foreach (var enemy in enemiesPrefab)
        {
            for (int i = 0; i < (enemy.tag == "FlyEnemy" ? flyEnemy : graundEnemy); i++)
            {
                Spawn(spawnType.enemy, enemy, RandomPosition(enemy.tag == "FlyEnemy"));
            }
        }

        for (int i = 0; i < level; i++)
        {
            Spawn(spawnType.shild, shildPrefab, RandomPosition(false));
        }

        Spawn(spawnType.player, playerPrefab, new Vector3(0, 0, -13));
    }

    private Vector3 RandomPosition(bool isfly)
    {
        var x = Random.Range(spawnZone.left, spawnZone.right);
        var y = isfly ? 6 : 0;
        var z = Random.Range(spawnZone.up, spawnZone.down);

        return new Vector3(x, y, z);
    }

    private void Spawn(spawnType type, GameObject prefab, Vector3 position)
    {
        var obj = Instantiate(prefab, position, Quaternion.identity);

        switch (type)
        {
            case spawnType.player:
                var player = obj.GetComponent<Player>();
                player.Died += PlayerDied;
                player.Damage += PlayerDamage;
                player.SpendCoin += SpendCoin;
                break;
            case spawnType.enemy:
                var enemy = obj.GetComponent<Enemy>();
                enemy.Died += EnemyDied;
                break;
            case spawnType.shild:
                break;
            default:
                break;
        }
    }

    private void SpendCoin(int coin)
    {
        coins -= coin;
        ui.UpdateCoin(coins);
    }


    private void PlayerDamage(Character obj, int damage)
    {
        ui.UpdateHp(obj.GetHp, damage);
    }

    private void PlayerDied(Character obj)
    {
        level = 1;
        coins = 0;
        ui.UpdateCoin(coins);
        Menu?.Invoke(true, gameState.end);
    }

    private void EnemyDied(Character obj)
    {
        Enemy enemy = (Enemy)obj;
        SpawnDrop(enemy);

        if (Enemy.GetLiveEnemies.Count == 0)
        {
            Shild.GetShilds.ForEach((shild) => { shild.GetComponent<Rigidbody>().isKinematic = false; });
            OpenGate?.Invoke();
        }
    }

    private void SpawnDrop(Enemy enemy)
    {
        var coin = Instantiate(dropPrefab).GetComponent<Coin>();
        coin.transform.position = enemy.GetTargetPoint.position;
        coin.Value = enemy.Level+1;
        coin.ColectCoin += ColectCoin;
    }

    private void ColectCoin(Coin coin)
    {
        coins += coin.Value;
        ui.UpdateCoin(coins);
    }

    private IEnumerator Win()
    {
        Player.GetPlayer.enabled = false;
        yield return new WaitForSeconds(1);

        level++;
        Menu?.Invoke(true, gameState.win);
    }
}
