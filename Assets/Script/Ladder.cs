using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private PlatformEffector2D oneWayPlatformsEffector;
    private float currentEffector;
    private float time = 1.5f;
    [SerializeField] private float timeCounter;

    // Start is called before the first frame update
    void Start()
    {
        oneWayPlatformsEffector = GetComponent<PlatformEffector2D>();
        currentEffector = oneWayPlatformsEffector.rotationalOffset;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CountdownToReset();
        PlatformRotation();
    }

    private void CheckInput()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            currentEffector = 0f;
            timeCounter = time;
        }
        
        if (Input.GetAxis("Vertical") < 0)
        {
            currentEffector = 180f;
            timeCounter = time;
        }
    }

    private void CountdownToReset()
    {
        timeCounter -= Time.deltaTime;
        if (timeCounter <= 0)
        {
            currentEffector = 0f;
        }
    }

    private void PlatformRotation()
    {
        if ((int) oneWayPlatformsEffector.rotationalOffset != (int) currentEffector)
        {
            oneWayPlatformsEffector.rotationalOffset = currentEffector;
        }
    }
}
