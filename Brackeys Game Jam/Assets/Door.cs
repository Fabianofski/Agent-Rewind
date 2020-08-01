using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public bool open;  

    private SpriteRenderer sr;
    private BoxCollider2D bc2d;
    private Rewind rewind;
    private bool CoroutineRunning;
    private List<bool> Container;

    void Awake()
    {
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
        sr.enabled = false;
        bc2d.enabled = false;
    }

    public void CloseDoor()
    {
        sr.enabled = true;
        bc2d.enabled = true;
    }

}
