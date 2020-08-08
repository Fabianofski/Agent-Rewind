using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class FMOD_footsteps : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string walkingevent;
    public string crouchingevent;
    bool playerismoving;
    bool playeriscrouching;
    public float walkingspeed;
    public float runningspeed;
    public Vector2 direction;


    void Update()
    {
        if (direction !=Vector2.zero)
        {
            playerismoving = true;
        }
    }

    void CallFootsteps ()
    {
        if (playerismoving == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot(walkingevent);
        }
    }

    void Start()
    {
        InvokeRepeating("CallFootsteps", 0, walkingspeed);
        InvokeRepeating("CallFootsteps", 0, runningspeed);
    }

    void OnDisable()
    {
        playerismoving = false;
    }
}
