using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MainL1 : MonoBehaviour
{
    public PullString pullString;  // Reference to the PullString script
    public CarController carController; // Reference to the CarController script
    public Transform[] spawnPoints;  // Array of spawn points
    public Text countdownText;  // UI Text for countdown
    bool timerActive = false; 
    float currentTime;
    public TextMeshProUGUI currentTimeText;

    private bool countdownComplete = false;
    private bool carMoved = false;

    void Start()
    {
        carController.enabled = false; // Disable car movement initially
        currentTime = 0;
    }

    void Update()
    {
        // Check if the slingshot was pulled and clicked, and the car hasn't been launched yet
        if (pullString.isLaunched== true && !carMoved)
        {
            MoveCarToSpawnPoint();
            pullString.isLaunched = false;
            Destroy(pullString.gameObject);
            StopTimer();
        }

        //timer 
        if(timerActive == true){
            currentTime = currentTime + timerActive.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        //currentTimeText.text = string.Format("{0:D2}:{1:D2}", time.Minutes.ToString(), time.Seconds.ToString());
        
       
    }

    public void StartTimer(){
        timerActive = true;
        if(seconds == 0){
            countDown.text = "3";
        }else if(seconds == 1){
            countDown.text = "2";
        }else if(seconds == 2){
            countDown.text = "1";
        }else if(seconds == 3){
            countDown.text = "Go!";
        }else if(seconds > 3){
            countDown.enabled = false;
            time.text = string.Format("{0:D2}:{1:D2}", time.Minutes.ToString(), time.Seconds.ToString());
        }
    }
    public void StopTimer(){
        timerActive = false;
    }

    void MoveCarToSpawnPoint()
    {
        // Calculate a random spawn point index
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Move the car to the selected spawn point
        Transform selectedSpawnPoint = spawnPoints[spawnIndex];
        pullString.Car.transform.position = selectedSpawnPoint.position;
        carController.enabled = true;
        StartTimer();
    }

  
}