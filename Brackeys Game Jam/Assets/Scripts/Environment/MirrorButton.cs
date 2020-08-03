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
    private int index;
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
                Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z + 45);
            else
                Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z - 45);

        }

    }

    void OnTriggerExit2D()
    {
        Entered = false;
        Rotate = false;
    }

    void Update()
    {
        Mirror.transform.rotation = Quaternion.Lerp(Mirror.transform.rotation, Rotation, Speed * Time.deltaTime);
    }

    void StartRewind(List<bool> rotated)
    {
        if (!rewinding)
        {
            StartCoroutine(Replay(rotated, rewind.SaveOffset));
            index = 0;
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

        if (rotated[index])
        {
            if (ClockWise)
                Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z - 45);
            else
                Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z + 45);

        }

        index++;

        StartCoroutine(Replay(rotated, rewind.SaveOffset));
    }

}