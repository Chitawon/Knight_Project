using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : Enemy
{
    [SerializeField] private GameObject Arrow;
    [SerializeField] private float objSpeedX;
    [SerializeField] private float time;
    [SerializeField] private float timeCounter;
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
        base.Update();
        CountDownToAttack();
    }

    private void CountDownToAttack()
    {
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
        Vector2 throwX = Vector2.right * objSpeedX;
        if (!IsFacingRight())
        {
            throwX *= -1;
            Vector3 throwArrowScale = throwArrow.transform.localScale;
            throwArrowScale.x *= -1;
            throwArrow.transform.localScale = throwArrowScale;
        }
        throwArrow.GetComponent<Rigidbody2D>().velocity = throwX;
    }

}
