using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProgressData
{
    public string ID;
    public string PlayerName;
    public int ReachedLevel;
    public float BestReaction;
    [SerializeField]
    public List<LevelProgress> LevelProgresses;

    public string SerializeToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}
