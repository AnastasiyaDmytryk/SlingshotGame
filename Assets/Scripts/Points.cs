using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    public static TextMeshProUGUI pointText;
    public static int points = 100;
    
    public void OnTriggerEnter(Collider collision){
        
        if(collision.gameObject.tag == "pUp"){
            AddPoints();
        }        

    }
    public void AddPoints(){
        points +=10;
        pointText.text = points.ToString();
    }

    public static void GetPoints(){
        int gotPoints = points;
        pointText.text = ("Points: " + gotPoints.ToString());
    }
}
