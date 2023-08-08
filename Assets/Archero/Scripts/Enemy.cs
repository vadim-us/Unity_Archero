using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Enemy : Character
{
    [SerializeField]
    private int healthUpdate = 10;
    [SerializeField]
    private float shootUpdate = 0.05f;

    /// <summary>
    /// мертвые враги
    /// </summary>
    private static List<Enemy> deadEnemies = new List<Enemy>();

    /// <summary>
    /// живые враги
    /// </summary>
    private static List<Enemy> liveEnemies = new List<Enemy>();
    public static List<Enemy> GetLiveEnemies { get { return liveEnemies; } }

    /// <summary>
    ///  дистанция перемещения в метрах
    /// </summary>
    [SerializeField]
    protected float moveDistance = 3f;

    /// <summary>
    /// время неподвижности в секундах
    /// </summary>
    [SerializeField]
    private float notMoveTime = 2f;

    [SerializeField]
    private Transform hp;

    protected float distanceTraveled;

    protected bool isCanMove;

    protected override void Awake()
    {
        base.Awake();

        liveEnemies.Add(this);

        level = Random.Range(0, Game.GetLevel + 1);
    }

    private void UpdateHpText()
    {
        hp.Find("health").GetComponentInChildren<TextMeshPro>().text = health.ToString();
    }

    protected override void Start()
    {
        base.Start();
        LevelUP();
        target = Player.GetPlayer;
        StartCoroutine(FindPlayerPosition());
        StartCoroutine(Wait());
    }



    private void LevelUP()
    {
        shotSpeed = shotSpeed - (level * shootUpdate);
        health = health + (level * healthUpdate);

        UpdateHpText();
    }

    protected virtual void Update()
    {
        RotateHpText();
    }

    protected Vector3 PlayerVector()
    {
        var target = Player.GetPlayer.transform.position;
        target.y = 0;

        var character = transform.position;
        character.y = 0;

        return target - character;
    }

    /// <summary>
    /// Ждать на месте
    /// </summary>
    protected IEnumerator Wait()
    {
        isCanMove = false;
        yield return new WaitForSeconds(notMoveTime);
        isCanMove = true;
    }

    private void RotateHpText()
    {
        var vector = Camera.main.transform.forward;
        hp.rotation = Quaternion.LookRotation(-vector);
    }

    /// <summary>
    /// Позиция игрока
    /// </summary>
    private IEnumerator FindPlayerPosition()
    {
        var tempTargetPosition = target.transform.position;

        while (true)
        {
            if (target == null) // прервать поиск если игрок убит
            {
                islookAtTarget = false;
                yield break;
            }

            if (tempTargetPosition != target.transform.position) // если игрок переместился
                islookAtTarget = false;

            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        UpdateHpText();
        hp.Find("damage").GetComponentInChildren<TextMeshPro>().text = (-damage).ToString();
    }

    protected override void Remove()
    {
        liveEnemies.Remove(this);
        deadEnemies.Remove(this);
        base.Remove();
    }

    protected override void Death()
    {
        hp.Find("health").gameObject.SetActive(false);
        liveEnemies.Remove(this);
        deadEnemies.Add(this);
        base.Death();
    }
}
