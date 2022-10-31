using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TMP_Text TextTimer = null;
    public TMP_Text TextPoints = null;
    public RTimer GameTimer = null;
    public int Points = 0;
    public Button[] Tiles = new Button[5];
    //public Button 

    bool tileVisible = false;
    //float gameTime = 0;
    //int minutes = 0;
    //int seconds = 0;
    //int decimalSec = 0;
    int currentIndex = 0;
    int prevIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        Points = 0;
        //gameTime = 0;
        GameTimer.StartTimer();
        foreach (Button item in Tiles)
        {
            item.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //gameTime += Time.deltaTime;
        //seconds = (int)gameTime % 60;
        //minutes = (int)gameTime / 60;
        TextTimer.text = string.Format("{0:00}:{1:00}", GameTimer.Minutes, GameTimer.Seconds);

        if (tileVisible == false)
        {
            while (currentIndex == prevIndex)
            {
                currentIndex = Random.Range(0, 5);
            }

            Tiles[currentIndex].gameObject.SetActive(true);
            tileVisible = true;
            prevIndex = currentIndex;
        }
    }

    public void TileCatched()
    {
        Points++;
        TextPoints.text = Points.ToString();
        tileVisible = false;
        foreach (Button item in Tiles)
        {
            item.gameObject.SetActive(false);
        }
    }
}
