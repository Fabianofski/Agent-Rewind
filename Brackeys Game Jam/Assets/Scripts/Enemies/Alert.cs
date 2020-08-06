using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Alert : MonoBehaviour
{

    private EnemyMovement em;
    private GameObject Player;
    private Vector2 direction;

    public float AlertingDistance;
    public float ChasingDistance;
    public float distance;
    public LayerMask blockSight;

    public bool PlayerCrouching;
    public bool InCone;

    private RaycastHit2D hit;

    public Light2D Cone;
    public Color alertColor;
    public Color normalColor;

    private bool CameraMovement;

    void Awake()
    {
        // Get Components
        Player = GameObject.FindWithTag("Player");
        em = GetComponent<EnemyMovement>();
    }

    void Update()
    {
        // Get Playercrouching State
        PlayerCrouching = Player.GetComponent<PlayerMovementController>().isCrouching;

        // Calculate Distance and Direction to Player
        distance = Vector2.Distance(Player.transform.position, transform.position);
        direction = new Vector2(Player.transform.position.x - transform.position.x, Player.transform.position.y- transform.position.y).normalized;

        // Shoot Raycast to Players Direction
        hit = Physics2D.Raycast(transform.position, direction, distance, blockSight);

        if (hit)
        {
            // Only Chase Player when player got hit by Raycast and the Enemy is close enough
            if (distance < AlertingDistance && hit.collider.gameObject.tag == "Player" && !PlayerCrouching)
            {
                em.chasing = true;
                NoCameraAlert();
                em.CheckCamera = false;

                //
                // Play Alert Sound
                //

            }
            else if (InCone && hit.collider.gameObject.tag == "Player")
            {
                em.chasing = true;
                NoCameraAlert();
                em.CheckCamera = false;

                //
                // Play Alert Sound
                //
            }
            else if (CameraMovement)
            {
                em.chasing = true;
                em.CheckCamera = true;

                //
                // Play Alert Sound
                //
            }
            else if (em.chasing && ChasingDistance < distance && !em.rewinding)
            {
                em.chasing = false;

                //
                // Play Enemy Losing Sound
                //
            }
            else if (em.rewinding && distance < AlertingDistance)
            {
                em.chasing = false;

                //
                // Play Enemy Losing Sound
                //
            }
        }

        if (em.chasing)
        {
            Cone.color = alertColor;
        }
        else
        {
            Cone.color = normalColor;
        }

    }

    void OnDrawGizmos()
    {
        // Draw Gizmos in Sceneview 
        // Red: Alerting Distance in Direction
        // Green: Distance to hit in Direction

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction * AlertingDistance);
        Gizmos.DrawWireSphere(transform.position, AlertingDistance);

        if (hit.distance < AlertingDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, direction * hit.distance);
        }
    }

    public void CameraAlert(Transform target, float maxDistance)
    {
        if (!em.chasing && Vector3.Distance(target.position, transform.position) < maxDistance)
        {
            CameraMovement = true;
            em.destination.target = target;

        }
    }

    public void NoCameraAlert()
    {
        CameraMovement = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            InCone = true;
    }

    void OnTriggerExit2D()
    {
        InCone = false;
    }

}
