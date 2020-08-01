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

    private RaycastHit2D hit;

    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        em = GetComponent<EnemyMovement>();
    }

    void Update()
    {

        distance = Vector2.Distance(Player.transform.position, transform.position);
        direction = new Vector2(Player.transform.position.x - transform.position.x, Player.transform.position.y- transform.position.y).normalized;

        hit = Physics2D.Raycast(transform.position, direction, distance, blockSight);

        if (hit)
        {
            if (distance < AlertingDistance && hit.collider.gameObject.tag == "Player")
            {
                em.chasing = true;
            }
            else
            {
                em.chasing = false;
            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction * AlertingDistance);

        if (hit.distance < AlertingDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, direction * hit.distance);
        }
    }

}
