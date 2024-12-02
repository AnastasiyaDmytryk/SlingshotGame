using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Public variables for tuning
    public bool isMovementAllowed = false; // Indicates if the car is allowed to move

    public float motorForce = 1000f;
    public float maxSteeringAngle = 15f;   // Reduced for better control
    public float brakeForce = 3000f;
    public float downforce = 75f;          // Increased for stability

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public GameObject frontLeftWheelVisual;
    public GameObject frontRightWheelVisual;
    public GameObject rearLeftWheelVisual;
    public GameObject rearRightWheelVisual;

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBrakeForce;
    private bool isBraking;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            UnityEngine.Debug.LogError("Rigidbody component missing! Please add a Rigidbody to your car object.");
        }

        // Adjust the center of mass
        rb.centerOfMass = new Vector3(0, -1.0f, 0); // Adjusted for better balance

        // Set Rigidbody mass and drag
        rb.mass = 1500f;
        rb.drag = 0.2f;          // Increased for more stability
        rb.angularDrag = 7f;     // Increased to reduce spinning out

        // Adjust wheel friction and suspension settings
        AdjustWheel(frontLeftWheelCollider);
        AdjustWheel(frontRightWheelCollider);
        AdjustWheel(rearLeftWheelCollider);
        AdjustWheel(rearRightWheelCollider);
    }

    private void FixedUpdate()
    {
        if (!isMovementAllowed)
    {
        ApplyBraking(); // Apply brakes to stop the car
        return;
    }
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheelVisuals();
        ApplyDownforce();
        UprightCar();
        TractionControl();
        StabilizeCar(); // Anti-roll bar function
    }

    // Get player inputs
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    // Apply motor force to all four wheels and braking
    private void HandleMotor()
    {
        float slopeFactor = 1f + GetSlopeFactor(); // Increase torque on slopes

        frontLeftWheelCollider.motorTorque = verticalInput * motorForce * slopeFactor;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce * slopeFactor;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce * slopeFactor;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce * slopeFactor;

        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();
    }

    private void ApplyBraking()
    {
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteeringAngle * horizontalInput;

        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheelVisuals()
    {
        UpdateWheelPosition(frontLeftWheelCollider, frontLeftWheelVisual);
        UpdateWheelPosition(frontRightWheelCollider, frontRightWheelVisual);
        UpdateWheelPosition(rearLeftWheelCollider, rearLeftWheelVisual);
        UpdateWheelPosition(rearRightWheelCollider, rearRightWheelVisual);
    }

    private void UpdateWheelPosition(WheelCollider collider, GameObject visualWheel)
    {
        if (visualWheel == null)
            return;

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    private void ApplyDownforce()
    {
        rb.AddForce(-transform.up * downforce * rb.velocity.magnitude);
    }

    private void AdjustWheel(WheelCollider wheel)
    {
        AdjustWheelFriction(wheel);
        AdjustSuspension(wheel);
    }

    private void AdjustWheelFriction(WheelCollider wheel)
    {
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        forwardFriction.extremumSlip = 0.2f;
        forwardFriction.extremumValue = 1f;
        forwardFriction.asymptoteSlip = 0.5f;
        forwardFriction.asymptoteValue = 0.75f;
        forwardFriction.stiffness = 2f;
        wheel.forwardFriction = forwardFriction;

        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.extremumSlip = 0.15f;
        sidewaysFriction.extremumValue = 1f;
        sidewaysFriction.asymptoteSlip = 0.4f;
        sidewaysFriction.asymptoteValue = 0.8f;
        sidewaysFriction.stiffness = 3.5f;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    private void AdjustSuspension(WheelCollider wheel)
    {
        JointSpring suspensionSpring = wheel.suspensionSpring;
        suspensionSpring.spring = 30000f;
        suspensionSpring.damper = 6000f;
        suspensionSpring.targetPosition = 0.5f;
        wheel.suspensionSpring = suspensionSpring;

        wheel.suspensionDistance = 0.25f;
    }

    private void UprightCar()
    {
        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            rb.AddTorque(transform.right * 10f, ForceMode.Acceleration);
        }
    }

    private void TractionControl()
    {
        WheelHit wheelHit;
        float slipLimit = 0.25f;

        WheelCollider[] wheels = { frontLeftWheelCollider, frontRightWheelCollider, rearLeftWheelCollider, rearRightWheelCollider };

        foreach (WheelCollider wheel in wheels)
        {
            wheel.GetGroundHit(out wheelHit);
            if (Mathf.Abs(wheelHit.forwardSlip) >= slipLimit)
            {
                wheel.motorTorque *= 0.8f;
            }
        }
    }

    private float GetSlopeFactor()
    {
        float angle = Vector3.Angle(transform.up, Vector3.up);
        return Mathf.Clamp01(angle / 45f);
    }

    private void StabilizeCar()
    {
        ApplyAntiRoll(frontLeftWheelCollider, frontRightWheelCollider);
        ApplyAntiRoll(rearLeftWheelCollider, rearRightWheelCollider);
    }

    private void ApplyAntiRoll(WheelCollider wheelL, WheelCollider wheelR)
    {
        WheelHit hit;
        float travelL = 1f;
        float travelR = 1f;

        if (wheelL.GetGroundHit(out hit))
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;

        if (wheelR.GetGroundHit(out hit))
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;

        float antiRollForce = (travelL - travelR) * 5000f;

        if (travelL < 1f)
            rb.AddForceAtPosition(wheelL.transform.up * -antiRollForce, wheelL.transform.position);

        if (travelR < 1f)
            rb.AddForceAtPosition(wheelR.transform.up * antiRollForce, wheelR.transform.position);
    }

    private void OnDrawGizmos()
    {
        if (rb != null)
        {
            Gizmos.color = Color.red;
            Vector3 worldCenterOfMass = transform.TransformPoint(rb.centerOfMass);
            Gizmos.DrawSphere(worldCenterOfMass, 0.1f);
        }
    }
}
