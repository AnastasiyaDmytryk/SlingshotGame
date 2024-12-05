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
    public float progress = 0f;   
    public int position = 0;     
    public float turnSpeed = 2f;

    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        agent.updatePosition = false;  
        agent.updateRotation = false;  
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

        
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

        
        rb.velocity = transform.forward * moveSpeed;
    }

    private void SetNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        targetWaypoint = waypoints[currentWaypointIndex];
        agent.SetDestination(targetWaypoint.position);  
    }

    private void UpdateProgress()
    {
        
        progress = currentWaypointIndex + Vector3.Distance(transform.position, targetWaypoint.position);
    }

    public void AdjustSpeed(float multiplier)
    {
        agent.speed = defaultSpeed * multiplier;
    }
}
