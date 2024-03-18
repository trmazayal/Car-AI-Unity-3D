using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
 
public class CarControllerAI : Agent
{
    [SerializeField] private CheckpointManager _checkpointManager;
    [SerializeField] private Transform carSpawnPoint;
    [SerializeField] private CarController _carController;
 
    private void Awake()
    {
        _carController = GetComponent<CarController>();
    }
 
    public override void OnEpisodeBegin()
    {
      _checkpointManager.ResetCheckpoints();
      _carController.Respawn();
    }
 
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 diff = _checkpointManager.nextCheckPointToReach.transform.position - transform.position;
        sensor.AddObservation(diff / 20f);
        AddReward(-0.001f);
    }
 
    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardAction = 0f;
        float steeringAction = 0f;
 
        int forwardMovementAction = actions.DiscreteActions[0];
        switch (forwardMovementAction)
        {
            case 1:
                forwardAction = 1f;
                break;
            case 2:
                forwardAction = -1f;
                break;
        }
 
        int steeringMovementAction = actions.DiscreteActions[1];
        switch (steeringMovementAction)
        {
            case 1:
                steeringAction = 1f;
                break;
            case 2:
                steeringAction = -1f;
                break;
        }
 
        transform.position += transform.forward * forwardAction * _carController.vehicleSpeed * Time.deltaTime;
        transform.Rotate(0f, steeringAction * _carController.vehicleSteeringAngle * Time.deltaTime,0f);
    }
 
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardMovementAction = 0;
        if (Input.GetKey(KeyCode.W)) forwardMovementAction = 1;
        if (Input.GetKey(KeyCode.S)) forwardMovementAction = 2;
 
        int steeringMovementAction = 0;
        if (Input.GetKey(KeyCode.D)) steeringMovementAction = 1;
        if (Input.GetKey(KeyCode.A)) steeringMovementAction = 2;
 
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardMovementAction;
        discreteActions[1] = steeringMovementAction;
    }
}
 