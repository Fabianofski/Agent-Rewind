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

        index = 0;
        Rewinding = true;
    }

    public void StopRewind()
    {
        Rewinding = false;
        StopAllCoroutines();
    }

    IEnumerator Replay(float time, List<bool> OpenList)
    {
        yield return new WaitForSeconds(time);

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
        rewind.SaveBinary(open);
    }

}
