using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LapCounter : MonoBehaviour
{
    public TextMeshProUGUI laps;
    public GameObject Leaderboard;
    int lapNum = 0;
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
    }
}
