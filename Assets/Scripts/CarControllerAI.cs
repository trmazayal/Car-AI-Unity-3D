using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
 
public class CarControllerAI : Agent
{
    [SerializeField] private TrackCheckpointManager trackCheckpointManager;
    [SerializeField] private Transform carSpawnPoint;
 
    [SerializeField] private CarController carController;
 
    private void Awake()
    {
        carController = GetComponent<CarController>();
    }
 
    private void Start()
    {
        // trackCheckpointManager.OnPlayerCorrectCheckpoint += CheckpointTracker_OnCorrectCheckpoint;
        // trackCheckpointManager.OnPlayerWrongCheckpoint += CheckpointTracker_OnWrongCheckpoint;
    }
 
    // private void CheckpointTracker_OnCorrectCheckpoint(object Sender, TrackCheckpointManager.CarCheckPointEventArgs e)
    // {
    //     //Debug.Log(e.carTransform.gameObject.name);
    //     if (e.carTransform == transform)
    //     {
    //         AddReward(1f);
    //     }
    // }
 
    // private void CheckpointTracker_OnWrongCheckpoint(object Sender, TrackCheckpointManager.CarCheckPointEventArgs e)
    // {
    //     //Debug.Log(e.carTransform.gameObject.name);
    //     if (e.carTransform == transform)
    //     {
    //         AddReward(-1f);
    //     }
    // }
 
    public override void OnEpisodeBegin()
    {
        //transform.position = carSpawnPoint.position;
        //transform.position = carSpawnPoint.position + new Vector3(20f, 0, Random.Range(-5f, 5f));
        transform.position = carSpawnPoint.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        if (transform.rotation.eulerAngles.y > -90f || transform.rotation.eulerAngles.y < -90f)
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        transform.forward = carSpawnPoint.forward;
        // trackCheckpointManager.ResetCarChecpoint(transform);
        carController.StopVehicleCompletely();
    }
 
    public override void CollectObservations(VectorSensor sensor)
    {
        // Vector3 nextCheckpoint = trackCheckpointManager.GetNextCheckpoint(transform).transform.forward;
        // float DirectionDot = Vector3.Dot(transform.forward, nextCheckpoint);
        // sensor.AddObservation(DirectionDot);

        sensor.AddObservation(transform.forward);
        sensor.AddObservation(transform.right);
        sensor.AddObservation(transform.position);
        // Vector3 nextCheckpoint = trackCheckpointManager.GetNextCheckpoint(transform).transform.position;
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
 
        transform.position += transform.forward * forwardAction * carController.vehicleSpeed * Time.deltaTime;
        transform.Rotate(transform.up, steeringAction * carController.vehicleSteeringAngle * Time.deltaTime);
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
 
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            AddReward(-0.5f);
        }
    }
 
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            AddReward(-0.1f);
        }
    }
}
 