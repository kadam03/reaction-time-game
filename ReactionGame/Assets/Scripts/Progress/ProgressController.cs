using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Windows;

public class ProgressController
{
    public static ProgressController Instance;
    public ProgressData ProgData;

    private string progressFileName = "progress.json";

    public ProgressController()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SaveGame(PlayerData pData, List<LevelData> lData)
    {
        ProgData = new();
        ProgData.GameVersion = Application.version;
        ProgData.SaveDate = DateTime.Now;
        ProgData.ReachedLevel = pData.ReachedLevel;
        ProgData.BestReaction = pData.BestReaction;
        ProgData.LevelProgresses = new();

        foreach (var level in lData)
        {
            if (!level.IsLevelPassed)
            {
                break;
            }

            ProgData.LevelProgresses.Add(new LevelProgress
            {
                LevelID = level.Level,
                MaxPoints = level.LevelHighScore,
                BestReaction = level.BestReaction
            });
        }

        SaveDataFile(ProgData.SerializeToJson());
    }

    private void SaveDataFile(string json)
    {
        string filePath = Application.persistentDataPath + @"\" + progressFileName;
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
        
        System.IO.File.WriteAllText(filePath, json);
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + @"\" + progressFileName;
        if (System.IO.File.Exists(filePath))
        {
            using (StreamReader reader = System.IO.File.OpenText(filePath))
            {
                JsonSerializer ser = new();
                ProgData = (ProgressData)ser.Deserialize(reader, typeof(ProgressData));
            }
        }
    }

    public void LoadPlayerData(PlayerData pdata)
    {
        if (ProgData != null)
        {
            SetPlayerData(pdata);
        }
    }

    private void SetPlayerData(PlayerData pData)
    {
            pData.PlayerName = ProgData.PlayerName;
            pData.ReachedLevel = ProgData.ReachedLevel;
            pData.BestReaction = ProgData.BestReaction;
    }

    public void LoadLevelData(List<LevelData> lData)
    {
        if (ProgData != null)
        {
            SetLevelData(lData);
        }
    }

    private void SetLevelData(List<LevelData> lData)
    {
        foreach (var level in ProgData.LevelProgresses)
        {
            lData[level.LevelID - 1].BestReaction = level.BestReaction;
            lData[level.LevelID - 1].CalculateLevelPass(level.MaxPoints);
        }
    }


}
