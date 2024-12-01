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

    private bool isLaunched = false;
    private bool countdownComplete = false;

    void Start()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false); // Ensure countdown text is hidden initially
        }

        carController.enabled = false; // Disable car movement initially
    }

    void Update()
    {
        // Check if the slingshot was pulled and clicked, and the car hasn't been launched yet
        if (pullString != null && pullString.move == false && !isLaunched)
        {
            MoveCarToSpawnPoint();
            StartCoroutine(CountdownBeforeMovement());
            isLaunched = true;
        }

        // Allow car control after countdown
        if (countdownComplete && carController != null)
        {
            carController.enabled = true;
        }
    }

    void MoveCarToSpawnPoint()
    {
        // Calculate a random spawn point index
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Move the car to the selected spawn point
        Transform selectedSpawnPoint = spawnPoints[spawnIndex];
        pullString.Car.transform.position = selectedSpawnPoint.position;
        pullString.Car.transform.rotation = selectedSpawnPoint.rotation;
    }

    IEnumerator CountdownBeforeMovement()
    {
        if (countdownText == null)
            yield break;

        // Show the countdown text
        countdownText.gameObject.SetActive(true);

        // Countdown from 3 to 1
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        // Hide the countdown text after it ends
        countdownText.gameObject.SetActive(false);

        countdownComplete = true;
    }
}
