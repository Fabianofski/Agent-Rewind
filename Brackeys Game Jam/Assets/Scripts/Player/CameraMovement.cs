using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector2 minXY;
    public Vector2 maxXY;

    public Transform Target;


    void Update()
    {
        transform.position = new Vector3(Target.position.x, Target.position.y, transform.position.z);

        // Y Axis
        if(transform.position.y > maxXY.y)
        {
            transform.position = new Vector3(transform.position.x, maxXY.y, transform.position.z);
        }
        else if (transform.position.y < minXY.y)
        {
            transform.position = new Vector3(transform.position.x, minXY.y, transform.position.z);
        }

        // X Axis
        if (transform.position.x > maxXY.x)
        {
            transform.position = new Vector3(maxXY.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < minXY.x)
        {
            transform.position = new Vector3(minXY.x, transform.position.y, transform.position.z);
        }
    }
}
