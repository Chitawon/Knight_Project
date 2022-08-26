using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [SerializeField] private GameObject Arrow;
    [SerializeField] private bool canShoot;
    [SerializeField] private bool canMove;
    [SerializeField] private float objSpeedY;
    [SerializeField] private float time;
    [SerializeField] private float timeCounter;
    [SerializeField] private LayerMask blockMask;
    protected override void Awake()
    {
        base.Awake();
        timeCounter = time;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        CountDownToAttack();
    }
    
    protected override void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        if (!canMove)
        {
            return;
        }
        
        Vector2 enemyVelocity = Vector2.right * moveSpeed;
        if (!IsFacingRight())
        {
            enemyVelocity *= -1;
        }
        enemyRigidbody.velocity = enemyVelocity;
    }

    private void CountDownToAttack()
    {
        if (!canShoot)
        {
            return;
        }
        
        timeCounter -= Time.deltaTime;
        if (timeCounter <= 0)
        {
            Attack();
            timeCounter = time;
        }
    }

    private void Attack()
    {
        GameObject throwArrow = Instantiate(Arrow, transform.position, Arrow.transform.rotation);
        Vector2 throwY = Vector2.down * objSpeedY;
        throwArrow.GetComponent<Rigidbody2D>().velocity = throwY;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == blockMask.value)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.velocity.x)), 1f);
        }
    }
}
