using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LapCounter : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI laps;
    //[SerializeField] public TextMeshProUGUI pointText;
    [SerializeField] public GameObject Leaderboard, lapObject, startobj, finishobj;
    public bool lapActive = true;
    public int lapNum = 0;
    //public static int points = 100;
    // Start is called before the first frame update
    void Start()
    {
        startobj.SetActive(true);
        finishobj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(lapNum == 2 && lapActive){
            startobj.SetActive(false);
            finishobj.SetActive(true);
        }
        if(lapNum == 3){
            Time.timeScale = 0f;
            Leaderboard.SetActive(true);
        }
    }

    public void OnTriggerEnter(Collider collision){
        if(collision.gameObject.tag == "Lap"){
            lapNum += 1;
            //lapObject.SetActive(false);
            //lapActive = false;
            laps.text = "Lap: " + lapNum +"/3";
        }
        /*if(collision.gameObject.tag == "pUp"){
            points +=10;
            pointText.text = ("Points: " + points);
        }*/
        if(collision.gameObject.tag == "halfway"){            
            lapObject.SetActive(true);  
            lapActive = true;          
        }

    }
}
