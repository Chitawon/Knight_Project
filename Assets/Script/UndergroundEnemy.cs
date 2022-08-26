using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundEnemy : Enemy
{
    [SerializeField] private float time;
    [SerializeField] private float timeCounter;
    [SerializeField] private bool isAttack;
    [SerializeField] private LayerMask blockMask;

    protected override void Awake()
    {
        base.Awake();
        timeCounter = time;
    }
    
    protected override void Update()
    {
        CountDownToAttack();
    }

    private void CountDownToAttack()
    {
        timeCounter -= Time.deltaTime;
        if (timeCounter <= 0)
        {
            isAttack = !isAttack;
            Attack();
            timeCounter = time;
        }
    }
    
    private void Attack()
    {
        Vector2 enemyVelocity = Vector2.up * moveSpeed;
        if (!isAttack)
        {
            enemyVelocity *= -1;
        }
        enemyRigidbody.velocity = enemyVelocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == blockMask.value)
        {
            enemyRigidbody.velocity = Vector2.zero;
            timeCounter = time;
        }
    }

}
