using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Added for the New Input System

public class CarControllerFinal : MonoBehaviour
{
    [Space(20)]
    [Space(10)]
    [Range(20, 190)]
    public int maxSpeed = 90; // The maximum speed that the car can reach in km/h.
    [Range(10, 120)]
    public int maxReverseSpeed = 45; // The maximum speed that the car can reach while going in reverse in km/h.
    [Range(1, 10)]
    public int accelerationMultiplier = 2; // How fast the car can accelerate.
    [Space(10)]
    [Range(10, 45)]
    public int maxSteeringAngle = 35; // Increased from 27 to 35 for sharper turns.
    [Range(0.1f, 1f)]
    public float steeringSpeed = 0.8f; // Increased from 0.5f to 0.8f for more responsive steering.
    [Space(10)]
    [Range(100, 600)]
    public int brakeForce = 350; // The strength of the wheel brakes.
    [Range(1, 10)]
    public int decelerationMultiplier = 2; // How fast the car decelerates when the user is not using the throttle.
    [Range(1, 10)]
    public int handbrakeDriftMultiplier = 2; // Reduced from 5 to 2 to decrease drifting.
    [Space(10)]
    public Vector3 bodyMassCenter; // Center of mass of the car.

    // WHEELS
    public GameObject frontLeftMesh;
    public WheelCollider frontLeftCollider;
    [Space(10)]
    public GameObject frontRightMesh;
    public WheelCollider frontRightCollider;
    [Space(10)]
    public GameObject rearLeftMesh;
    public WheelCollider rearLeftCollider;
    [Space(10)]
    public GameObject rearRightMesh;
    public WheelCollider rearRightCollider;

    // PARTICLE SYSTEMS
    [Space(20)]
    [Space(10)]
    public bool useEffects = false;

    public ParticleSystem RLWParticleSystem;
    public ParticleSystem RRWParticleSystem;

    [Space(10)]
    public TrailRenderer RLWTireSkid;
    public TrailRenderer RRWTireSkid;

    // SPEED TEXT (UI)
    [Space(20)]
    [Space(10)]
    public bool useUI = false;
    public Text carSpeedText;

    // SOUNDS
    [Space(20)]
    [Space(10)]
    public bool useSounds = false;

    // New AudioSources for different car states
    [Space(10)]
    public AudioSource idleEngineSound;
    public AudioSource accelerationSound;
    public AudioSource brakingSound;
    public AudioSource driftingSound;

    // INPUT SYSTEM
    [Space(20)]
    [Space(10)]
    public InputActionAsset inputActions; // Reference to the Input Action Asset

    // CAR DATA
    [HideInInspector]
    public float carSpeed;
    [HideInInspector]
    public bool isDrifting;
    [HideInInspector]
    public bool isTractionLocked;

    // PRIVATE VARIABLES
    Rigidbody carRigidbody;
    float steeringAxis;
    float throttleAxis;
    float localVelocityZ;
    float localVelocityX;
    bool deceleratingCar;

    WheelFrictionCurve FLwheelFriction;
    float FLWextremumSlip;
    WheelFrictionCurve FRwheelFriction;
    float FRWextremumSlip;
    WheelFrictionCurve RLwheelFriction;
    float RLWextremumSlip;
    WheelFrictionCurve RRwheelFriction;
    float RRWextremumSlip;

    // Input Actions
    private InputAction steerAction;
    private InputAction accelerateAction;
    private InputAction brakeAction;
    private InputAction handbrakeAction;

