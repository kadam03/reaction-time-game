using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerData : ScriptableObject
{
    public static PlayerData Instance;
    public string PlayerName;
    public int ReachedLevel;
    public float BestReaction = Mathf.Infinity;
    public float BestAverageReaction = Mathf.Infinity;

    public void UpdateRactionResults(float best, float avg)
    {
        if (best < BestReaction)
        {
            BestReaction = best;
        }
        if (avg < BestAverageReaction)
        {
            BestAverageReaction = avg;
        }
    }
}
