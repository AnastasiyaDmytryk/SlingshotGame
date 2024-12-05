using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(LineRenderer))]
public class SplineMeshToLineRenderer : MonoBehaviour
{
    public MeshFilter meshFilter; 
    public int resolution = 125; 

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter is not assigned!");
            return;
        }

        GenerateLineFromMesh();
    }

    private void GenerateLineFromMesh()
    {
        Mesh mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            Debug.LogError("No mesh found in MeshFilter!");
            return;
        }

        Vector3[] vertices = mesh.vertices;

        if (vertices.Length < resolution)
        {
            Debug.LogError("Not enough vertices in the mesh for the desired resolution.");
            return;
        }

        
        lineRenderer.positionCount = resolution;

      
        List<Vector3> midpoints = CalculateMidpoints(vertices, resolution);

        
        for (int i = 0; i < midpoints.Count; i++)
        {
            lineRenderer.SetPosition(i, midpoints[i]);
        }

        Debug.Log($"Generated {midpoints.Count} points from the mesh.");
    }

    private List<Vector3> CalculateMidpoints(Vector3[] vertices, int resolution)
    {
        List<Vector3> midpoints = new List<Vector3>();

        
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = meshFilter.transform.TransformPoint(vertices[i]);
        }

        
        float step = (float)vertices.Length / resolution;

        for (int i = 0; i < resolution; i++)
        {
            int startIndex = Mathf.FloorToInt(i * step);
            int endIndex = Mathf.Min(Mathf.FloorToInt((i + 1) * step), vertices.Length - 1);

            Vector3 leftmost = vertices[startIndex];
            Vector3 rightmost = vertices[startIndex];

           
            for (int j = startIndex; j <= endIndex; j++)
            {
                if (vertices[j].x < leftmost.x) leftmost = vertices[j];
                if (vertices[j].x > rightmost.x) rightmost = vertices[j];
            }

            
            Vector3 midpoint = (leftmost + rightmost) / 2.0f;
            midpoints.Add(midpoint);
        }

        return midpoints;
    }
}
