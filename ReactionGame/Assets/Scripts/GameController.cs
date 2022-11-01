using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;
    public TMP_Text TextTimer = null;
    public TMP_Text TextPoints = null;
    public RTimer GameTimer = null;
    public int Points = 0;
    public GameObject TilePrefab = null;
    public GameObject[] Positions = new GameObject[5];

    Tile currentTile;
    bool tileVisible;
    int currentIndex;
    int prevIndex;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Points = 0;
        tileVisible = false;
        currentIndex = 0;
        prevIndex = 0;
        currentTile = null;
        GameTimer.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        TextTimer.text = string.Format("{0:00}:{1:00}", GameTimer.Minutes, GameTimer.Seconds);

        if (tileVisible == false)
        {
            while(currentIndex == prevIndex)
            {
                currentIndex = Random.Range(0, 5);
            }

            currentTile = Instantiate(TilePrefab, Positions[currentIndex].transform.position, Quaternion.identity).GetComponent<Tile>();
            currentTile.transform.SetParent(GameObject.FindWithTag("Canvas").transform);
            currentTile.transform.localScale = new Vector3(1, 1, 1);
        }

        tileVisible = true;
        prevIndex = currentIndex;
    }

    public void TileCatched()
    {
        Points++;
        TextPoints.text = Points.ToString();
        tileVisible = false;
        Destroy(currentTile.gameObject);
    }
}
