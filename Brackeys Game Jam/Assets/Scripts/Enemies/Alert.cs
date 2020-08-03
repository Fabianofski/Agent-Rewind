using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (distance < AlertingDistance && hit.collider.gameObject.tag == "Player" && !PlayerCrouching && !em.chasing)
            {
                em.chasing = true;
            }
            else if (InCone && hit.collider.gameObject.tag == "Player" && !em.chasing)
            {
                em.chasing = true;
            }
            else if(em.chasing && ChasingDistance < distance && !em.rewind)
            {
                em.chasing = false;
            }
            else if(em.rewind && distance < AlertingDistance)
            {
                em.chasing = false;
            }
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
