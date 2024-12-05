using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class racerAI01Track : MonoBehaviour
{
    public GameObject TheMarker;
    public GameObject Mark01; 
    public GameObject Mark02; 
    public GameObject Mark03; 
    public GameObject Mark04; 
    public GameObject Mark05; 
    public GameObject Mark06; 
    public GameObject Mark07; 
    public GameObject Mark08; 
    public GameObject Mark09; 
    public GameObject Mark10;
    public GameObject Mark11; 
    public GameObject Mark12; 
    public GameObject Mark13; 
    public GameObject Mark14; 
    public GameObject Mark15; 
    public GameObject Mark16; 
    public GameObject Mark17; 
    public GameObject Mark18; 
    public GameObject Mark19; 
    public GameObject Mark20; 
    public GameObject Mark21; 
    public GameObject Mark22; 
    public GameObject Mark23; 
    public GameObject Mark24; 
    public GameObject Mark25; 
    public GameObject Mark26; 
    public GameObject Mark27; 
    public GameObject Mark28; 
    public GameObject Mark29; 
    public GameObject Mark30; 
    public GameObject Mark31;
    public int MarkTracker; 
    

    void Update()
    {
        if(MarkTracker == 0)
        {
            TheMarker.transform.position = Mark01.transform.position;
        }
        if (MarkTracker == 1)
        {
            TheMarker.transform.position = Mark02.transform.position;
        }
        if (MarkTracker == 2)
        {
            TheMarker.transform.position = Mark03.transform.position;
        }
    }
}
