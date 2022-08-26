using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObj : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float time;
    [SerializeField] private float timeCounter;
    [SerializeField] private bool useTimer;
    [SerializeField] private bool useSpinner;

    private void Awake()
    {
        timeCounter = time;
    }
    
    void Update()
    {
        Timer();
        Spinner();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }

    private void Timer()
    {
        if (!useTimer)
        {
            return;
        }
        
        timeCounter -= Time.deltaTime;
        if (timeCounter <= 0)
        {
            DestroyObj();
        }
    }

    private void Spinner()
    {
        if (!useSpinner)
        {
            return;
        }
        
        transform.Rotate(0,0, 720f *Time.deltaTime);
    }
    
    public bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }
    
    
    public float GetDamage()
    {
        return damage;
    }

    private void DestroyObj()
    {
        Destroy(gameObject);
    }
}
