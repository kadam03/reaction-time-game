using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
    List<string> tileSet = new();

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        tileSet.Clear();
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

    public void LoadLevel(int level)
    {
        if (level == 1 || Levels[level - 2].IsLevelPassed)
        {
            GameController.CurrentLevelData = Levels[level - 1]; // - 1 because of indexing (the level stores the real level number)
            SceneManager.LoadScene(1);
        }
    }

    public void LoadSelectedLevel(GameObject go)
    {
        int level = int.Parse(go.GetComponent<Button>().GetComponentInChildren<TMP_Text>().text);
        LoadLevel(level);
    }

    // Lame weighted random generator, might be improved in the future (or not, like evething else like this :D)
    public TileData WeightedRandomTile(LevelData ld)
    {
        if (tileSet.Count <= 0)
        {
            foreach (TileData tile in ld.Tiles)
            {
                for (int i = 0; i < tile.Weight; i++)
                {
                    tileSet.Add(tile.TileName);
                }
            }
        }

        int place = Random.Range(0, tileSet.Count);
        return ld.Tiles.First(x => x.TileName.Equals(tileSet[place]));
    }
}
