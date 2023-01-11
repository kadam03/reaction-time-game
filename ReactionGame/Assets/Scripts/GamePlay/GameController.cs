using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;
    public static int StartTime = 20; // todo: set based on the menu setting
    public static LevelData CurrentLevelData = null;
    public TMP_Text TextTimer = null;
    public TMP_Text TextPoints = null;
    public TMP_Text TextBestReaction = null;
    public TMP_Text TextGameOverResult = null;
    public TMP_Text TextBestReactionResult = null;
    public TMP_Text TextAVGReactionResult = null;
    public TMP_Text TextGameOver = null;
    public TMP_Text TextLevel = null;
    public TMP_Text TextCountDown = null;
    public RTimer GameTimer = null;
    public bool IsTimeTrial = true;
    public bool IsPaused = false;

    public GameObject TilePrefab = null;
    public GameObject PauseMenu = null;
    public GameObject GameOverMenu = null;
    public GameObject PauseShader = null;
    public GameObject SpawnArea = null;
    public GameObject GameCanvas = null;
    public GameObject BtnNextLevel = null;

    Tile currentTile;
    bool tileVisible;
    bool isCountDown;
    int points = 0;
    float bestReaction = Mathf.Infinity;
    float avgReaction = 0;
    int numberOfCatched = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        if (PlayerData.Instance == null)
        {
            PlayerData.Instance = ScriptableObject.CreateInstance<PlayerData>();  // once save and load are implemented here the play data will be loaded
        }
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

        if (isCountDown)
        {
            TextCountDown.text = GameTimer.RemainSeconds.ToString();
            if (GameTimer.RemainTime <= 1)
            {
                isCountDown = false;
                TextCountDown.gameObject.SetActive(false);
                GameTimer.ResetTimer(StartTime);
                GameTimer.StartTimer();
            }

            return;
        }

        if (IsTimeTrial && GameTimer.RemainTime <= 0)
        {
            GameOver();
            return;
        }

        UpdateTimeText();
        SpawnTileRandom();
    }

    public void RestartGame()
    {
        ResetGame();
    }

    void ResetGame()
    {
        isCountDown = true;
        LoadLevel(CurrentLevelData);
        points = 0;
        TextPoints.text = 0.ToString();
        tileVisible = false;
        IsPaused = false;
        Time.timeScale = IsPaused ? 0 : 1;
        bestReaction = Mathf.Infinity;
        TextBestReaction.text = "";
        PauseShader.SetActive(false);
        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        numberOfCatched = 0;
        avgReaction = 0;
        CountDown();
    }

    private void CountDown()
    {
        GameTimer.ResetTimer(4);
        GameTimer.StartTimer();
        TextCountDown.gameObject.SetActive(true);
    }

    void LoadLevel(LevelData levelData)
    {
        IsTimeTrial = levelData.IsTimeTrial;
        StartTime = levelData.StartTime;
        TextLevel.text = levelData.Level.ToString();
    }
    public void NextLevel()
    {
        if (CurrentLevelData.Level < LevelsController.Instance.Levels.Count)
        {
            LevelsController.Instance.LoadLevel(CurrentLevelData.Level + 1);
        }
    }

    public void PauseGame()
    {
        StopStartGame();
        PauseMenu.SetActive(IsPaused);
    }

    public void TileCatched(GameObject tile)
    {
        TileData td = tile.GetComponent<Tile>().tileData;
        if (!IsPaused)
        {
            switch (td.friendlyness)
            {
                case TileData.Friendlyness.Friendly:
                    points += td.RewardValue;
                    break;
                case TileData.Friendlyness.PointKiller:
                    points += td.RewardValue;
                    break;
                case TileData.Friendlyness.TimeKiller:
                    GameTimer.RemainTime -= td.MistakeValue;
                    break;
                case TileData.Friendlyness.TimeHealer:
                    GameTimer.RemainTime += td.RewardValue;
                    break;
                default:
                    break;
            }

            UpdateStats();
            KillTile(currentTile.gameObject);
        }
    }

    public void MissClick()
    {
        if (!IsPaused)
        {
            if (currentTile.tileData.friendlyness == TileData.Friendlyness.TimeKiller)
            {
                points++;
                UpdateStats();
                KillTile(currentTile.gameObject);
            }
            else
            {
                points--;
            }

            TextPoints.text = points.ToString();
        }
    }

    public void TileDisappeared(GameObject tile)
    {
        TileData td = tile.GetComponent<Tile>().tileData;
        if (!IsPaused)
        {
            switch (td.friendlyness)
            {
                case TileData.Friendlyness.PointKiller:
                    points -= td.MistakeValue;
                    break;
                default:
                    break;
            }

            KillTile(currentTile.gameObject);
        }
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    void UpdateStats()
    {
        numberOfCatched++;
        CalcAverageReact();
        CheckAndUpdateBestReact();
    }

    void CheckAndUpdateBestReact()
    {
        if (currentTile.Timer.SecondsWithDecimals < bestReaction)
        {
            bestReaction = currentTile.Timer.SecondsWithDecimals;
            TextBestReaction.text = string.Format("{0:0.00}", bestReaction) + "s";
        }
    }

    void StopStartGame()
    {
        IsPaused = !IsPaused;
        if (currentTile != null)  // at the beginning of the game, during countdown there is no tile
        {
            currentTile.gameObject.SetActive(!IsPaused); // ugly hotfix so the tile does not cover the pause or game over menu
        }
        Time.timeScale = IsPaused ? 0 : 1;
        PauseShader.SetActive(IsPaused);
    }

    void GameOver()
    {
        PlayerData.Instance.UpdateRactionResults(bestReaction, avgReaction);

        if (CurrentLevelData.CalculateLevelPass(points))
        {
            TextGameOver.text = "Level Done!";
            BtnNextLevel.SetActive(true);
        }
        else
        {
            TextGameOver.text = "Game Over!";
            BtnNextLevel.SetActive(false);
        }

        SaveProgress();

        TextTimer.text = "00:00.00";
        TextBestReactionResult.text = TextBestReaction.text;
        TextAVGReactionResult.text = string.Format("{0:0.00}", avgReaction) + "s";
        StopStartGame();
        GameOverMenu.SetActive(true);
        Destroy(currentTile.gameObject);
        TextGameOverResult.text = points.ToString();
    }
    void KillTile(GameObject gO)
    {
        TextPoints.text = points.ToString();
        tileVisible = false;
        Destroy(gO);
    }

    void CalcAverageReact()
    {
        avgReaction += currentTile.Timer.SecondsWithDecimals;
        avgReaction /= 2f;
    }

    void UpdateTimeText()
    {
        if (IsTimeTrial & GameTimer.RemainTime >= 0)
        {
            TextTimer.text = string.Format("{0:00}:{1:00.00}", GameTimer.RemainMinutes, GameTimer.RemainSecondsWithDecimals);
        }
        else
        {
            TextTimer.text = string.Format("{0:00}:{1:00.00}", GameTimer.Minutes, GameTimer.SecondsWithDecimals);
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
            currentTile.tileData = LevelsController.Instance.WeightedRandomTile(CurrentLevelData);
            currentTile.transform.SetParent(GameCanvas.transform);

            currentTile.transform.localScale = new Vector3(currentTile.GetComponent<RectTransform>().localScale.x * refScaleX, currentTile.GetComponent<RectTransform>().localScale.y * refScaleY, GameCanvas.GetComponent<RectTransform>().localScale.z);

            tileVisible = true;
        }
    }

    void SaveProgress()
    {
        CurrentLevelData.BestReaction = PlayerData.Instance.BestReaction;
        //ProgressController pCont = new();
        ProgressController.Instance.SaveGame(PlayerData.Instance, LevelsController.Instance.Levels);
    }
}
