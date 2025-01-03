using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LapComplete : MonoBehaviour
{
    public GameObject LapCompleteTrigger;
    public GameObject HalfLapTrigger;

    public GameObject MinuteDisplay;
    public GameObject SecondDisplay;
    public GameObject MilliDisplay;

    public GameObject LapTimeBox;

    private void OnTriggerEnter(Collider other)
    {
        if (LapTimeManager.SecondCount <= 9 )
        {
            SecondDisplay.GetComponent<TMP_Text>().text = "0" + LapTimeManager.SecondCount + ".";
        } else
        {
            SecondDisplay.GetComponent<TMP_Text>().text = "" + LapTimeManager.SecondCount + ".";
        }

        if(LapTimeManager.MinuteCount <= 9)
        {
            MinuteDisplay.GetComponent<TMP_Text>().text = "0" + LapTimeManager.MinuteCount + ".";
        } else
        {
            MinuteDisplay.GetComponent<TMP_Text>().text = "" + LapTimeManager.MinuteCount + "."; 
        }
        MilliDisplay.GetComponent<TMP_Text>().text = "" + LapTimeManager.MilliCount;

        LapTimeManager.MinuteCount = 0;
        LapTimeManager.SecondCount = 0; 
        LapTimeManager.MilliCount = 0;

        HalfLapTrigger.SetActive(true);
        LapCompleteTrigger.SetActive(false);
    }
}
