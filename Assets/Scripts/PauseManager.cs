using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject musicUI;
    public bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            UnPause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseUI.SetActive(true);
        musicUI.SetActive(false);
        isPaused = true;
    }
    public void UnPause()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        musicUI.SetActive(true);
        isPaused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
