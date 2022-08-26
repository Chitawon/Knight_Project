using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D _oneWayPlatformsEffector;
    [SerializeField] [Range(0, 1f)] private float dropDownTime = 0.5f;
    [SerializeField] private float timeCounter;
    [SerializeField] private bool platfromeOffsetFlip;
    
    void Awake()
    {
        _oneWayPlatformsEffector = GetComponent<PlatformEffector2D>();
    }
    
    void Update()
    {
        TimeCheck();
        PlatformRotation();
    }

    private void TimeCheck()
    {
        timeCounter -= Time.deltaTime;
    }

    private void PlatformRotation()
    {
        if (Input.GetAxis("Vertical") < 0 && Input.GetButtonDown("Jump"))
        {
            timeCounter = dropDownTime;
        }
        
        if (timeCounter > 0 && !platfromeOffsetFlip)
        { 
            _oneWayPlatformsEffector.rotationalOffset = 180f;
            platfromeOffsetFlip = !platfromeOffsetFlip;

        }
        else if(timeCounter <= 0 && platfromeOffsetFlip)
        {
            _oneWayPlatformsEffector.rotationalOffset = 0f;
            platfromeOffsetFlip = !platfromeOffsetFlip;
        }
    }
}
