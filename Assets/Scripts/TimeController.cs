using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class TimeController : MonoBehaviour
{

    public TextMeshProUGUI time;
    public TextMeshProUGUI title;
    public TextMeshProUGUI countDown;
    public GameObject player;
    public CarController carController;
    public Transform[] spawnPoints;
    public bool timeComplete;

    // Start is called before the first frame update
    void Start()
    {
        
        carController.enabled=false;
        timeComplete=false;
    }

    // Update is called once per frame
    void Update()
    {
        
        Scene current = SceneManager.GetActiveScene();
        string sceneName = current.name;

        if(sceneName == "levltown"){
            title.text = "Level 1";
        }else if(sceneName == "Level2"){
            title.text = "Level 2";
        }else if(sceneName == "Levl3"){
            title.text = "Level 3";
        }

        float timeSinceLoad = Time.timeSinceLevelLoad -4;
        int minutes = (int)((timeSinceLoad % 3600) / 60);
        int seconds = (int)(timeSinceLoad % 60);

        if(seconds == -3){
            countDown.text = "3";
        }else if(seconds == -2){
            countDown.text = "2";
        }else if(seconds == -1){
            countDown.text = "1";
        }else if(seconds == 0){
            countDown.text = "Go!";
            
            
            
           

        }else if(seconds > 0){
            countDown.enabled = false;
            timeComplete=true;
            Debug.Log("timep completed");
            carController.enabled = true;
            carController.started = true;
            time.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }

    }


}
