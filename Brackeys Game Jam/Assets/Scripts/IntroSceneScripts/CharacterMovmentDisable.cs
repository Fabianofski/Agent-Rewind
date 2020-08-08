using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovmentDisable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject varGameObject = GameObject.Find("Player");
        GameObject aniGameObject = GameObject.Find("GameObject");
        varGameObject.GetComponent<PlayerMovementController>().enabled = false;
        aniGameObject.GetComponent<Animator>().enabled = false;
        Debug.Log("Player Movment False");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
