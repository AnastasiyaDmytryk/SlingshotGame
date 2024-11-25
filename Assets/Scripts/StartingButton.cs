using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingButton : MonoBehaviour
{
    public GameObject panel;
    public Button primaryButton;
    // Start is called before the first frame update
    void Start()
    {
        //primaryButton.Select();
    }

    void OnEnable(){
        if(panel.activeInHierarchy){
            primaryButton.Select();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
