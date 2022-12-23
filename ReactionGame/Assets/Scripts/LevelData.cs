using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public int LevelHighScore;
    public bool IsLevelPassed;
    public PassLevels PassLevel;
    public GameObject LevelButton;
    //public Color ButtonColor = Color.white;
    public Color ButtonColor;

    public bool CalculateLevelPass(int points)
    {
        LevelHighScore = points;
        if (LevelHighScore < LevelPassPoints)
        {
            IsLevelPassed = false;
            return false;
        }

        if (IsInRange(LevelHighScore, LevelPassPoints, SilverPassPoints))
        {
            PassLevel = PassLevels.Bronze;
            ButtonColor = new Color(0.7372549f, 0.4588235f, 0, 1);
        }
        else if (IsInRange(LevelHighScore, SilverPassPoints, GoldPassPoints))
        {
            PassLevel = PassLevels.Silver;
            ButtonColor = Color.gray;
        }
        else if (IsInRange(LevelHighScore, GoldPassPoints, 999999999))
        {
            PassLevel = PassLevels.Gold;
            ButtonColor = Color.yellow;
        }

        IsLevelPassed = true;
        PlayerData.Instance.ReachedLevel = Level;
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
