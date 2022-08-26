using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePit : MonoBehaviour
{
    public event EventHandler OnPlayerHitPitDamage;
    private List<PitDamage> _pitDamageList;
    private void Awake()
    {
        Transform transformPitDamage = transform.Find("PitDamageContainer");
        _pitDamageList = new List<PitDamage>();
        foreach (Transform pitDamageTransform in transformPitDamage)
        {
            PitDamage pitDamageSingle = pitDamageTransform.GetComponent<PitDamage>();
            pitDamageSingle.SetGamePit(this);
            _pitDamageList.Add(pitDamageSingle);
        }
    }
    
    public void PlayerThroughPitDamage(PitDamage pitDamage)
    {
        OnPlayerHitPitDamage?.Invoke(this, EventArgs.Empty);
    }
}
