using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGround : MonoBehaviour
{
    [SerializeField] private float timeGround;
    [SerializeField] private float timeCouter;
    [SerializeField] private bool startCounter;
    [SerializeField] private LayerMask playerMask;
    private BoxCollider2D timeCollider;
    private SpriteRenderer groundSprite;
    //[SerializeField] private float 
    
    void Awake()
    {
        timeCouter = timeGround;
        timeCollider = GetComponent<BoxCollider2D>();
        groundSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CountdownGround();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (1 << col.gameObject.layer == playerMask.value && !startCounter)
        {
            startCounter = !startCounter;
        }
    }

    private void CountdownGround()
    {
        if (!startCounter)
        {
            return;
        }
        
        timeCouter -= Time.deltaTime;
        if (timeCouter <= 0)
        {
            SetCollider();
        }
    }

    private void SetCollider()
    {
        if (!timeCollider.enabled)
        {
            startCounter = !startCounter;
        }

        groundSprite.enabled = !groundSprite.enabled;
        timeCollider.enabled = !timeCollider.enabled;
        timeCouter = timeGround;
    }
}
