using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{

    InputMaster1 controls;
    public bool Quit;

    void Awake()
    {
        controls = new InputMaster1();

        controls.IntroScene.nextText.performed += _ => NextScen();

        if (Quit)
        {
            controls.IntroScene.nextText.performed += _ => QuitApp();
        }
    }

    void NextScen()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void QuitApp()
    {
        Application.Quit();
    }

    void OnEnable()
    {
        controls.Enable();
    }

}
