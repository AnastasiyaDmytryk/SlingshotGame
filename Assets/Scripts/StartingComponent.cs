using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartingComponent : MonoBehaviour
{
    public GameObject panel, element;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){
        if(panel.activeInHierarchy){
            EventSystem.current.SetSelectedGameObject(element);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
