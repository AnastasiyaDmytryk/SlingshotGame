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

    public GameObject[] AIDrivers; // Prefabs for AI cars
    public Rigidbody[] AIRb; // Rigidbody components for AI cars

    public bool carMoved = false;

    void Start()
    {
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
        for (int i = 0; i < AIDrivers.Length; i++)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length - 1);

            if (spawnIndexes[spawnIndex] != 0)
            {
                Transform selectedSpawnPoint = spawnPoints[spawnIndex];
                AIDrivers[i].transform.position = new Vector3( selectedSpawnPoint.position.x,selectedSpawnPoint.position.y,  selectedSpawnPoint.position.z);

                AIDrivers[i].transform.localScale = Vector3.one * 0.1f;
              
                spawnIndexes[spawnIndex] = 0;
            }
        }

       
    }
}
