using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivator : MonoBehaviour
{

    ActivatorHandler activatorhandler;

    void Start()
    {
        activatorhandler = GetComponentInParent<ActivatorHandler>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {

        activatorhandler.Activate(this, int.Parse(gameObject.name));

    }

    void OnTriggerExit2D()
    {
        activatorhandler.Deactivate(this, int.Parse(gameObject.name));
    }
}
