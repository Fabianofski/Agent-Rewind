using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    public GameObject pause;
    public GameObject pauseButton;
    public InputMaster1 controls;

    private Keyboard kb;

    void Awake()
    {
        controls = new InputMaster1();

        controls.Player.Back.performed += _ => Back();
        controls.Player.Restart.performed += _ => Restart();
    }

    void Back()
    {

        if (pause.activeSelf)
            Resume();
        else
            Pause();

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

    // for new Input System
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

}
