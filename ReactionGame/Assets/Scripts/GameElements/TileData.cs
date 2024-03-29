using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "ReactionGame/Tile")]
public class TileData : ScriptableObject
{
    public enum Friendlyness
    {
        Friendly,
        PointKiller,
        Offensive,
        TimeKiller, // in practice mode this does not do anything, find out an alternative
        TimeHealer, // in practice mode this does not do anything, find out an alternative
        Neutral
    }

    public string TileName;
    public int RewardValue;
    public int MistakeValue;
    public Color TileColor;
    public bool Disappears;
    public float Time;
    public Friendlyness friendlyness;
    public AudioClip BreakEffect;
    [Range(0, 100)]
    public int Weight;
    [TextArea(3, 5)]
    public string Description;
}
