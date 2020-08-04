using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    public GameObject pause;
    public GameObject pauseButton;

    private Keyboard kb;

    void Awake()
    {
        kb = InputSystem.GetDevice<Keyboard>();
    }

    void Update()
    {
        if (kb.rKey.wasReleasedThisFrame)
        {
            Restart();
        }

        if (kb.escapeKey.wasReleasedThisFrame)
        {
            if (pause.activeSelf)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pause.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pause.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

}
