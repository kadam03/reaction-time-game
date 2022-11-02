using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTimer : MonoBehaviour
{
    public int Hours = 0;
    public int Minutes = 0;
    public int Seconds = 0;
    public float DecimalSec = 0;
    public float SecondsWithDecimals = 0;

    float elapsedTime = 0;
    bool started;

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (started == true)
        {
            elapsedTime += Time.deltaTime;
            DecimalSec = elapsedTime - (int)elapsedTime;
            Seconds = (int)elapsedTime % 60;
            SecondsWithDecimals = Seconds + DecimalSec;
            Minutes = (int)elapsedTime / 60;
            Hours = Minutes / 60;
        }

        //if (started == false)
        //{
        //    Hours = Hours;
        //}
    }

    public void StartTimer()
    {
        started = true;
    }

    public void StopTimer()
    {
        started = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0;
        Hours = 0;
        Minutes = 0;
        Seconds = 0;
        DecimalSec = 0;
        SecondsWithDecimals = 0;
    }

    public ObjectTime GetElapsetTime()
    {
        return new ObjectTime()
        {
            Hours = Hours,
            Minutes = Minutes,
            Seconds = Seconds
        };
    }

    public void PauseTimer()
    {

    }
}
