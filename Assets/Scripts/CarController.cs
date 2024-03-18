using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CarController : MonoBehaviour
{
    private const string HORIZONTAL_MOTION = "Horizontal";
    private const string VERTICAL_MOTION = "Vertical";

    private SpawnPointManager _spawnPointManager;
    public float vehicleSpeed;

    public float horizontalInput;
    public float verticalInput;

    public Rigidbody rb;
 
    public bool bIsVehicleBraking;
    public float currentBrakingForce;
    public float currentSteeringAngle;
 
    [SerializeField] public float vehicleMotorForce;
    [SerializeField] public float vehicleBrakingForce;
    [SerializeField] public float vehicleSteeringAngle;
 
 
    [SerializeField] private WheelCollider FrontRightWheelCollider;
    [SerializeField] private WheelCollider FrontLeftWheelCollider;
    [SerializeField] private WheelCollider RearRightWheelCollider;
    [SerializeField] private WheelCollider RearLeftWheelCollider;
 
    [SerializeField] private Transform FrontRightWheelTransform;
    [SerializeField] private Transform FrontLeftWheelTransform;
    [SerializeField] private Transform RearRightWheelTransform;
    [SerializeField] private Transform RearLeftWheelTransform;
 
 
   public void Awake()
   {
      _spawnPointManager = FindObjectOfType<SpawnPointManager>();
   }
    
    private void FixedUpdate()
    {
        GetMovementInput();
        VehicleMotorHandling();
        VehicleSteeringHandling();
        VehicleWheelAnimationUpdate();
    }
 
    public void GetMovementInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL_MOTION);
        verticalInput = Input.GetAxis(VERTICAL_MOTION);
        bIsVehicleBraking = Input.GetKey(KeyCode.Space);
 
    }
 
    private void VehicleMotorHandling()
    {
        FrontLeftWheelCollider.motorTorque = verticalInput * vehicleMotorForce;
        FrontRightWheelCollider.motorTorque = verticalInput * vehicleMotorForce;

        if (bIsVehicleBraking)
        {
            currentBrakingForce = vehicleBrakingForce;
        }
        else
        {
            currentBrakingForce = 0f;
        }
        ApplyBrakingForceToVehicle();
    }
 
    private void ApplyBrakingForceToVehicle()
    {
        FrontLeftWheelCollider.brakeTorque = currentBrakingForce;
        FrontRightWheelCollider.brakeTorque = currentBrakingForce;
        RearLeftWheelCollider.brakeTorque = currentBrakingForce;
        RearRightWheelCollider.brakeTorque = currentBrakingForce;
    }
 
    public void StopVehicleCompletely()
    {
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
 
    private void VehicleSteeringHandling()
    {
        currentSteeringAngle = vehicleSteeringAngle * horizontalInput;
        FrontLeftWheelCollider.steerAngle = currentSteeringAngle;
        FrontRightWheelCollider.steerAngle = currentSteeringAngle;
    }
 
    private void VehicleWheelAnimationUpdate()
    {
        UpdateWheelOrientation(FrontLeftWheelCollider, FrontLeftWheelTransform);
        UpdateWheelOrientation(FrontRightWheelCollider, FrontRightWheelTransform);
        UpdateWheelOrientation(RearLeftWheelCollider, RearLeftWheelTransform);
        UpdateWheelOrientation(RearRightWheelCollider, RearRightWheelTransform);
    }
 
    private void UpdateWheelOrientation(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 wheelposition;
        Quaternion wheelrotation;
        wheelCollider.GetWorldPose(out wheelposition, out wheelrotation);
        wheelTransform.position = wheelposition;
        wheelTransform.rotation = wheelrotation;
 
    }
   
   public void Respawn()
   {
      Vector3 pos = _spawnPointManager.SelectRandomSpawnpoint();
      rb.MovePosition(pos);
      transform.position = pos - new Vector3(0, 0.4f, 0);
   }

}
 