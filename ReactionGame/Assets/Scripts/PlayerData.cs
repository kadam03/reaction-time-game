using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ReactionGame/Player")]
public class PlayerData : ScriptableObject
{
    public string PlayerName = "";
    public int ReachedLevel;
    public float BestReaction;
    public float BestAverageReaction;
}
