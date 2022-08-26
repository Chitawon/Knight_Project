using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSingle : MonoBehaviour
{
    private GameCheckPoint gameCheckPoint;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<Player>(out Player player))
        {
            gameCheckPoint.PlayerThroughCheckpoint(this);
        }
    }

    public void SetGameCheckPoint(GameCheckPoint gameCheckPoint)
    {
        this.gameCheckPoint = gameCheckPoint;
    }
    
}
