using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void goToGame()
    {
        SceneManager.LoadScene("levltown");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
