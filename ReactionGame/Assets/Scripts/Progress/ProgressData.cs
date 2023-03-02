using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProgressData
{
    // Player related info
    public string ID;
    public string PlayerName;
    public float BestReaction;

    // Level progress info
    public int ReachedLevel;
    [SerializeField]
    public List<LevelProgress> LevelProgresses;

    // General app info
    public string GameVersion;
    public DateTime SaveDate;
    public bool MutedGame;

    public string SerializeToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}
