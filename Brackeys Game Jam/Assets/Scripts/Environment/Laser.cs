using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public Vector2 direction;
    public LineRenderer linerenderer;

    public LayerMask Reflecting;

    Vector3 hitpoint;
    Vector3 normal;

    void Update()
    {
        linerenderer.positionCount = 1;
        newRayCast(direction, transform.position);
    }

    void newRayCast(Vector2 dir, Vector2 origin)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(origin, dir, Mathf.Infinity);

        if (hit)
        {
            Vector2 lastpos = transform.TransformPoint(linerenderer.GetPosition(linerenderer.positionCount - 1));

            if (hit.point != lastpos)
            {
                linerenderer.positionCount = linerenderer.positionCount + 1;
                linerenderer.SetPosition(linerenderer.positionCount - 1, transform.InverseTransformPoint(hit.point));

                if (hit.collider.gameObject.layer == 9)
                {
                    Vector2 direct = hit.collider.transform.rotation * transform.up;

                    hitpoint =hit.point;
                    normal = direct;

                    newRayCast(direct, hit.point);
                }
            }
        }
    }

    void OnDrawGizmos()
    {

        Gizmos.DrawLine(hitpoint, hitpoint + normal * 1);
    }

}
