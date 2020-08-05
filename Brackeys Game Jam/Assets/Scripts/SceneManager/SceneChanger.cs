using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Transform Player;
    private float YPlayerCor;

    private void Update()
    {

        if (Player.transform.position.x > 22)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void YSpawnCord()
    {
        Player.transform.position.y = YPlayerCor;
    }
}
