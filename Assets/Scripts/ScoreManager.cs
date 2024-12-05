using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
   [SerializeField]
   private TMP_InputField inputName;

   public int currentScore = 0;

   public UnityEvent<string, int> submitNameEvent;

   public void SubmitName(){
    submitNameEvent.Invoke(inputName.text, currentScore);
   }
}
