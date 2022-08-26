using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCheckPoint : MonoBehaviour
{
    public event EventHandler<SetPlayerCheckpointEventArgs> OnPlayerHitCheckpoint;
    public class SetPlayerCheckpointEventArgs : EventArgs
    {
        public Transform LastPlayerCheckpoint;
    }
    
    private List<CheckPointSingle> checkPointList;
    private int nextCheckPointIndex;
    private void Awake()
    {
        Transform transformCheckPoint = transform.Find("CheckPointContainer");
        checkPointList = new List<CheckPointSingle>();
        foreach (Transform CheckPointSingleTransform in transformCheckPoint)
        {
            CheckPointSingle checkPointSingle = CheckPointSingleTransform.GetComponent<CheckPointSingle>();
            checkPointSingle.SetGameCheckPoint(this);
            checkPointList.Add(checkPointSingle);
        }
    }
    
    public void PlayerThroughCheckpoint(CheckPointSingle checkPoint)
    {
        if (checkPointList.IndexOf(checkPoint) == nextCheckPointIndex)
        {
            OnPlayerHitCheckpoint?.Invoke(this,
                new SetPlayerCheckpointEventArgs{ LastPlayerCheckpoint = checkPointList[nextCheckPointIndex].transform });
            nextCheckPointIndex = (nextCheckPointIndex + 1 ) % checkPointList.Count;
        }
    }
}
