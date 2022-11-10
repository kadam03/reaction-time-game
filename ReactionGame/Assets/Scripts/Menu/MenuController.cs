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
    public static bool IsTimeTrial = true;
    public static int TrialLength;

    // Start is called before the first frame update
    void Start()
    {
        //Instance = this;
        ToggleTimeTrial.isOn = IsTimeTrial;
        InputTimeTrialLength.SetActive(ToggleTimeTrial.isOn);
        TrialLength = int.Parse(InputTimeTrialLength.GetComponent<TMP_InputField>().text);
        TextVersion.text = "v" + Application.version;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
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
        // Todo implement appearing timebox to set the maximum time
        IsTimeTrial = ToggleTimeTrial.isOn;
        InputTimeTrialLength.SetActive(ToggleTimeTrial.isOn);
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
}
