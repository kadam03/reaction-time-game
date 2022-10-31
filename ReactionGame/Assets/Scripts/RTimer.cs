using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTimer : MonoBehaviour
{
    public int Hours = 0;
    public int Minutes = 0;
    public int Seconds = 0;
    public int DecimalSec = 0;

    float gameTime = 0;
    bool started;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (started == true)
        {
            gameTime += Time.deltaTime;
            Seconds = (int)gameTime % 60;
            Minutes = (int)gameTime / 60;
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
        gameTime = 0;
        Hours = 0;
        Minutes = 0;
        Seconds = 0;
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
}
