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
        // Sort racers by progress to determine positions
        racers.Sort((r1, r2) => r2.progress.CompareTo(r1.progress));

        // Assign positions based on sorted list
        for (int i = 0; i < racers.Count; i++)
        {
            racers[i].position = i + 1;
        }
    }

    private void AdjustAISpeed()
    {
        foreach (var racer in racers)
        {
            if (racer.position == 1) // Leading racer
            {
                racer.AdjustSpeed(0.95f);  // Slightly reduce speed
            }
            else if (racer.position == racers.Count) // Last place
            {
                racer.AdjustSpeed(1.1f);  // Slight boost
            }
            else
            {
                racer.AdjustSpeed(1.0f);  // Normal speed
            }
        }
    }
}
