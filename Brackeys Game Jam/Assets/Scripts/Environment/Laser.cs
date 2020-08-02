using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    private LineRenderer linerenderer;
    private MeshCollider meshCollider;

    [Header("Laser Parameters")]

    public Vector2 direction;
    public float maxDistance;

    void Start()
    {
        // Get LineRenderer and MeshCollider
        linerenderer = GetComponent<LineRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        // Reset LineRenderer shoot New Raycast from Laser
        linerenderer.positionCount = 1;
        newRayCast( transform.position, direction);

        // Get the Mesh of the LineRenderer so it can be applied to an MeshCollider for Collision
        Mesh mesh = new Mesh();
        linerenderer.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;

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
