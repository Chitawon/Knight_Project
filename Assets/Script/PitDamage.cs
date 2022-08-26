using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitDamage : Enemy
{
    private GamePit _gamePit;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<Player>(out Player player))
        {
            _gamePit.PlayerThroughPitDamage(this);
        }
    }

    protected override void Die()
    {
        return;
    }

    public void SetGamePit(GamePit gamePit)
    {
        this._gamePit = gamePit;
    }
}
