using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMOD_code : MonoBehaviour
{
    [FMODUnity.EventRef]
    public bool PlayOnAwake;

    public void PlayCode()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Interactables/code_entering", gameObject);
    }
    void Start()
    {
        if (PlayOnAwake)
            PlayCode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
