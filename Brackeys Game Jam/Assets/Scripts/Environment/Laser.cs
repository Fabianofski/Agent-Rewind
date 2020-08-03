using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    private LineRenderer linerenderer;
    private EdgeCollider2D edgeCollider;

    [Header("Laser Parameters")]

    public Vector2 direction;
    public float maxDistance;
    public List<Vector2> hits;

    void Start()
    {
        // Get LineRenderer and MeshCollider
        linerenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        hits.Add(Vector2.zero);
    }

    void Update()
    {
        // Reset LineRenderer shoot New Raycast from Laser
        linerenderer.positionCount = 1;
        edgeCollider.points = new Vector2[0];
        edgeCollider.points = hits.ToArray();

        hits = new List<Vector2>(0);
        hits.Add(Vector2.zero);

        newRayCast( transform.position, direction);

    }

    void newRayCast(Vector2 origin, Vector2 dir)
    {
        // Check for hit with Raycast from origin to direction
        RaycastHit2D hit;
        hit = Physics2D.Raycast(origin, dir, maxDistance);

        if (hit)
        {
            // Get Last Pos of LineRenderer
            Vector2 lastpos = transform.TransformPoint(linerenderer.GetPosition(linerenderer.positionCount - 1));

            // Dont Draw new Line if the last pos is the same as the Hit.point
            if (hit.point != lastpos)
            {
                // Draw new Line
                linerenderer.positionCount = linerenderer.positionCount + 1;
                hits.Add(transform.InverseTransformPoint(hit.point));
                linerenderer.SetPosition(linerenderer.positionCount - 1, transform.InverseTransformPoint(hit.point));
              
                // Reflect when Collider is something Reflecting 
                if (hit.collider.gameObject.layer == 9)
                {
                    // Calculate new Direction
                    Vector2 direct =  Vector2.Reflect(dir, hit.normal);

                    // Shoot new Raycast
                    // Add Offset to origin because it otherwise collides with the same wall
                    newRayCast(hit.point + new Vector2(0.001f, 0.001f) * direct, direct);
                }
            }
        }
        else
        {
            // Draw Line to MaxDistance when there is no Hit
            linerenderer.positionCount = linerenderer.positionCount + 1;
            linerenderer.SetPosition(linerenderer.positionCount - 1, transform.InverseTransformPoint(origin + maxDistance * dir));

        }
    }


}
