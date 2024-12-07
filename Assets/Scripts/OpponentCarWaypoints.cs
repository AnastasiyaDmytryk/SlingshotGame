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

    private bool initialized = false; 

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
        
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
            yield break;
        }

        
        DynamicMarkerGenerator markerGenerator = null;
        while (markerGenerator == null)
        {
            markerGenerator = FindObjectOfType<DynamicMarkerGenerator>();
            if (markerGenerator == null)
            {
                Debug.LogError("DynamicMarkerGenerator not found in the scene!");
                yield break;
            }
            yield return null; 
        }

       
        while (markerGenerator.generatedMarkers == null || markerGenerator.generatedMarkers.Count == 0)
        {
            yield return null; 
        }

        
        waypoints = new List<Transform>(markerGenerator.generatedMarkers);

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("No waypoints found after waiting!");
            yield break;
        }

       
        opponentCar.LocateDestination(waypoints[currentWaypointIndex].position);

        Debug.Log("Waypoints successfully assigned in OpponentCarWaypoints for " + gameObject.name);

        initialized = true; 
    }

    private void Update()
    {
        if (!initialized) return;

        
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

       
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            //Debug.Log(gameObject.name + " reached waypoint " + currentWaypointIndex);
            AdvanceToNextWaypoint();
        }
    }

    private void AdvanceToNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        //Debug.Log(gameObject.name + " advancing to waypoint " + currentWaypointIndex);
        opponentCar.LocateDestination(waypoints[currentWaypointIndex].position);
    }
}
