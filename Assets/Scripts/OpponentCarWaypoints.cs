using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpponentCarWaypoints : MonoBehaviour
{
    [Header("Opponent Car")]
    public OpponentCar opponentCar;

    [Header("Waypoints")]
    private int currentWaypointIndex = 0;
    private List<Transform> waypoints;

    [Header("Navigation Settings")]
    private float lookaheadDistance = 3f;
    private float rotationSpeed = 5f;

    private Rigidbody rb;
    private NavMeshAgent agent;

    private bool initialized = false; // Flag to check if initialization is complete

    private void Awake()
    {
        opponentCar = GetComponent<OpponentCar>();

        if (opponentCar == null)
        {
            Debug.LogError("OpponentCar component not found on " + gameObject.name);
            enabled = false;
            return;
        }
    }

    private IEnumerator Start()
    {
        // Initialize agent
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
            yield break;
        }

        // Wait until the DynamicMarkerGenerator is available
        DynamicMarkerGenerator markerGenerator = null;
        while (markerGenerator == null)
        {
            markerGenerator = FindObjectOfType<DynamicMarkerGenerator>();
            if (markerGenerator == null)
            {
                Debug.LogError("DynamicMarkerGenerator not found in the scene!");
                yield break;
            }
            yield return null; // Wait for the next frame
        }

        // Wait until the markers are generated
        while (markerGenerator.generatedMarkers == null || markerGenerator.generatedMarkers.Count == 0)
        {
            yield return null; // Wait for the next frame
        }

        // Assign the waypoints
        waypoints = new List<Transform>(markerGenerator.generatedMarkers);

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("No waypoints found after waiting!");
            yield break;
        }

        // Set the initial destination
        opponentCar.LocateDestination(waypoints[currentWaypointIndex].position);

        Debug.Log("Waypoints successfully assigned in OpponentCarWaypoints for " + gameObject.name);

        initialized = true; // Mark initialization as complete
    }

    private void Update()
    {
        if (!initialized) return;

        // Check if waypoints list and agent are valid
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("Waypoints list is null or empty in " + gameObject.name);
            return;
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is null in " + gameObject.name);
            return;
        }

        // Check if the agent has reached the current waypoint
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log(gameObject.name + " reached waypoint " + currentWaypointIndex);
            AdvanceToNextWaypoint();
        }
    }

    private void AdvanceToNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        Debug.Log(gameObject.name + " advancing to waypoint " + currentWaypointIndex);
        opponentCar.LocateDestination(waypoints[currentWaypointIndex].position);
    }
}
