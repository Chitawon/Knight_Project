using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    [SerializeField] private LayerMask groundMask;
    
    protected override void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        Vector2 enemyVelocity = Vector2.right * moveSpeed;
        if (!IsFacingRight())
        {
            enemyVelocity *= -1;
        }
        enemyRigidbody.velocity = enemyVelocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == enemyRigidbody.gameObject.layer)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.velocity.x)), 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == groundMask.value)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.velocity.x)), 1f);
        }
    }
}
