using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    List<GameObject> levelTiles = new();

    //private void Awake()
    //{
    //    Instance = this;
    //}

    // Start is called before the first frame update
    void Start()
    {
        int row = 0;
        int col = 0;
        float scaleX = LevelCanvas.transform.localScale.x;
        Vector3 startPos = ReferenceTile.transform.position;

        for (int i = 1; i < 26; i++)
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

            newTile.GetComponent<Button>().GetComponentInChildren<TMP_Text>().text = i.ToString();
            newTile.GetComponent<Button>().onClick.AddListener(delegate { LoadSelectedLevel(newTile); });
            levelTiles.Add(newTile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
