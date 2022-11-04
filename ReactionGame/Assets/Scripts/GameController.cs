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
    public TMP_Text TextBestReaction = null;
    public TMP_Text TextGameOverResult = null;
    public RTimer GameTimer = null;
    public float StartTime = 10f; // todo: set based on the menu setting
    public bool IsTimeTrial = true;
    public bool IsPaused = false;

    public GameObject TilePrefab = null;
    public GameObject PauseMenu = null;
    public GameObject GameOverMenu = null;
    public GameObject PauseShader = null;
    public GameObject[] Positions = new GameObject[5];

    Tile currentTile;
    bool tileVisible;
    int currentIndex;
    int prevIndex;
    int points = 0;
    float bestReaction = Mathf.Infinity;
    


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        ResetGame();
        IsTimeTrial = MenuController.IsTimeTrial;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPaused)
        {
            return;
        }

        if (IsTimeTrial && GameTimer.RemainTime <= 0)
        {
            TextTimer.text = "00:00.00";
            GameOver();
        }

        UpdateTimeText();

        if (tileVisible == false)
        {
            while(currentIndex == prevIndex)
            {
                currentIndex = Random.Range(0, 5);
            }

            currentTile = Instantiate(TilePrefab, Positions[currentIndex].transform.position, Quaternion.identity).GetComponent<Tile>();
            currentTile.transform.SetParent(GameObject.FindWithTag("Canvas").transform);
            currentTile.transform.localScale = new Vector3(1, 1, 1);

            tileVisible = true;
            prevIndex = currentIndex;
        }
    }

    void ResetGame()
    {
        points = 0;
        TextPoints.text = 0.ToString();
        tileVisible = false;
        currentIndex = 0;
        prevIndex = 0;
        GameTimer.StartTimer();
        GameTimer.RemainTime = StartTime;
        IsPaused = false;
        Time.timeScale = IsPaused ? 0 : 1;
        bestReaction = Mathf.Infinity;
        TextBestReaction.text = "";
        PauseShader.SetActive(false);
        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
    }

    public void RestartGame()
    {
        ResetGame();
    }

    public void PauseGame()
    {
        StopStartGame();
        PauseMenu.SetActive(IsPaused);
    }

    void StopStartGame()
    {
        IsPaused = !IsPaused;
        currentTile.gameObject.SetActive(!IsPaused); // ugly hotfix so the tile does not cover the pause or game over menu
        Time.timeScale = IsPaused ? 0 : 1;
        PauseShader.SetActive(IsPaused);
    }

    private void GameOver()
    {
        StopStartGame();
        GameOverMenu.SetActive(true);
        Destroy(currentTile.gameObject);
        TextGameOverResult.text = points.ToString();
    }

    public void TileCatched(GameObject tile)
    {
        if (!IsPaused)
        {
            points++;
            if (currentTile.Timer.SecondsWithDecimals < bestReaction)
            {
                bestReaction = currentTile.Timer.SecondsWithDecimals;
                TextBestReaction.text = string.Format("{0:0.00}", bestReaction) + "s";
            }
            TextPoints.text = points.ToString();
            tileVisible = false;
            Destroy(currentTile.gameObject);
        }
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    void UpdateTimeText()
    {
        if (IsTimeTrial)
        {
            TextTimer.text = string.Format("{0:00}:{1:00.00}", GameTimer.RemainMinutes, GameTimer.RemainSecondsWithDecimals);
        }
        else
        {
            TextTimer.text = string.Format("{0:00}:{1:00.00}", GameTimer.Minutes, GameTimer.SecondsWithDecimals);
        }
    }



}
