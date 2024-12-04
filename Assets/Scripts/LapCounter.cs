using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LapCounter : MonoBehaviour
{
    public TextMeshProUGUI laps;
    public TextMeshProUGUI pointText;
    public GameObject Leaderboard;
    public int lapNum = 0;
    public int points = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lapNum == 3){
            Time.timeScale = 0f;
            Leaderboard.SetActive(true);
        }
    }

    public void OnTriggerEnter(Collider collision){
        if(collision.gameObject.tag == "Lap"){
            lapNum += 1;
            laps.text = "Lap: " + lapNum +"/3";
        }
        if(collision.gameObject.tag == "pUp"){
            points +=1;
            pointText.text = ("Points: " + points);
        }

    }
}