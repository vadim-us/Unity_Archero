using System;
using System.Collections;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    /// <summary>
    /// скорость (растояние в секунду)
    /// </summary>
    [SerializeField]
    protected float moveSpeed = 10f;

    [Space, Header("начальное параметры (будут меняться с уровнем)")]

    /// <summary>
    /// текущее здоровье (будет расти с уровнем)
    /// </summary>
    [SerializeField]
    protected int health = 100;
    public int GetHp { get { return health; } }

    /// <summary>
    /// начальная скорость стрельбы в секундах (будет уменьшатся с уровнем)
    /// </summary>
    [SerializeField, Range(0.5f, 3)]
    protected float shotSpeed = 1f;

    protected Animator animator;

    /// <summary>
    /// начальный урон (будет расти с уровнем)
    /// </summary>
    [SerializeField]
    protected int damage = 10;
    public int GetDamage { get { return damage; } }

    protected int level;
    public int Level { get { return level; } }

    [Space]
    [SerializeField]
    protected GameObject weaponPrefab;

    /// <summary>
    /// текушее оружие
    /// </summary>
    protected Weapon weapon;

    protected Rigidbody rigibody;

    /// <summary>
    /// руки
    /// </summary>
    [SerializeField]
    protected Transform heand;

    /// <summary>
    /// точка для прицела в песонаж
    /// </summary>
    [SerializeField]
    protected Transform targetPoint;
    public Transform GetTargetPoint { get { return targetPoint; } }

    /// <summary>
    /// цель стрельбы
    /// </summary>
    protected Character target;
    public Character SetTarget { set { target = value; } }

    /// <summary>
    /// двигется?
    /// </summary>
    protected bool isMove;
    /// <summary>
    /// смотрит на цель?
    /// </summary>
    protected bool islookAtTarget;
    /// <summary>
    /// стреляет?
    /// </summary>
    protected bool isShot;

    /// <summary>
    /// жив или нет персонаж
    /// </summary>
    protected bool isAlive = true;

    public event Action<Character> Died;
    public event Action<Character, int> Damage;
    public event Action Shot;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rigibody = GetComponent<Rigidbody>();

        if (weaponPrefab != null && heand != null)
        {
            weapon = Instantiate(weaponPrefab, heand).GetComponent<Weapon>();
            weapon.SetOwner = this;
        }

        Game.ClearLevel += Remove;
    }

    protected virtual void Start()
    {
        StartCoroutine(AutoShot());
    }

    protected void Move(Vector3 direction)
    {
        if (direction.magnitude > 0)
        {
            isMove = true;

            Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 500 * Time.deltaTime);

            Vector3 velocity = direction * moveSpeed;
            velocity.y = rigibody.velocity.y;

            rigibody.velocity = velocity;
        }
        else
        {
            rigibody.velocity = Vector3.zero;
            isMove = false;
        }
    }

    /// <summary>
    /// Авто стрельба (считает прошедшее время без выстела, может стрелять сразу как остановится если прошло заданное время и не нужно целиться)
    /// </summary>
    protected IEnumerator AutoShot()
    {
        float wait = 0; // сколько прошло с предыдущего выстрела

        while (true)
        {
            // не двигается, смотрит на цель и может стрелять
            if (!isMove && islookAtTarget && wait >= shotSpeed)
            {
                Shot?.Invoke();
                wait = 0;
            }
            else
            {
                yield return StartCoroutine(LookAtTarget());
            }

            yield return null;
            wait += Time.deltaTime;
        }
    }

    /// <summary>
    /// Поворот в сторону цели
    /// </summary>
    protected IEnumerator LookAtTarget()
    {
        while (true)
        {
            // прервать поворот если начал движение или смотрит на цель
            if (isMove || islookAtTarget)
            {
                yield break;
            }

            if (target != null)
            {
                var target = this.target.transform.position;
                target.y = 0;

                var character = transform.position;
                character.y = 0;

                var targetVector = target - character;

                Quaternion toRotation = Quaternion.LookRotation(targetVector, transform.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 500 * Time.deltaTime);

                if (transform.rotation == toRotation)
                {
                    islookAtTarget = true;
                    weapon.transform.LookAt(this.target.targetPoint);
                }
            }

            yield return null;
        }
    }

    protected virtual void Remove()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Arrow>())
        {
            TakeDamage(collision.gameObject.GetComponent<Arrow>().Damage);
        }

        if (this is Player)
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null && enemy.isAlive)
                TakeDamage(30);
        }
    }

    protected virtual void TakeDamage(int damage)
    {
        if (isAlive)
        {
            health -= damage;

            if (animator)
                animator.SetTrigger("damage");

            Damage?.Invoke(this, damage);
            if (health <= 0)
                Death();
        }
    }

    protected virtual void Death()
    {
        isAlive = false;

        rigibody.constraints = RigidbodyConstraints.None;

        enabled = false;
        StopAllCoroutines();

        Died?.Invoke(this);
    }
}
