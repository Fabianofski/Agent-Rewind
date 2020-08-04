using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{

    public Vector2 maxAngles;
    public float speed;
    public float waitTime;

    private bool ReachedMaxAngle = false;
    private bool Rewinding;

    void Update()
    {
        if(Approximately(transform.eulerAngles.z, maxAngles.y, 0.1f) && !ReachedMaxAngle)
        {
            if (Rewinding)
                StartCoroutine(Wait(waitTime / 2, true));
            else
                StartCoroutine(Wait(waitTime / 2, true));
        }
        else if (Approximately(transform.eulerAngles.z, maxAngles.x, 0.1f) && ReachedMaxAngle)
        {
            if(Rewinding)
               StartCoroutine(Wait(waitTime / 2, false));
            else
                StartCoroutine(Wait(waitTime / 2, false));
        }

        if (ReachedMaxAngle)
        {
            if(!Rewinding)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,maxAngles.x), speed * Time.deltaTime);
            else
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, maxAngles.x), 2 * speed * Time.deltaTime);

        }
        else if(!ReachedMaxAngle)
        {
            if (!Rewinding)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, maxAngles.y), speed * Time.deltaTime);
            else
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, maxAngles.y), 2 * speed * Time.deltaTime);
        }
    }

    void StartRewind()
    {
        if (!Rewinding)
        {
           StopAllCoroutines();
           StartCoroutine(Wait(waitTime/2, !ReachedMaxAngle));
        }

        Rewinding = true;
    }

    void StopRewind()
    {
        Rewinding = false;
    }


    IEnumerator Wait(float time, bool set)
    {
        yield return new WaitForSeconds(time);

        ReachedMaxAngle = set;
    }

    bool Approximately(float a, float b, float difference)
    {
        if (Mathf.Abs(a - b) < difference)
            return true;
        else
            return false;     
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            Debug.Log("Gotcha");
        }
    }

}
