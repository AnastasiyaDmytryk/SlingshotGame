using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainL2 : MonoBehaviour
{
    public PullStringL1 pullString; // Reference to the PullString script
    public Transform[] spawnPoints; // Array of spawn points
    public TimeController timeController;
    public int[] spawnIndexes = { 6, 1, 2, 3, 4 };

    public GameObject[] AIDrivers; // Prefabs for AI cars
    public Rigidbody[] AIRb; // Rigidbody components for AI cars

    public bool carMoved = false;
    public UnityEngine.AI.NavMeshAgent[] aiNavAgents;

    void Start()
    {
         foreach(UnityEngine.AI.NavMeshAgent a in aiNavAgents)
        {
             a.enabled = false;
        }
        timeController.enabled = false;
    }

    void Update()
    {
        if (pullString.isLaunched && !carMoved)
        {
            MoveCarToSpawnPoint();
            pullString.isLaunched = false;
            Destroy(pullString.gameObject);
            MoveAICars();
        }

        if (timeController.timeComplete)
        {
            foreach (Transform spawn in spawnPoints)
            {
                spawn.gameObject.SetActive(false);
            }

            foreach (Rigidbody carA in AIRb)
            {
                carA.constraints = RigidbodyConstraints.None;
            }

            pullString.rb.constraints = RigidbodyConstraints.None;
        }
    }

    void MoveCarToSpawnPoint()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length - 1);
        spawnIndexes[spawnIndex] = 0;

        Transform selectedSpawnPoint = spawnPoints[spawnIndex];
        pullString.Car.transform.position = selectedSpawnPoint.position;
        carMoved = true;
        timeController.enabled = true;
    }

       void MoveAICars()
{
    
        foreach(UnityEngine.AI.NavMeshAgent a in aiNavAgents)
        {
             a.enabled = true;
        }
    
}
}
