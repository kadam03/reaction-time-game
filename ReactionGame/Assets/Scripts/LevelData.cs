using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ReactionGame/Level")]
public class LevelData : ScriptableObject
{
    public enum PassLevels
    {
        Bronze,
        Silver,
        Gold
    }

    public int Level;
    public bool IsTimeTrial;
    [Tooltip("Max available time in seconds (between 0 and 3540)")]
    [Range(0, 3540)]
    public int StartTime;
    [Range(0, 9999)]
    public int LevelPassPoints;
    [Range(0, 9999)]
    public int SilverPassPoints;
    [Range(0, 9999)]
    public int GoldPassPoints;
    public List<TileData> Tiles = new List<TileData>();

    public int FinishedPoints;
    public PassLevels PassLevel;

    public bool CalculateLevelPass(int points)
    {
        FinishedPoints = points;
        if (FinishedPoints < LevelPassPoints)
        {
            return false;
        }

        if (IsInRange(FinishedPoints, LevelPassPoints, SilverPassPoints))
        {
            PassLevel = PassLevels.Bronze;
        }
        else if (IsInRange(FinishedPoints, SilverPassPoints, GoldPassPoints))
        {
            PassLevel = PassLevels.Silver;
        }
        else if (IsInRange(FinishedPoints, GoldPassPoints, 999999999))
        {
            PassLevel = PassLevels.Gold;
        }

        return true;
    }

    private bool IsInRange(int point, int lowerLimit, int higherLimit)
    {
        if (point >= lowerLimit && point < higherLimit)
        {
            return true;
        }

        return false;
    }
}
