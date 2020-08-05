using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorButton : MonoBehaviour
{

    public GameObject Mirror;
    public float Speed;
    public bool ClockWise;

    public Quaternion Rotation;

    private Rewind rewind;
    private bool rewinding;
    private bool Rotate;
    private bool Entered;

    void Start()
    {
        Rotation = Mirror.transform.rotation;
        rewind = GetComponent<Rewind>();
    }

    void OnTriggerEnter2D()
    {
        if (!rewinding)
        {
            Rotate = false;

            if (!Entered)
            {
                Rotate = true;
                rewind.SaveBinary(Rotate);
            }
            Entered = true;

            if (ClockWise)
                Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z + 90);
            else
                Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z - 90);

        }

    }

    void OnTriggerExit2D()
    {
        Entered = false;
        Rotate = false;
    }

    void Update()
    {
        Mirror.transform.rotation = Rotation;
    }

    void StartRewind(List<bool> rotated)
    {
        if (!rewinding)
        {
            StartCoroutine(Replay(rotated, rewind.SaveOffset / 2));
        }
        rewinding = true;
    }

    void StopRewind()
    {
        rewinding = false;
        StopAllCoroutines();
    }

    void CheckBool()
    {
        if (!Rotate)
            rewind.SaveBinary(Rotate);
        else
        {
            rewind.SaveBinary(false);
        }

    }

    public IEnumerator Replay(List<bool> rotated, float time)
    {
        yield return new WaitForSeconds(time);

        if (rewind.active[0])
        {
            if (ClockWise)
                Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z - 90);
            else
                Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z + 90);

        }
        rewind.active.RemoveAt(0);

        StartCoroutine(Replay(rotated, time));
    }

}