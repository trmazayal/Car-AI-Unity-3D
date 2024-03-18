using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class TrackCheckpointManager : MonoBehaviour
{
    //public event EventHandler OnPlayerCorrectCheckpoint;
    //public event EventHandler OnPlayerWrongCheckpoint;
 
    public event EventHandler<CarCheckPointEventArgs> OnPlayerCorrectCheckpoint;
    public event EventHandler<CarCheckPointEventArgs> OnPlayerWrongCheckpoint;
 
    [SerializeField] private List<Transform> RacersTransformList;
    private List<Checkpoint> checkpointList;
    private List<int> nextCheckpointIndexList;
       
    private void Awake()
    {
        checkpointList = new List<Checkpoint>(GetComponentsInChildren<Checkpoint>());
    }
 
    public void CarPassedCheckpoint(Checkpoint checkpoint, Transform racerTransform)
    {
        int nextCheckpointIndex = nextCheckpointIndexList[RacersTransformList.IndexOf(racerTransform)];
 
        CarCheckPointEventArgs e = new CarCheckPointEventArgs
        {
            carTransform = racerTransform
        };
 
        if (checkpointList.IndexOf(checkpoint) == nextCheckpointIndex)
        {
            //Debug.Log("Correct Checkpoint");
            nextCheckpointIndexList[RacersTransformList.IndexOf(racerTransform)] = (nextCheckpointIndex + 1) % checkpointList.Count;
 
            //CarCheckPointEventArgs e = new CarCheckPointEventArgs
            //{
            //    carTransform = racerTransform
            //};
            OnPlayerCorrectCheckpoint?.Invoke(this, e);
        }
 
        else
        {
            //Debug.Log("Wrong Checkpoint");
            OnPlayerWrongCheckpoint?.Invoke(this, e);
        }
 
    }
 
    public class CarCheckPointEventArgs : EventArgs
    {
        public Transform carTransform { get; set; }
    }
 
    public void ResetCarChecpoint(Transform racerTransform)
    {
        nextCheckpointIndexList[RacersTransformList.IndexOf(racerTransform)] = 0;
    }
 
    public Transform GetNextCheckpoint(Transform racerTransform)
    {
        int nextCheckpointIndex = nextCheckpointIndexList[RacersTransformList.IndexOf(racerTransform)];
 
        Transform CheckpointTransforms = transform.Find("Checkpoints");
 
        Transform NextCheckpointTransform = null;
 
        int count = 0;
 
        foreach (Transform CheckpointTransform in CheckpointTransforms)
        {
            if (count == nextCheckpointIndex)
            {
                NextCheckpointTransform = CheckpointTransform;
                //Debug.Log(NextCheckpointTransform.gameObject.name);
                break;
            }
 
            else
                count++;
        }
 
        return NextCheckpointTransform;
 
    }
}