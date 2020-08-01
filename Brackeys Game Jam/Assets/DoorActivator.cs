using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivator : MonoBehaviour
{
    public LayerMask TriggerLayer;

    private Door door;

    void Start()
    {
        door = GetComponentInParent<Door>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.layer == TriggerLayer)
        {

            door.open = true;

        }
        else
        {
            door.open = false;
        }

    }

    void OnTriggerExit2D()
    {
        door.open = false;
    }
}