    void Awake()
    {
        carRigidbody = gameObject.GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = bodyMassCenter;

        // Initial setup for wheel friction curves
        FLwheelFriction = new WheelFrictionCurve();
        FLwheelFriction.extremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLWextremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLwheelFriction.extremumValue = frontLeftCollider.sidewaysFriction.extremumValue;
        FLwheelFriction.asymptoteSlip = frontLeftCollider.sidewaysFriction.asymptoteSlip;
        FLwheelFriction.asymptoteValue = frontLeftCollider.sidewaysFriction.asymptoteValue;
        FLwheelFriction.stiffness = frontLeftCollider.sidewaysFriction.stiffness;

        FRwheelFriction = new WheelFrictionCurve();
        FRwheelFriction.extremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRWextremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRwheelFriction.extremumValue = frontRightCollider.sidewaysFriction.extremumValue;
        FRwheelFriction.asymptoteSlip = frontRightCollider.sidewaysFriction.asymptoteSlip;
        FRwheelFriction.asymptoteValue = frontRightCollider.sidewaysFriction.asymptoteValue;
        FRwheelFriction.stiffness = frontRightCollider.sidewaysFriction.stiffness;

        RLwheelFriction = new WheelFrictionCurve();
        RLwheelFriction.extremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLWextremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLwheelFriction.extremumValue = rearLeftCollider.sidewaysFriction.extremumValue;
        RLwheelFriction.asymptoteSlip = rearLeftCollider.sidewaysFriction.asymptoteSlip;
        RLwheelFriction.asymptoteValue = rearLeftCollider.sidewaysFriction.asymptoteValue;
        RLwheelFriction.stiffness = rearLeftCollider.sidewaysFriction.stiffness;

        RRwheelFriction = new WheelFrictionCurve();
        RRwheelFriction.extremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRWextremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRwheelFriction.extremumValue = rearRightCollider.sidewaysFriction.extremumValue;
        RRwheelFriction.asymptoteSlip = rearRightCollider.sidewaysFriction.asymptoteSlip;
        RRwheelFriction.asymptoteValue = rearRightCollider.sidewaysFriction.asymptoteValue;
        RRwheelFriction.stiffness = rearRightCollider.sidewaysFriction.stiffness;

        // Input System setup
        var drivingMap = inputActions.FindActionMap("Driving");

        steerAction = drivingMap.FindAction("Steer");
        accelerateAction = drivingMap.FindAction("Accelerate");
        brakeAction = drivingMap.FindAction("Brake");
        handbrakeAction = drivingMap.FindAction("Handbrake");
    }

    void OnEnable()
    {
        steerAction.Enable();
        accelerateAction.Enable();
        brakeAction.Enable();
        handbrakeAction.Enable();
    }

    void OnDisable()
    {
        steerAction.Disable();
        accelerateAction.Disable();
        brakeAction.Disable();
        handbrakeAction.Disable();
    }

    void Start()
    {
        // Invoke methods for UI and sounds
        if (useUI)
        {
            InvokeRepeating("CarSpeedUI", 0f, 0.1f);
        }
        else if (!useUI)
        {
            if (carSpeedText != null)
            {
                carSpeedText.text = "0";
            }
        }

        if (useSounds)
        {
            InvokeRepeating("CarSounds", 0f, 0.1f);
        }
        else if (!useSounds)
        {
            StopAllSounds();
        }

        if (!useEffects)
        {
            StopAllEffects();
        }
    }

    void Update()
    {
        // CAR DATA
        carSpeed = (2 * Mathf.PI * frontLeftCollider.radius * frontLeftCollider.rpm * 60) / 1000;
        localVelocityX = transform.InverseTransformDirection(carRigidbody.velocity).x;
        localVelocityZ = transform.InverseTransformDirection(carRigidbody.velocity).z;

        // INPUT HANDLING USING NEW INPUT SYSTEM
        float steerInput = steerAction.ReadValue<float>();
        steeringAxis = Mathf.Clamp(steerInput, -1f, 1f);

        // Steering
        var steeringAngle = steeringAxis * maxSteeringAngle;
        frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
        frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);

        // Acceleration
        float accelerateInput = accelerateAction.ReadValue<float>();
        if (accelerateInput > 0)
        {
            CancelInvoke("DecelerateCar");
            deceleratingCar = false;
            GoForward(accelerateInput);
        }

        // Braking (Reverse)
        float brakeInput = brakeAction.ReadValue<float>();
        if (brakeInput > 0)
        {
            CancelInvoke("DecelerateCar");
            deceleratingCar = false;
            GoReverse(brakeInput);
        }

        // Handbrake
        float handbrakeInput = handbrakeAction.ReadValue<float>();
        if (handbrakeInput > 0)
        {
            CancelInvoke("DecelerateCar");
            deceleratingCar = false;
            Handbrake();
        }
        else
        {
            RecoverTraction();
        }

        // Decelerate Car if No Input
        if (accelerateInput == 0 && brakeInput == 0 && handbrakeInput == 0 && !deceleratingCar)
        {
            InvokeRepeating("DecelerateCar", 0f, 0.1f);
            deceleratingCar = true;
        }

