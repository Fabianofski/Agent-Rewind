using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transisitonTime = 1f;

    void LoadNextLevelDetector()
    {
        LoadNextLevel();
        Debug.Log("Loadcalled");
    }


    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transisitonTime);

        SceneManager.LoadScene(levelIndex);
    }
}
