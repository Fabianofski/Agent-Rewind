using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int currentPathTarget = 1;


    [Header("States")]
    public bool waiting;
    public bool chasing;
    public bool rewind;

    [Header("Rewind")]
    public int RewindTarget;
    List<Vector3> lastPoses;



    void Update()
    {
        if(!chasing && !rewind)
        {
            MoveAlongPath(currentPathTarget);
        }
        else if(!chasing && rewind)
        {
            MoveAlongPath(currentPathTarget);

        }
        else if(chasing && !rewind)
        {
            Chase(lastPoses);
        }
        else if (chasing && rewind)
        {
            Chase(lastPoses);
        }

    }

    void MoveAlongPath(int Target)
    {
        // Move Enemy to new Position when not Waiting
        if (!waiting)
            transform.position = Vector3.MoveTowards(transform.position, path[Target].Points.position, speed * Time.deltaTime);

        // When the Enemies Position matches the Target Position and a Waittime is specified the Enemy will start waiting
        if (path[Target].Points.position == transform.position && !waiting)
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
                if (!rewind)
                    IncreasePathTarget();
                else
                    DecreasePathTarget();
            }
        }
    }

    void Chase(List<Vector3> positions)
    {
        if (rewind)
        {
            transform.position = Vector3.MoveTowards(transform.position, positions[RewindTarget], chasespeed * Time.deltaTime);

            if (positions[RewindTarget] == transform.position)
            {
                if (RewindTarget < positions.Count)
                    RewindTarget++;
            }
        }
        else if (chasing)
        {
            GameObject player = GameObject.FindWithTag("Player");
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chasespeed * Time.deltaTime);
        }
    }

    private IEnumerator Wait(float waitTime)
    {
        // Enable Movement after waitTime
        yield return new WaitForSeconds(waitTime);
        waiting = false;
        if (!rewind)
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

    public void RewindEnemy(List<Vector3> pos)
    {
        rewind = true;
        lastPoses = pos;
        RewindTarget = 0;

        if(!waiting)
          DecreasePathTarget();
    }

    public void StopRewindEnemy()
    {
        rewind = false;

        if(!waiting)
          IncreasePathTarget();
    }

    void OnDrawGizmosSelected()
    {
        // Visualize Path in Scene View with Gizmos

        for(int i = 1; i < path.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(path[i].Points.position, path[i - 1].Points.position);
        }
        Gizmos.DrawLine(path[path.Length - 1].Points.position, path[0].Points.position);
    }
}
