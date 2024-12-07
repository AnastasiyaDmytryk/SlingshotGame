using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    public TextMeshProUGUI pointText;
    public int points = 0;
    
    public void OnTriggerEnter(Collider collision){
        
        if(collision.gameObject.tag == "pUp"){
            points +=10;
            GetPoints();
        }        

    }
    /*public void AddPoints(){
        points +=10;
        pointText.text = points.ToString();
    }*/

    public void GetPoints(){
        int gotPoints = points;
        pointText.text = ("Points: " + gotPoints.ToString());
    }
}
