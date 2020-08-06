using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{

    [System.Serializable]
    public class Path
    {
        public Transform Points;
        public float waitTime;
    }

    [Header("Pathfinding")]

    public Path[] path;
    public float speed;
    public float chasespeed;
    public float rotspeed;
    public int currentPathTarget = 1;
    public Transform t;
    public AIDestinationSetter destination;
    public AIPath ai;

    [Header("States")]
    public bool waiting;
    public bool chasing;
    public bool rewinding;
    public bool CheckCamera;

    [Header("Rewind")]
    public int RewindTarget;
    public Vector2 direction;
    List<Vector3> lastPoses;
    private Rewind rewind;


    void Awake()
    {
        destination = GetComponent<AIDestinationSetter>();
        ai = GetComponent<AIPath>();
        rewind = GetComponent<Rewind>();
    }

    void Update()
    {
        if(!chasing)
        {
            // When patroling set the Speed to times 2
            // Because Rewind is 2x << 
            if (rewinding)
                ai.maxSpeed = speed * 2;
            else
                ai.maxSpeed = speed;
            MoveAlongPath(currentPathTarget);
        }
        else if(chasing)
        {
            // When chasing set the Speed to times 2
            // Because Rewind is 2x << 
            if (rewinding)
                ai.maxSpeed = chasespeed * 2;
            else
                ai.maxSpeed = chasespeed;
            Chase(lastPoses);
        }

        if (rewinding)
        {
            // Get the direction from Pathfinderscript
            direction = -ai.desiredVelocity;
        }
        else
        {
            // Get the direction from Pathfinderscript
            direction = ai.desiredVelocity;
        }

        // Set the Rotation to the direction 
        if(!waiting || chasing)
           transform.up = Vector2.Lerp(transform.up , direction, rotspeed * Time.deltaTime);
    }

    void MoveAlongPath(int Target)
    {
        // Move Enemy to new Position when not Waiting
        if (!waiting)
            destination.target = path[Target].Points;

        // When the Enemies Position matches the Target Position and a Waittime is specified the Enemy will start waiting
        if (Approximately(path[Target].Points.position, transform.position, ai.endReachedDistance * 2) && !waiting)
        {
            if (path[Target].waitTime != 0)
            {
                // Disable Movement for the Time specified
                waiting = true;
                StartCoroutine(Wait(path[Target].waitTime));
            }
            else
            {
                // When the Enemy has no Waittime specified he will just move on
                waiting = false;
                if (!rewinding)
                    IncreasePathTarget();
                else
                    DecreasePathTarget();
            }
        }
    }

    void Chase(List<Vector3> positions)
    {
        if (rewinding)
        {
            // Set Transform t to Target
            t.position = positions[RewindTarget];
            destination.target = t;
            
            // if Enemy reached destination remove used Inputs from List and increase RewindTarget
            if (Approximately(positions[RewindTarget], transform.position, ai.endReachedDistance / 2))
            {
                if (RewindTarget < positions.Count)
                    RewindTarget++;
                rewind.position.RemoveAt(0);
            }
        }
        else if (chasing)
        {
            // When chasing select the Player as Target
            if (!CheckCamera)
            {
                GameObject player = GameObject.FindWithTag("Player");
                destination.target = player.transform;
            }
            else if(Approximately(transform.position, destination.target.position, ai.endReachedDistance))
            {
                CheckCamera = false;
                GetComponent<Alert>().NoCameraAlert();
            }
        }
    }

    private IEnumerator Wait(float waitTime)
    {
        // Enable Movement after waitTime
        yield return new WaitForSeconds(waitTime);
        waiting = false;
        if (!rewinding)
            IncreasePathTarget();
        else
            DecreasePathTarget();
    }

    void IncreasePathTarget()
    {
        // Increase the Target to the next Path
        if (currentPathTarget == path.Length - 1)
        {
            currentPathTarget = 0;
        }
        else
        {
            currentPathTarget++;
        }
    }

    void DecreasePathTarget()
    {
        // Increase the Target to the next Path
        if (currentPathTarget == 0)
        {
            currentPathTarget = path.Length - 1;
        }
        else
        {
            currentPathTarget--;
        }
    }

    public void StartRewind(List<Vector3> pos)
    {
        // Start Rewind
        if(!rewinding)
           lastPoses = pos;
        rewinding = true;

        // Set RewindTarget to 0
        RewindTarget = 0;

        if(!waiting)
          DecreasePathTarget();
    }

    public void StopRewind()
    {
        // Stop Rewind
        rewinding = false;

        if(!waiting)
          IncreasePathTarget();
    }

    void OnDrawGizmos()
    {
        // Visualize Path in Scene View with Gizmos

        for(int i = 1; i < path.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(path[i].Points.position, path[i - 1].Points.position);
        }
        Gizmos.DrawLine(path[path.Length - 1].Points.position, path[0].Points.position);
    }

    // Takes in 3 Parameters a, b and difference
    // When the difference between a and b is smaller than difference
    // return true else false
    public bool Approximately(Vector3 me, Vector3 other, float allowedDifference)
    {
        var dx = me.x - other.x;
        if (Mathf.Abs(dx) > allowedDifference)
            return false;

        var dy = me.y - other.y;
        if (Mathf.Abs(dy) > allowedDifference)
            return false;

        var dz = me.z - other.z;

        if (Mathf.Abs(dz) > allowedDifference)
            return false;

        return true;
    }
}
