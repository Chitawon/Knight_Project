using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeEnemy : Enemy
{
    [SerializeField] private GameObject Axe;
    [SerializeField] private float objSpeedX;
    [SerializeField] private float objSpeedY;
    [SerializeField] private float time;
    [SerializeField] private float timeCounter;
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
            Attack();
            timeCounter = time;
        }
    }

    private void Attack()
    {
        GameObject throwAxe = Instantiate(Axe, transform.position, Quaternion.identity);
        Vector2 throwX = Vector2.right * objSpeedX;
        if (!IsFacingRight())
        {
            throwX *= -1;
            Vector3 throwAxeScale = throwAxe.transform.localScale;
            throwAxeScale.x *= -1;
            throwAxe.transform.localScale = throwAxeScale;
        }
        Vector2 throwY = Vector2.up * objSpeedY;
        Vector2 throwFroce = throwX + throwY;
        throwAxe.GetComponent<Rigidbody2D>().AddForce(throwFroce, ForceMode2D.Impulse);
    }

}
