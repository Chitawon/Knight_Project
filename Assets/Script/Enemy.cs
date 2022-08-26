using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float damege;
    [SerializeField] protected bool knockBack;
    
    protected Rigidbody2D enemyRigidbody;
    protected Animator enemyAnimator;
    
    protected virtual void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        //
    }
    
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        
    }

    protected virtual void Move()
    {   
        //
    }
    
    public bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    public float GetDamege()
    {
        return damege;
    }

    public bool HaveKnockBack()
    {
        return knockBack;
    }

    public void ProcessHit(float Damage)
    {
        health -= Damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
