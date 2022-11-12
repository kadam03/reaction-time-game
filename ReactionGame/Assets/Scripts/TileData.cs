using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "ReactionGame/Tile")]
public class TileData : ScriptableObject
{
    public string TileName;
    public int PointValue;
    public int MinusPoints;
    public Color TileColor;
    public bool Disappears;
    public float Time;

}
