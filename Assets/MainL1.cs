using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainL1 : MonoBehaviour
{
    public PullString pullString;  // Reference to the PullString script
    public CarController carController; // Reference to the CarController script
    public Transform[] spawnPoints;  // Array of spawn points
    public TimeController timeController;

    public bool carMoved = false;

    void Start()
    {
        
        pullString.Car.GetComponent<CarController>().enabled = false;// Disable car movement initially
        timeController.enabled=false;
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
       
        if(timeController.timeComplete && spawnPoints.Length>0)
        {
            foreach(Transform spawn in spawnPoints)
            {
                Debug.Log(spawnPoints.Length);
                spawn.gameObject.SetActive(false);

                
            }
             spawnPoints=new Transform [1];

            
        }
        if(spawnPoints.Length<=1)
        {
            //add car script
            pullString.Car.GetComponent<CarController>().enabled = true;
            // modify car weight and speed and force
            pullString.Car.GetComponent<CarController>().motorForce=250;
       
            Debug.Log(carController.enabled);
            carController.started=true;
            carController.isMovementAllowed = true;
            pullString.rb.constraints = RigidbodyConstraints.None;
        }
       

       
    }

    void MoveCarToSpawnPoint()
    {
        // Calculate a random spawn point index
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Move the car to the selected spawn point
        Transform selectedSpawnPoint = spawnPoints[spawnIndex];
        pullString.Car.transform.position = selectedSpawnPoint.position;
        carMoved=true;
        timeController.enabled=true;
         
       
    }

  
}
