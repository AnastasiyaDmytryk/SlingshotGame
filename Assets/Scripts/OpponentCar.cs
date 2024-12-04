using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpponentCar : MonoBehaviour
{
    [Header("Car Physics")]
    public WheelCollider[] wheelColliders = new WheelCollider[4]; // 0: FL, 1: FR, 2: RL, 3: RR
    public Transform[] wheelMeshes = new Transform[4];
    public float maxMotorTorque = 1500f;
    public float maxSteerAngle = 30f;

    [Header("AI Navigation")]
    public Vector3 destination;
    public bool destinationReached;

    [Header("Components")]
    public NavMeshAgent agent;
    private Rigidbody rb;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
            enabled = false; // Disable this script to prevent further errors
            return;
        }

        // Disable NavMeshAgent's automatic movement
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    private void FixedUpdate()
    {
        Drive();
        UpdateWheelMeshes();

        // Sync the NavMeshAgent position with the car's Rigidbody
        if (agent != null)
        {
            agent.nextPosition = rb.position;
        }
    }

    public void Drive()
    {
        // Reset brake torque
        foreach (WheelCollider wheel in wheelColliders)
        {
            wheel.brakeTorque = 0f;
        }

        // Check if destination is reached
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            destinationReached = true;
            ApplyBrake();
            return;
        }
        else
        {
            destinationReached = false;
        }

        // Get desired velocity from NavMeshAgent
        Vector3 desiredVelocity = agent.desiredVelocity;

        // Convert desired velocity to local space
        Vector3 localDesiredVelocity = transform.InverseTransformDirection(desiredVelocity);

        // Invert motor torque if car is moving backward
        float motorTorque = maxMotorTorque * Mathf.Clamp(localDesiredVelocity.z, 0f, 1f);

        // Apply motor torque to rear wheels if they are grounded
        if (wheelColliders[2].isGrounded)
            wheelColliders[2].motorTorque = motorTorque;

        if (wheelColliders[3].isGrounded)
            wheelColliders[3].motorTorque = motorTorque;

        // Calculate steering angle
        float steerAngle = maxSteerAngle * Mathf.Clamp(localDesiredVelocity.x, -1f, 1f);

        // Apply steering to front wheels
        wheelColliders[0].steerAngle = steerAngle;
        wheelColliders[1].steerAngle = steerAngle;

        // Apply downforce if needed
        ApplyDownforce();
    }

    private void ApplyBrake()
    {
        // Apply brake torque to all wheels
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].brakeTorque = maxMotorTorque;
        }
    }

    private void ApplyDownforce()
    {
        float downforce = 100f; // Adjust as needed
        rb.AddForce(-transform.up * downforce * rb.velocity.magnitude);
    }

    private void UpdateWheelMeshes()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            Quaternion quat;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out quat);

            wheelMeshes[i].position = position;
            wheelMeshes[i].rotation = quat;
        }
    }

    public void LocateDestination(Vector3 destination)
    {
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is null in OpponentCar.");
            return;
        }

        this.destination = destination;
        agent.SetDestination(destination);
        destinationReached = false;
    }

    void OnDrawGizmos()
    {
        if (agent != null && agent.path != null)
        {
            var path = agent.path;
            Gizmos.color = Color.green;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
            }
        }
    }
}
