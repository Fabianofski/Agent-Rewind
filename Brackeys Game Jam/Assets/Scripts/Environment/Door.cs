using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public bool open;

    private SpriteRenderer sr;
    private BoxCollider2D bc2d;
    private int index;

    private Rewind rewind;
    [HideInInspector]
    public bool Rewinding;

    void Awake()
    {
        // Get Components
        rewind = GetComponent<Rewind>();
        sr = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Open and Close Door
        if (open)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        if (sr.enabled)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Interactables/door_open", transform.position);            
        }

        // When Door is opened disable Sprite and Collider
        sr.enabled = false;
        bc2d.enabled = false;
    }

    public void CloseDoor()
    {
        if (!sr.enabled)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Interactables/door_close", transform.position);
        }

        // When Door is closed enable Sprite and Collider
        sr.enabled = true;
        bc2d.enabled = true;
    }

    public void StartRewind(List<bool> OpenList)
    {
        // Start Rewind and Coroutine
        if (!Rewinding)
        {
            StartCoroutine(Replay(rewind.SaveOffset/2, OpenList));
        }

        index = 0;
        Rewinding = true;
    }

    public void StopRewind()
    {
        // Stop Rewind and all Coroutines
        Rewinding = false;
        StopAllCoroutines();
    }

    IEnumerator Replay(float time, List<bool> OpenList)
    {
        // set open to List input after time seconds
        yield return new WaitForSeconds(time);

        // When the Input is not saved set open to List indexes 
        // else to List[0]
        if (!rewind.Save)
        {
            open = rewind.active[index];
            index++;
        }
        else
        {
            rewind.active.RemoveAt(0);
            open = rewind.active[0];

        }
        StartCoroutine(Replay(time, OpenList));

    }

    public void CheckBool()
    {
        // Check and Save bool for Rewindscript
        rewind.SaveBinary(open);
    }

}
