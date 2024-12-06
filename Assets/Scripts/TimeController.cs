using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;


public class TimeController : MonoBehaviour
{

    public TextMeshProUGUI time;
    public TextMeshProUGUI title;
    public TextMeshProUGUI countDown;
    public GameObject player;
   // public CarController carController;
    public Transform[] spawnPoints;
    public bool timeComplete;

    //TIMERCLOCKTHING
    bool clockActive = false;
    float currentTimeClock;

    //TIMERFORCOUNTDOWN
    bool timerActive = false;
    public int startTime;
    float currentTimeCountDown;
    


    // Start is called before the first frame update
    void Start()
    {
        
        //carController.enabled=false;
        timeComplete=false;
        currentTimeClock = 0;
        currentTimeCountDown = startTime;
        StartTimer();
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

        if(clockActive == true){
            currentTimeClock = currentTimeClock + Time.deltaTime;
        }
        TimeSpan timeC = TimeSpan.FromSeconds(currentTimeClock);
        //clockText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
        TimeSpan timeT = TimeSpan.FromSeconds(currentTimeCountDown);

        if(timerActive == true){
            countDown.enabled = true;
            Debug.Log("countdown start");
            currentTimeCountDown = currentTimeCountDown - Time.deltaTime;
            if(currentTimeCountDown == 4){
                countDown.text = "3";
            }else if(currentTimeCountDown == 3){
                countDown.text = "2";
            }else if(currentTimeCountDown == 2){
                countDown.text = "1";
            }else if(currentTimeCountDown == 0){
                countDown.text = "Go!";
            }else if(currentTimeCountDown <= 0){
                timerActive = false;
            }
        }

        if(timerActive == false){
            countDown.enabled = false;
            timeComplete=true;
            StartClock();
            Debug.Log("countdown done, clock started");
            //carController.enabled = true;
            //carController.started = true;
            time.text = string.Format("{0:D2}:{1:D2}", timeC.Minutes, timeC.Seconds);
        }

    }

    public void StartClock(){
        clockActive = true;
    }

    public void StartTimer(){
        timerActive = true;
    }
    


}
