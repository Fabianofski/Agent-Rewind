using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public bool open;

    private SpriteRenderer sr;
    private BoxCollider2D bc2d;

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
        // When Door is opened disable Sprite and Collider
        sr.enabled = false;
        bc2d.enabled = false;
    }

    public void CloseDoor()
    {
        // When Door is closed enable Sprite and Collider
        sr.enabled = true;
        bc2d.enabled = true;
    }

    public void StartRewind(List<bool> OpenList)
    {
        if (!Rewinding)
        {
            StartCoroutine(Replay(rewind.SaveOffset/2, OpenList));
        }

        Rewinding = true;
    }

    public void StopRewind()
    {
        Rewinding = false;
        StopAllCoroutines();
    }

    IEnumerator Replay(float time, List<bool> OpenList)
    {
        foreach (bool check in OpenList)
        {
            yield return new WaitForSeconds(time);
            open = check;    
        }

    }

    public void CheckBool()
    {
        rewind.SaveBinary(open);
    }

}
