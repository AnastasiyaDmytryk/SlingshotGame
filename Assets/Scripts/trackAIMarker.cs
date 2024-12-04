using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class trackAIMarker : MonoBehaviour
{
    public static trackAIMarker Instance { get; private set; }
    public List<Transform> Markers = new List<Transform>();

    void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

       
        RefreshMarkers();
    }

    void Start()
    {
       

        Debug.Log("trackAIMarker: Found " + Markers.Count + " markers.");
    }

    public void RefreshMarkers()
    {
        GameObject[] markerObjects = GameObject.FindGameObjectsWithTag("Marker");

        if (markerObjects.Length == 0)
        {
            Debug.LogError("No markers found with tag 'Marker'!");
        }
        else
        {
            Markers = markerObjects.OrderBy(marker => marker.name).Select(m => m.transform).ToList();
            Debug.Log($"Found {Markers.Count} markers.");
        }
    }

    public void AddMarker(Transform marker)
    {
        if (!Markers.Contains(marker))
        {
            Markers.Add(marker);
        }
    }
}
