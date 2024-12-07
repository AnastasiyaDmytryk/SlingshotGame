using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void goToGame()
    {
        SceneManager.LoadScene("levltown");
        Time.timeScale = 1f;
    }

    public void goToLevel2()
    {
        SceneManager.LoadScene("Level2");
        Time.timeScale = 1f;
        
    }

    public void goToLevel3()
    {
        SceneManager.LoadScene("Levl3");
        Time.timeScale = 1f;
    }

}
