using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(LineRenderer))]
public class SplineMeshToLineRenderer : MonoBehaviour
{
    public MeshFilter meshFilter; // Mesh filter from the spline
    public int resolution = 125; // Number of points to sample along the mesh

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

        // Set up the LineRenderer
        lineRenderer.positionCount = resolution;

        // Calculate midpoints along the mesh
        List<Vector3> midpoints = CalculateMidpoints(vertices, resolution);

        // Assign midpoints to the LineRenderer
        for (int i = 0; i < midpoints.Count; i++)
        {
            lineRenderer.SetPosition(i, midpoints[i]);
        }

        Debug.Log($"Generated {midpoints.Count} points from the mesh.");
    }

    private List<Vector3> CalculateMidpoints(Vector3[] vertices, int resolution)
    {
        List<Vector3> midpoints = new List<Vector3>();

        // Transform vertices to world space
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = meshFilter.transform.TransformPoint(vertices[i]);
        }

        // Divide vertices into steps along the mesh
        float step = (float)vertices.Length / resolution;

        for (int i = 0; i < resolution; i++)
        {
            int startIndex = Mathf.FloorToInt(i * step);
            int endIndex = Mathf.Min(Mathf.FloorToInt((i + 1) * step), vertices.Length - 1);

            Vector3 leftmost = vertices[startIndex];
            Vector3 rightmost = vertices[startIndex];

            // Find the leftmost and rightmost vertices in this segment
            for (int j = startIndex; j <= endIndex; j++)
            {
                if (vertices[j].x < leftmost.x) leftmost = vertices[j];
                if (vertices[j].x > rightmost.x) rightmost = vertices[j];
            }

            // Calculate the midpoint between the leftmost and rightmost vertices
            Vector3 midpoint = (leftmost + rightmost) / 2.0f;
            midpoints.Add(midpoint);
        }

        return midpoints;
    }
}
