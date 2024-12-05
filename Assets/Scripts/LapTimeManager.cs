using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro; 

public class LapTimeManager : MonoBehaviour
{
    public static int MinuteCount;
    public static int SecondCount;
    public static float MilliCount;
    public static string MilliDisplay;

    public GameObject MinuteBox;
    public GameObject SecondBox;
    public GameObject MilliBox;

    private TMP_Text minuteText;
    private TMP_Text secondText;
    private TMP_Text milliText;

    void Awake()
    {
        if (MinuteBox == null) Debug.LogError("MinuteBox is not assigned in the Inspector.");
        if (SecondBox == null) Debug.LogError("SecondBox is not assigned in the Inspector.");
        if (MilliBox == null) Debug.LogError("MilliBox is not assigned in the Inspector.");

        minuteText = MinuteBox?.GetComponent<TMP_Text>();
        secondText = SecondBox?.GetComponent<TMP_Text>();
        milliText = MilliBox?.GetComponent<TMP_Text>();

        if (minuteText == null) Debug.LogError("MinuteBox does not have a TMP_Text component.");
        if (secondText == null) Debug.LogError("SecondBox does not have a TMP_Text component.");
        if (milliText == null) Debug.LogError("MilliBox does not have a TMP_Text component.");
    }

    void Update()
    {
        MilliCount += Time.deltaTime * 100;
        if (MilliCount >= 100)
        {
            MilliCount = 0;
            SecondCount += 1;
        }

        if (SecondCount >= 60)
        {
            SecondCount = 0;
            MinuteCount += 1;
        }

        MilliDisplay = ((int)MilliCount).ToString("D2");
        if (milliText != null)
            milliText.text = MilliDisplay;
        if (secondText != null)
            secondText.text = SecondCount.ToString("D2") + ".";
        if (minuteText != null)
            minuteText.text = MinuteCount.ToString("D2") + ":";
    }
}
