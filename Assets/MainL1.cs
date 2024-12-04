using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainL1 : MonoBehaviour
{
    public PullString pullString;  // Reference to the PullString script
    public CarController carController; // Reference to the CarController script
    public Transform[] spawnPoints;  // Array of spawn points
    public Text countdownText;  // UI Text for countdown

    private bool countdownComplete = false;
    private bool carMoved = false;

    void Start()
    {
        carController.enabled = false; // Disable car movement initially
    }

    void Update()
    {
        // Check if the slingshot was pulled and clicked, and the car hasn't been launched yet
        if (pullString.isLaunched== true && !carMoved)
        {
            MoveCarToSpawnPoint();
            pullString.isLaunched = false;
            Destroy(pullString.gameObject);
        }

       
    }

    void MoveCarToSpawnPoint()
    {
        // Calculate a random spawn point index
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Move the car to the selected spawn point
        Transform selectedSpawnPoint = spawnPoints[spawnIndex];
        pullString.Car.transform.position = selectedSpawnPoint.position;
        carController.enabled = true;
        
    }

  
}