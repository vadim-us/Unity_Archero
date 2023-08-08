using UnityEngine;

public class FlyEnemy : Enemy
{
    protected override void Update()
    {
        base.Update();
        Move();
    }

    private void Move()
    {
        if (isCanMove)
        {
            Move(PlayerVector().normalized);
            isMove = true;

            distanceTraveled += moveSpeed * Time.deltaTime;
            if (distanceTraveled >= moveDistance || PlayerVector().magnitude < 10)
            {
                rigibody.velocity = Vector3.zero;
                distanceTraveled = 0;
                isMove = false;
                StartCoroutine(Wait());
            }
        }
    }
}