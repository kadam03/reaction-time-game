using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class LevelsController : MonoBehaviour
{
    public static LevelsController Instance = new();
    public GameObject ReferenceTile;
    public GameObject LevelCanvas;
    public GameObject TilePanel;
    public List<LevelData> Levels = new();

    int stepSize = 140;

    private void Awake()
    {
        CreateInstance();
    }

    // Start is called before the first frame update
    void Start()
    {
        InstLvlButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        ProgressController.Instance.LoadLevelData(Levels);
    }

    private void InstLvlButtons()
    {
        int row = 0;
        int col = 0;
        float scaleX = LevelCanvas.transform.localScale.x;
        Vector3 startPos = ReferenceTile.transform.position;

        for (int i = 0; i < Levels.Count; i++)
        {
            GameObject newTile = Instantiate(ReferenceTile, startPos + (new Vector3(stepSize * col, stepSize * row) * scaleX), Quaternion.identity);
            newTile.SetActive(true);
            newTile.transform.SetParent(LevelCanvas.transform);
            newTile.transform.localScale = new Vector3(1, 1, 1);
            newTile.GetComponent<Button>().GetComponentInChildren<TMP_Text>().text = (i + 1).ToString();
            newTile.GetComponent<Button>().onClick.AddListener(delegate { LoadSelectedLevel(newTile); });
            newTile.GetComponent<Image>().color = Levels[i].ButtonColor;

            if (Levels[i].IsLevelPassed)
            {
                newTile.transform.Find("Thick").gameObject.SetActive(true);
            }
            else if (i > 0 && !Levels[i - 1].IsLevelPassed)
            {
                newTile.GetComponent<Image>().color = Color.black;
            }

            StepTileLocation(ref col, ref row);
        }
    }

    private void StepTileLocation(ref int col, ref int row)
    {
        col++;
        if (col > 4)
        {
            col = 0;
            row--;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevel(int level) // Real level number not index. If you want to load lvl 8, pass 8 as parameter
    {
        if (level == 1 || Levels[level - 2].IsLevelPassed)
        {
            GameController.CurrentLevelData = Levels[level - 1]; 
            SceneManager.LoadScene(1);
        }
    }

    private void LoadSelectedLevel(GameObject go)
    {
        int level = int.Parse(go.GetComponent<Button>().GetComponentInChildren<TMP_Text>().text);
        LoadLevel(level);
    }

    public void SaveLevelData()
    {
        
    }
}
