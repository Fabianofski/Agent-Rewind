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
    public int currentPathTarget = 1; 
    public AIDestinationSetter destination;
    public AIPath ai;

    [Header("States")]
    public bool waiting;
    public bool chasing;
    public bool rewind;

    [Header("Rewind")]
    public int RewindTarget;
    List<Vector3> lastPoses;


    void Start()
    {
        destination = GetComponent<AIDestinationSetter>();
        ai = GetComponent<AIPath>();
    }

    void Update()
    {
        if(!chasing)
        {
            ai.maxSpeed = speed;
            MoveAlongPath(currentPathTarget);
        }
        else if(chasing)
        {
            ai.maxSpeed = chasespeed;
            Chase(lastPoses);
        }

    }

    void MoveAlongPath(int Target)
    {
        // Move Enemy to new Position when not Waiting
        if (!waiting)
            destination.target = path[Target].Points;

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
            List<Transform> transforms = new List<Transform>(positions.Count);

            for(int i = 0; i < transforms.Count; i++)
            {
                transforms[i].position = positions[i];
            }

            destination.target = transforms[RewindTarget];
            

            if (positions[RewindTarget] == transform.position)
            {
                if (RewindTarget < positions.Count)
                    RewindTarget++;
            }
        }
        else if (chasing)
        {
            GameObject player = GameObject.FindWithTag("Player");
            destination.target = player.transform;
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

    public void StartRewind(List<Vector3> pos)
    {
        rewind = true;
        lastPoses = pos;
        RewindTarget = 0;

        if(!waiting)
          DecreasePathTarget();
    }

    public void StopRewind()
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
