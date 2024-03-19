using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseUI;
    public GameObject settingUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }    
        }
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        settingUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    void Paused()
    {
        pauseUI.SetActive(true);
        settingUI.SetActive(false);
        Time.timeScale = 0f;
        gamePaused = true;
    }
    public void Settings()
    {
        settingUI.SetActive(true);
        pauseUI.SetActive(false);
    }

    public void QuitApp()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        Application.Quit();
    }

}
