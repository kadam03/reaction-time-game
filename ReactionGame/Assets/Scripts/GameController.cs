using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;
    public TMP_Text TextTimer = null;
    public TMP_Text TextPoints = null;
    public RTimer GameTimer = null;
    
    public GameObject TilePrefab = null;
    public GameObject PauseMenu = null;
    public GameObject PauseShader = null;
    public bool isPaused = false;
    public GameObject[] Positions = new GameObject[5];

    Tile currentTile;
    bool tileVisible;
    int currentIndex;
    int prevIndex;
    int points = 0;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        points = 0;
        tileVisible = false;
        currentIndex = 0;
        prevIndex = 0;
        currentTile = null;
        GameTimer.StartTimer();
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            return;
        }

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
        if (!isPaused)
        {
            points++;
            TextPoints.text = points.ToString();
            tileVisible = false;
            Destroy(currentTile.gameObject);
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        PauseShader.SetActive(isPaused);
        PauseMenu.SetActive(isPaused);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
