using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private bool Rewinding;
    private int index;
    private Rewind rewind;
    private bool CoroutineIsRunning;
    List<Vector3> lastposes;


    void Awake()
    {
        rewind = GetComponent<Rewind>();
    }

    void Update()
    {
        if (Rewinding)
        {
            transform.position = Vector3.Lerp(transform.position, lastposes[index], rewind.SaveOffset / 2);

            if (Approximately(transform.position.x, lastposes[index].x, 0.1f) && Approximately(transform.position.y, lastposes[index].y, 0.1f))
            {
                if (Approximately(lastposes[index].x, lastposes[index + 1].x, 0.1f) && Approximately(lastposes[index].y, lastposes[index + 1].y, 0.1f))
                {
                    if (!CoroutineIsRunning)
                        StartCoroutine(Increase(rewind.SaveOffset / 2));
                }
                else
                    index++;
            }
        }

    }

    void StartRewind(List<Vector3> poses)
    {
        if(!Rewinding)
           index = 0;
        lastposes = poses;
        Rewinding = true;
    }

    void StopRewind()
    {
        Rewinding = false;
    }

    IEnumerator Increase(float time)
    {
        CoroutineIsRunning = true;
        yield return new WaitForSeconds(time);
        index++;

        CoroutineIsRunning = false;
    }

    bool Approximately(float a, float b, float difference)
    {
        if (Mathf.Abs(a - b) < difference)
            return true;
        else
            return false;
    }
}
