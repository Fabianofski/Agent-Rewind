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

        if (Player.position.x > 22)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void YSpawnCord()
    {
        Player.position = new Vector2(Player.position.x, YPlayerCor);
    }
}
