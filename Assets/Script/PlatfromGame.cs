using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatfromGame : MonoBehaviour
{
    [SerializeField] private GameCheckPoint gameCheckPoint;
    [SerializeField] private GamePit gamePit;
    [SerializeField] private Transform playerCheckPoint;
    [SerializeField] private Player player;
    private void Start()
    {
        gameCheckPoint.OnPlayerHitCheckpoint += SetPlayerCheckpoint;
        player.PlayerDeath += PlayerOnSetPosition;
        gamePit.OnPlayerHitPitDamage += PlayerOnSetPosition;
    }

    private void PlayerOnSetPosition(object sender, EventArgs e)
    {
        player.transform.position = playerCheckPoint.position;
    }

    private void SetPlayerCheckpoint(object sender, GameCheckPoint.SetPlayerCheckpointEventArgs e)
    {
        playerCheckPoint = e.LastPlayerCheckpoint.transform.Find("PlayerCheckPoint");
    }
}
