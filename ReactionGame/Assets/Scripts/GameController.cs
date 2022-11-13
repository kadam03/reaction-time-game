using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;
    public static int StartTime = 10; // todo: set based on the menu setting
    public TMP_Text TextTimer = null;
    public TMP_Text TextPoints = null;
    public TMP_Text TextBestReaction = null;
    public TMP_Text TextGameOverResult = null;
    public RTimer GameTimer = null;
    public bool IsTimeTrial = true;
    public bool IsPaused = false;

    public GameObject TilePrefab = null;
    public GameObject PauseMenu = null;
    public GameObject GameOverMenu = null;
    public GameObject PauseShader = null;
    public GameObject SpawnArea = null;
    public GameObject GameCanvas = null;
    public List<TileData> TileTypes = new List<TileData>();
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
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
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
        SpawnTileRandom();
    }

    void ResetGame()
    {
        points = 0;
        TextPoints.text = 0.ToString();
        tileVisible = false;
        currentIndex = 0;
        prevIndex = 0;
        GameTimer.ResetTimer(StartTime);
        GameTimer.StartTimer();
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
            points += tile.GetComponent<Tile>().tileData.PointValue;
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

    public void TileDisappeared(GameObject tile)
    {
        if (!IsPaused)
        {
            points -= tile.GetComponent<Tile>().tileData.MinusPoints;
            TextPoints.text = points.ToString();
            tileVisible = false;
            Destroy(currentTile.gameObject);
        }
    }

    public void MissClick()
    {
        points--;
        TextPoints.text = points.ToString();
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

    void SpawnTileDefinedLoc()
    {
        if (tileVisible == false)
        {
            while (currentIndex == prevIndex)
            {
                currentIndex = Random.Range(0, 5);
            }

            currentTile = Instantiate(TilePrefab, Positions[currentIndex].transform.position, Quaternion.identity).GetComponent<Tile>();
            currentTile.transform.SetParent(GameCanvas.transform);
            currentTile.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            tileVisible = true;
            prevIndex = currentIndex;
        }
    }

    void SpawnTileRandom()
    {
        if (tileVisible == false)
        {
            Vector3 pos = new Vector3();
            RectTransform rt = SpawnArea.GetComponent<RectTransform>();

            float refScaleX = GameCanvas.GetComponent<RectTransform>().localScale.x;
            float refScaleY = GameCanvas.GetComponent<RectTransform>().localScale.y;

            pos.x = Random.Range(rt.rect.xMin, rt.rect.xMax) * refScaleX;
            pos.y = Random.Range(rt.rect.yMin, rt.rect.yMax) * refScaleY;

            currentTile = Instantiate(TilePrefab, pos + rt.transform.position, Quaternion.identity).GetComponent<Tile>();
            currentTile.tileData = TileTypes[Random.Range(0, 2)];
            currentTile.transform.SetParent(GameCanvas.transform);

            currentTile.transform.localScale = new Vector3(currentTile.GetComponent<RectTransform>().localScale.x * refScaleX, currentTile.GetComponent<RectTransform>().localScale.y * refScaleY, GameCanvas.GetComponent<RectTransform>().localScale.z);

            tileVisible = true;
        }
    }
}
