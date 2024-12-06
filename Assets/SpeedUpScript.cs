using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpScript : MonoBehaviour
{
    public int boostMultiplier = 20; // Factor by which to increase speed
    public int boostDuration = 5;  // Duration of the speed boost
    public CarControllerFinal carController;
    private void OnTriggerEnter(Collider other)
    {
            StartCoroutine(carController.SpeedBoost(boostMultiplier, boostDuration));
        
    }
}