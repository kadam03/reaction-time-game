using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsController : MonoBehaviour
{
    public static LevelsController Instance;
    public GameObject ReferenceTile;
    public GameObject LevelCanvas;
    public GameObject TilePanel;
    public List<LevelData> Levels = new();

    int stepSize = 140;
    //readonly List<GameObject> levelTiles = new();

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        InstLvlButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            col++;
            if (col > 4)
            {
                col = 0;
                row--;
            }

            newTile.GetComponent<Button>().GetComponentInChildren<TMP_Text>().text = (i + 1).ToString();
            newTile.GetComponent<Button>().onClick.AddListener(delegate { LoadSelectedLevel(newTile); });
            if (Levels[i].IsLevelPassed)
            {
                newTile.transform.Find("Thick").gameObject.SetActive(true);
                //newTile.GetComponent<Image>().color = Color.black;
                newTile.GetComponent<Image>().color = Levels[i].ButtonColor;
            }
            //Levels[i].LevelButton = newTile;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevel(int level)
    {
        GameController.CurrentLevelData = Levels[level];
        SceneManager.LoadScene(1);
    }

    public void LoadSelectedLevel(GameObject go)
    {
        int level = int.Parse(go.GetComponent<Button>().GetComponentInChildren<TMP_Text>().text);
        LoadLevel(level - 1); // - 1 because of indexing (the level stores the real level number)
    }
}
