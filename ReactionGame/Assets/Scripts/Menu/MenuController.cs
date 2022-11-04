using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance = null;
    public Toggle ToggleTimeTrial = null;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
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
        #else
                    Application.Quit();
        #endif
    }

    public void ToggleTimeTrialChanged()
    {
        // Todo implement appearing timebox to set the maximum time
    }
}
