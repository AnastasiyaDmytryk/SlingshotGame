using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DynamicMarkerGenerator : MonoBehaviour
{
    [Header("Spline Setup")]
    public LineRenderer roadSpline;      
    public GameObject markerPrefab;      
    public float markerSpacing = 2.0f;  

    [Header("Generated Markers")]
    public List<Transform> generatedMarkers = new List<Transform>(); 

    private void Start()
    {
        if (roadSpline == null || markerPrefab == null)
        {
            Debug.LogError("Please assign the Road Spline and Marker Prefab!");
            return;
        }

        GenerateMarkers();
    }

    private void GenerateMarkers()
    {
       
        foreach (Transform marker in generatedMarkers)
        {
            Destroy(marker.gameObject);
        }
        generatedMarkers.Clear();

        
        float totalSplineLength = CalculateSplineLength(roadSpline);

       
        for (float distance = 0; distance <= totalSplineLength; distance += markerSpacing)
        {
           
            Vector3 markerPosition = GetPointOnSpline(roadSpline, distance / totalSplineLength);

           
            NavMeshHit hit;
            if (NavMesh.SamplePosition(markerPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                markerPosition = hit.position;
            }
            else
            {
                Debug.LogWarning("Marker position not on NavMesh: " + markerPosition);
                
            }

            
            GameObject newMarker = Instantiate(markerPrefab, markerPosition, Quaternion.identity, transform);
            newMarker.name = "Marker_" + generatedMarkers.Count; 
            generatedMarkers.Add(newMarker.transform);
        }

        Debug.Log("Generated " + generatedMarkers.Count + " markers along the spline.");
    }

    private float CalculateSplineLength(LineRenderer spline)
    {
        float length = 0f;
        for (int i = 1; i < spline.positionCount; i++)
        {
            length += Vector3.Distance(spline.GetPosition(i - 1), spline.GetPosition(i));
        }
        return length;
    }

    private Vector3 GetPointOnSpline(LineRenderer spline, float t)
    {
        t = Mathf.Clamp01(t); 
        float totalLength = CalculateSplineLength(spline);

        float currentLength = 0f;
        for (int i = 1; i < spline.positionCount; i++)
        {
            float segmentLength = Vector3.Distance(spline.GetPosition(i - 1), spline.GetPosition(i));
            if (currentLength + segmentLength >= t * totalLength)
            {
                float segmentT = (t * totalLength - currentLength) / segmentLength;
                return Vector3.Lerp(spline.GetPosition(i - 1), spline.GetPosition(i), segmentT);
            }
            currentLength += segmentLength;
        }

        return spline.GetPosition(spline.positionCount - 1); 
    }
}
