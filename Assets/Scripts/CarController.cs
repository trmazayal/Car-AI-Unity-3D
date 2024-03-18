using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CarController : MonoBehaviour
{
    private const string HORIZONTAL_MOTION = "Horizontal";
    private const string VERTICAL_MOTION = "Vertical";

    public float vehicleSpeed;

    public float horizontalInput;
    public float verticalInput;
 
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
 
 
 
    private void FixedUpdate()
    {
        GetMovementInput();
        VehicleMotorHandling();
        VehicleSteeringHandling();
        VehicleWheelAnimationUpdate();
    }
 
    //private void FixedUpdate()
    //{
    //    GetMovementInput();
    //    VehicleMotorHandling();
    //    VehicleSteeringHandling();
    //    VehicleWheelAnimationUpdate();
    //}
 
    public void GetMovementInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL_MOTION);
        verticalInput = Input.GetAxis(VERTICAL_MOTION);
        //bIsVehicleBraking = Input.GetKey(KeyCode.Space);
 
    }
 
    private void VehicleMotorHandling()
    {
        FrontLeftWheelCollider.motorTorque = verticalInput * vehicleMotorForce;
        FrontRightWheelCollider.motorTorque = verticalInput * vehicleMotorForce;
        //currentBrakingForce = bIsVehicleBraking ? vehicleBrakingForce : 0f;
        //if (bIsVehicleBraking)
        //{
        //    ApplyBrakingForceToVehicle();
        //}
        if (Input.GetKey(KeyCode.Space))
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
        //FrontLeftWheelCollider.brakeTorque = vehicleBrakingForce * 2;
        //FrontRightWheelCollider.brakeTorque = vehicleBrakingForce * 2;
        //RearLeftWheelCollider.brakeTorque = vehicleBrakingForce * 2;
        //RearRightWheelCollider.brakeTorque = vehicleBrakingForce * 2;
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
}
 