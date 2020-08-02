using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivator : MonoBehaviour
{

    ActivatorHandler activatorhandler;

    void Start()
    {
        // Get ActivatorHandler in Parent
        activatorhandler = GetComponentInParent<ActivatorHandler>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // When DoorActivator is triggered notify the ActivatorHandler 
        // with their Index (Name)
        activatorhandler.Activate(int.Parse(gameObject.name));

    }

    void OnTriggerExit2D()
    {
        // When DoorActivator is triggered notify the ActivatorHandler 
        // with their Index (Name)

        activatorhandler.Deactivate(int.Parse(gameObject.name));
    }
}
