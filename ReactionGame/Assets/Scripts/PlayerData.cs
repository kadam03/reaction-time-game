using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

//[CreateAssetMenu(fileName = "Player", menuName = "ReactionGame/Player")]
public class PlayerData
{
    public static PlayerData Instance;
    public string PlayerName;
    public int ReachedLevel;
    public float BestReaction;
    public float BestAverageReaction;

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
