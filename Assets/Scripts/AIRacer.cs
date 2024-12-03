using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRacer : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rb;
    public Transform[] waypoints;
    public float moveSpeed = 10f;
    public float defaultSpeed = 10f;
    public float progress = 0f;   // Track progress for sorting
    public int position = 0;      // Track position in the race
    public float turnSpeed = 2f;

    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        agent.updatePosition = false;  // Disable automatic NavMeshAgent position update
        agent.updateRotation = false;  // Disable automatic NavMeshAgent rotation update
        SetNextWaypoint();
    }

    void FixedUpdate()
    {
        MoveTowardsWaypoint();
    }

    private void MoveTowardsWaypoint()
    {
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 1f)
        {
            SetNextWaypoint();
        }

        // Calculate direction and rotate towards it
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

        // Apply forward force for movement
        rb.velocity = transform.forward * moveSpeed;
    }

    private void SetNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        targetWaypoint = waypoints[currentWaypointIndex];
        agent.SetDestination(targetWaypoint.position);  // Use NavMeshAgent to calculate path to next waypoint
    }

    private void UpdateProgress()
    {
        // Calculate progress based on distance to next waypoint
        progress = currentWaypointIndex + Vector3.Distance(transform.position, targetWaypoint.position);
    }

    public void AdjustSpeed(float multiplier)
    {
        agent.speed = defaultSpeed * multiplier;
    }
}
