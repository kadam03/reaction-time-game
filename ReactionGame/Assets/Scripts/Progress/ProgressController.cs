using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

[Serializable]
public class ProgressController
{
    public string ID;
    public int ReachedLevel;
    public float BestReaction;
    public List<LevelProgress> LevelProgresses;

    public void SaveGame(PlayerData pData, List<LevelData> lData)
    {
        ReachedLevel = pData.ReachedLevel;
        BestReaction = pData.BestReaction;
        LevelProgresses = new();

        foreach (var level in lData)
        {
            if (!level.IsLevelPassed)
            {
                break;
            }

            LevelProgresses.Add(new LevelProgress
            {
                LevelID = level.Level,
                MaxPoints = level.LevelHighScore,
                BestReaction = level.BestReaction
            });
        }

        string json = JsonConvert.SerializeObject(this);
        Debug.Log(json);
    }

    public static void LoadGame()
    {

    }
}
