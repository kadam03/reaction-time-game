using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    //public static MenuController Instance = null;
    public Toggle ToggleTimeTrial = null;
    public GameObject InputTimeTrialLength = null;
    public TMP_Text TextVersion = null;
    public GameObject CanvasMenu = null;
    public GameObject[] DecorTiles = new GameObject[4];

    public static bool IsTimeTrial = true;
    public int TrialLength;

    int[] decorDirections = new int[4] { 1, -1, -1, 1 };
    float tempDtime;

    // Start is called before the first frame update
    void Start()
    {
        PlayerData.Instance = ScriptableObject.CreateInstance<PlayerData>();
        //LevelsController.Instance = ScriptableObject.CreateInstance<LevelsController>();
        tempDtime = 0;
        ToggleTimeTrial.isOn = IsTimeTrial;
        TrialLength = int.Parse(InputTimeTrialLength.GetComponent<TMP_InputField>().text);
        TextVersion.text = "v" + Application.version;
    }

    // Update is called once per frame
    void Update()
    {
        tempDtime += Time.deltaTime;
        if (tempDtime >= 0.015)
        {
            MoveDecorTiles();
            tempDtime = 0;
        }
    }

    public void ViewLevels()
    {
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        // handling the application differently in case of editor run or normal build run
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif

        #if UNITY_WEBGL
            return;
        #else
            Application.Quit();
        #endif
    }

    public void ToggleTimeTrialChanged()
    {
        IsTimeTrial = ToggleTimeTrial.isOn;
        InputTimeTrialLength.SetActive(ToggleTimeTrial.isOn);
    }

    public void ClickHelp()
    {
        SceneManager.LoadScene(3);
    }

    public void StoreTimeData()
    {
        TrialLength = int.Parse(InputTimeTrialLength.GetComponent<TMP_InputField>().text);
    }

    public void EditEnded()
    {
        int tmp = int.Parse(InputTimeTrialLength.GetComponent<TMP_InputField>().text);
        if (tmp < 5)
        {
            TrialLength = 5;
            InputTimeTrialLength.GetComponent<TMP_InputField>().text = TrialLength.ToString();
        }
    }

    void MoveDecorTiles()
    {
        for (int i = 0; i < DecorTiles.Length; i++)
        {
            // In case it's even, move up & down
            if (i % 2 == 0)
            {
                MoveInDirection(i, DecorTiles[i].transform.position.y, 0, 1, CanvasMenu.GetComponent<RectTransform>().rect.yMin, CanvasMenu.GetComponent<RectTransform>().rect.yMax, CanvasMenu.GetComponent<RectTransform>().localScale.y);
            }
            // if odd, move left & right
            else
            {
                MoveInDirection(i, DecorTiles[i].transform.position.x, 1, 0, CanvasMenu.GetComponent<RectTransform>().rect.xMin, CanvasMenu.GetComponent<RectTransform>().rect.xMax, CanvasMenu.GetComponent<RectTransform>().localScale.x);
            }
        }
    }

    void MoveInDirection(int index, float pos, int stepX, int stepY, float min, float max, float scale)
    {
        float speed = decorDirections[index] * (0.8f + index / 10f);
        DecorTiles[index].transform.Translate(stepX * speed, stepY * speed, 0); // up & down

        if (pos > max * scale * 1.2)
        {
            decorDirections[index] = -1;
        }
        if (pos < min * scale * 1.2)
        {
            decorDirections[index] = 1;
        }
    }
}
