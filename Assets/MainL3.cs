using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainL3 : MonoBehaviour
{
    public PillStringL3 pullString; // Reference to the PullString script
    public Transform[] spawnPoints; // Array of spawn points
    public TimeController timeController;
    public int[] spawnIndexes = { 6, 1, 2, 3, 4 };
    public UnityEngine.AI.NavMeshAgent[] aiNavAgents;


    public bool carMoved = false;

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
