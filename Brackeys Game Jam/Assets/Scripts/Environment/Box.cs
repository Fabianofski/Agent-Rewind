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
            // Lerp to old Position
            transform.position = Vector3.Lerp(transform.position, lastposes[index], rewind.SaveOffset / 2);

            // If the Box is at the Position get to next Position
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
        // Start Rewind set index to 0 and get the Input
        if (!Rewinding)
        {
            index = 0;
            lastposes = poses;
        }
        Rewinding = true;
    }

    void StopRewind()
    {
        // Stop the Rewind
        Rewinding = false;
        rewind.position.RemoveAt(0);
    }

    IEnumerator Increase(float time)
    {
        // Increase the index after time seconds
        CoroutineIsRunning = true;
        yield return new WaitForSeconds(time);
        rewind.position.RemoveAt(0);
        index++;

        CoroutineIsRunning = false;
    }

    // Takes in 3 Parameters a, b and difference
    // When the difference between a and b is smaller than difference
    // return true else false
    bool Approximately(float a, float b, float difference)
    {
        if (Mathf.Abs(a - b) < difference)
            return true;
        else
            return false;
    }
}
