using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public List<AIRacer> racers = new List<AIRacer>();

    void Update()
    {
        UpdateRacerPositions();
        AdjustAISpeed();
    }

    private void UpdateRacerPositions()
    {
        
        racers.Sort((r1, r2) => r2.progress.CompareTo(r1.progress));

       
        for (int i = 0; i < racers.Count; i++)
        {
            racers[i].position = i + 1;
        }
    }

    private void AdjustAISpeed()
    {
        foreach (var racer in racers)
        {
            if (racer.position == 1)
            {
                racer.AdjustSpeed(0.95f);  
            }
            else if (racer.position == racers.Count) 
            {
                racer.AdjustSpeed(1.1f);  
            }
            else
            {
                racer.AdjustSpeed(1.0f);  
            }
        }
    }
}