        // Animate wheel meshes
        AnimateWheelMeshes();
    }

    public void CarSpeedUI()
    {
        if (useUI)
        {
            try
            {
                float absoluteCarSpeed = Mathf.Abs(carSpeed);
                carSpeedText.text = Mathf.RoundToInt(absoluteCarSpeed).ToString();
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
        }
    }

    public void CarSounds()
    {
        if (useSounds)
        {
            try
            {
                // Idle Engine Sound
                if (Mathf.Abs(carSpeed) < 0.1f)
                {
                    if (!idleEngineSound.isPlaying)
                    {
                        idleEngineSound.Play();
                        accelerationSound.Stop();
                    }
                }
                else
                {
                    idleEngineSound.Stop();
                }

                // Acceleration Sound
                if (throttleAxis > 0 && carSpeed > 0.1f)
                {
                    if (!accelerationSound.isPlaying)
                    {
                        accelerationSound.Play();
                    }
                }
                else
                {
                    accelerationSound.Stop();
                }

                // Braking Sound
                if (throttleAxis < 0 && carSpeed > 0.1f)
                {
                    if (!brakingSound.isPlaying)
                    {
                        brakingSound.Play();
                    }
                }
                else
                {
                    brakingSound.Stop();
                }

                // Drifting Sound
                if (isDrifting)
                {
                    if (!driftingSound.isPlaying)
                    {
                        driftingSound.Play();
                    }
                }
                else
                {
                    driftingSound.Stop();
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
        }
        else
        {
            StopAllSounds();
        }
    }

    void StopAllSounds()
    {
        if (idleEngineSound != null && idleEngineSound.isPlaying)
        {
            idleEngineSound.Stop();
        }
        if (accelerationSound != null && accelerationSound.isPlaying)
        {
            accelerationSound.Stop();
        }
        if (brakingSound != null && brakingSound.isPlaying)
        {
            brakingSound.Stop();
        }
        if (driftingSound != null && driftingSound.isPlaying)
        {
            driftingSound.Stop();
        }
    }

    void StopAllEffects()
    {
        if (RLWParticleSystem != null)
        {
            RLWParticleSystem.Stop();
        }
        if (RRWParticleSystem != null)
        {
            RRWParticleSystem.Stop();
        }
        if (RLWTireSkid != null)
        {
            RLWTireSkid.emitting = false;
        }
        if (RRWTireSkid != null)
        {
            RRWTireSkid.emitting = false;
        }
    }

    // STEERING METHODS

    // The steering is now handled in the Update() method using the Input System.

    // ENGINE AND BRAKING METHODS

    public void GoForward(float input)
    {
        // Adjust isDrifting logic
        if (Mathf.Abs(localVelocityX) > 5f) // Increased threshold from 2.5f to 5f
        {
            isDrifting = true;
            DriftCarPS();
        }
        else
        {
            isDrifting = false;
            DriftCarPS();
        }

        // Smooth throttle axis
        throttleAxis += input * Time.deltaTime * 3f;
        throttleAxis = Mathf.Clamp(throttleAxis, 0f, 1f);

        if (localVelocityZ < -1f)
        {
            Brakes();
        }
        else
        {
            if (Mathf.RoundToInt(carSpeed) < maxSpeed)
            {
                ApplyMotorTorque((accelerationMultiplier * 50f) * throttleAxis);
            }
            else
            {
                ApplyMotorTorque(0f);
            }
        }
    }

    public void GoReverse(float input)
    {
        // Adjust isDrifting logic
        if (Mathf.Abs(localVelocityX) > 5f) // Increased threshold from 2.5f to 5f
        {
            isDrifting = true;
            DriftCarPS();
        }
        else
        {
            isDrifting = false;
            DriftCarPS();
        }

        // Smooth throttle axis
        throttleAxis -= input * Time.deltaTime * 3f;
        throttleAxis = Mathf.Clamp(throttleAxis, -1f, 0f);

        if (localVelocityZ > 1f)
        {
            Brakes();
        }
        else
        {
            if (Mathf.Abs(Mathf.RoundToInt(carSpeed)) < maxReverseSpeed)
            {
                ApplyMotorTorque((accelerationMultiplier * 50f) * throttleAxis);
            }
            else
            {
                ApplyMotorTorque(0f);
            }
        }
    }

    void ApplyMotorTorque(float torque)
    {
        frontLeftCollider.brakeTorque = 0;
        frontLeftCollider.motorTorque = torque;
        frontRightCollider.brakeTorque = 0;
        frontRightCollider.motorTorque = torque;
        rearLeftCollider.brakeTorque = 0;
        rearLeftCollider.motorTorque = torque;
        rearRightCollider.brakeTorque = 0;
        rearRightCollider.motorTorque = torque;
    }

    public void ThrottleOff()
    {
        ApplyMotorTorque(0f);
    }

    public void DecelerateCar()
    {
        if (Mathf.Abs(localVelocityX) > 5f) // Increased threshold from 2.5f to 5f
        {
            isDrifting = true;
            DriftCarPS();
        }
        else
        {
            isDrifting = false;
            DriftCarPS();
        }

        // Reset throttle axis smoothly
        if (throttleAxis != 0f)
        {
            if (throttleAxis > 0f)
            {
                throttleAxis -= Time.deltaTime * 10f;
            }
            else if (throttleAxis < 0f)
            {
                throttleAxis += Time.deltaTime * 10f;
            }
            if (Mathf.Abs(throttleAxis) < 0.15f)
            {
                throttleAxis = 0f;
            }
        }
        carRigidbody.velocity *= (1f / (1f + (0.025f * decelerationMultiplier)));
        ApplyMotorTorque(0f);
        if (carRigidbody.velocity.magnitude < 0.25f)
        {
            carRigidbody.velocity = Vector3.zero;
            CancelInvoke("DecelerateCar");
        }
    }

    public void Brakes()
    {
        frontLeftCollider.brakeTorque = brakeForce;
        frontRightCollider.brakeTorque = brakeForce;
        rearLeftCollider.brakeTorque = brakeForce;
        rearRightCollider.brakeTorque = brakeForce;
    }

    public void Handbrake()
    {
        CancelInvoke("RecoverTraction");

        // Adjust driftingAxis
        float driftingAxis = 1f;

        // Adjust isDrifting logic
        if (Mathf.Abs(localVelocityX) > 5f) // Increased threshold
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }

        // Adjust friction curves to reduce drifting effect
        float driftMultiplier = handbrakeDriftMultiplier * 0.5f; // Reduced effect by multiplying with 0.5f

        FLwheelFriction.extremumSlip = FLWextremumSlip * driftMultiplier * driftingAxis;
        frontLeftCollider.sidewaysFriction = FLwheelFriction;

        FRwheelFriction.extremumSlip = FRWextremumSlip * driftMultiplier * driftingAxis;
        frontRightCollider.sidewaysFriction = FRwheelFriction;

        RLwheelFriction.extremumSlip = RLWextremumSlip * driftMultiplier * driftingAxis;
        rearLeftCollider.sidewaysFriction = RLwheelFriction;

        RRwheelFriction.extremumSlip = RRWextremumSlip * driftMultiplier * driftingAxis;
        rearRightCollider.sidewaysFriction = RRwheelFriction;

        isTractionLocked = true;
        DriftCarPS();
    }

    public void DriftCarPS()
    {
        if (useEffects)
        {
            try
            {
                if (isDrifting)
                {
                    RLWParticleSystem.Play();
                    RRWParticleSystem.Play();
                }
                else if (!isDrifting)
                {
                    RLWParticleSystem.Stop();
                    RRWParticleSystem.Stop();
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }

            try
            {
                if ((isTractionLocked || Mathf.Abs(localVelocityX) > 5f) && Mathf.Abs(carSpeed) > 12f)
                {
                    RLWTireSkid.emitting = true;
                    RRWTireSkid.emitting = true;
                }
                else
                {
                    RLWTireSkid.emitting = false;
                    RRWTireSkid.emitting = false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
        }
        else
        {
            StopAllEffects();
        }
    }

    public void RecoverTraction()
    {
        isTractionLocked = false;

        // Reset friction curves to original values
        FLwheelFriction.extremumSlip = FLWextremumSlip;
        frontLeftCollider.sidewaysFriction = FLwheelFriction;

        FRwheelFriction.extremumSlip = FRWextremumSlip;
        frontRightCollider.sidewaysFriction = FRwheelFriction;

        RLwheelFriction.extremumSlip = RLWextremumSlip;
        rearLeftCollider.sidewaysFriction = RLwheelFriction;

        RRwheelFriction.extremumSlip = RRWextremumSlip;
        rearRightCollider.sidewaysFriction = RRwheelFriction;
    }

    void AnimateWheelMeshes()
    {
        try
        {
            Quaternion FLWRotation;
            Vector3 FLWPosition;
            frontLeftCollider.GetWorldPose(out FLWPosition, out FLWRotation);
            frontLeftMesh.transform.position = FLWPosition;
            frontLeftMesh.transform.rotation = FLWRotation;

            Quaternion FRWRotation;
            Vector3 FRWPosition;
            frontRightCollider.GetWorldPose(out FRWPosition, out FRWRotation);
            frontRightMesh.transform.position = FRWPosition;
            frontRightMesh.transform.rotation = FRWRotation;

            Quaternion RLWRotation;
            Vector3 RLWPosition;
            rearLeftCollider.GetWorldPose(out RLWPosition, out RLWRotation);
            rearLeftMesh.transform.position = RLWPosition;
            rearLeftMesh.transform.rotation = RLWRotation;

            Quaternion RRWRotation;
            Vector3 RRWPosition;
            rearRightCollider.GetWorldPose(out RRWPosition, out RRWRotation);
            rearRightMesh.transform.position = RRWPosition;
            rearRightMesh.transform.rotation = RRWRotation;
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }
}
