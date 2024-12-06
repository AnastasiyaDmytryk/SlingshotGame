using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainL1 : MonoBehaviour
{
    public PullString pullString;  // Reference to the PullString script
    //public CarController carController; // Reference to the CarController script
    public Transform[] spawnPoints;  // Array of spawn points
    public TimeController timeController;
    public int[] spawnIndexes={6,1,2,3,4};
    public GameObject[] AIDriversPrefabs;
    public GameObject[] AIDrivers;
    public Rigidbody[] AIRb;

    public bool carMoved = false;

    void Start()
    {
        
        //carController.enabled=false;
        //carController.started = false; // Disable car movement initially
        timeController = GetComponent<TimeController> ();
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
            MoveAICars();
            
            
        }
       
        if(timeController.timeComplete && spawnPoints.Length>0)
        {
            foreach(Transform spawn in spawnPoints)
            {
                //Debug.Log("destriyng spawns");
                spawn.gameObject.SetActive(false);
                
            }
            //carController.enabled=true;
            
          
           /* Debug.Log(carController.enabled);
            carController.started=true;
            carController.isMovementAllowed = true;*/
            pullString.rb.constraints = RigidbodyConstraints.None;
        }
       

       
    }

    void MoveCarToSpawnPoint()
    {
        // Calculate a random spawn point index
        int spawnIndex = Random.Range(0, spawnPoints.Length-1);
        spawnIndexes[spawnIndex]=0;

        // Move the car to the selected spawn point
        Transform selectedSpawnPoint = spawnPoints[spawnIndex];
        pullString.Car.transform.position = selectedSpawnPoint.position;
        carMoved=true;
        timeController.enabled=true;
         
       
    }
    void MoveAICars()
    {
        int numOfCars=0;
        while(numOfCars<3)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length-1);
            if(spawnIndexes[spawnIndex]!=0)
            {
                //Debug.Log(spawnIndexes[spawnIndex]);
                Transform selectedSpawnPoint = spawnPoints[spawnIndex];
                AIDrivers[numOfCars]=Instantiate(AIDriversPrefabs[numOfCars]);
                AIRb[numOfCars] = AIDrivers[numOfCars].GetComponent<Rigidbody>();
                AIDrivers[numOfCars].transform.position=selectedSpawnPoint.position;
                
                numOfCars+=1;
                spawnIndexes[spawnIndex]=0;



            }

        }
        
        
    }

  
}
