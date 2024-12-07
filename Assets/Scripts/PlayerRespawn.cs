using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public List<Transform> waypoints;
    private bool markersReady = false;

    private IEnumerator Start()
    {
        DynamicMarkerGenerator markerGenerator = null;

        while (markerGenerator == null)
        {
            markerGenerator = FindObjectOfType<DynamicMarkerGenerator>();
            yield return null;
        }

        while (markerGenerator.generatedMarkers == null || markerGenerator.generatedMarkers.Count == 0)
        {
            yield return null;
        }

       
        waypoints = markerGenerator.generatedMarkers;
        markersReady = true;
    }

    private void Update()
    {
        if (markersReady && Input.GetKeyDown(KeyCode.R))
        {
            RespawnAtNearestMarker();
        }
    }

    void RespawnAtNearestMarker()
    {
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogWarning("No markers available for respawn!");
            return;
        }

        Transform nearestWaypoint = null;
        float nearestDist = Mathf.Infinity;
        Vector3 playerPos = transform.position;

        foreach (Transform marker in waypoints)
        {
            float dist = Vector3.Distance(playerPos, marker.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearestWaypoint = marker;
            }
        }

        if (nearestWaypoint != null)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            transform.position = nearestWaypoint.position;
            
        }
    }
}
