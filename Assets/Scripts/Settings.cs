using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
    }  

    public void ChangeRes(int val){
        if(val == 0){
            Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
            Debug.Log("Res1");
        }
        
        if(val == 1){
            Screen.SetResolution(1366, 768, FullScreenMode.Windowed);
            Debug.Log("Res2");
        }

        if(val == 2){
            Screen.SetResolution(2560, 1440, FullScreenMode.Windowed);
            Debug.Log("Res3");
        }

        if(val == 3){
            Screen.SetResolution(3840, 2160, FullScreenMode.Windowed);
            Debug.Log("Res4");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
