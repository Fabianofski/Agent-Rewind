using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorHandler : MonoBehaviour
{

    public enum Type { SingleActivator, MultipleActivator }
    public LayerMask TriggerLayer;
    public Type doorType;

    [System.Serializable]
    public class Activators
    {
        public DoorActivator dooractivator;
        public bool press;
    }

    public Activators[] activators;
    public bool[] TriggersPressed;
    private bool AllTriggersSetUp;

    private Door door;

    void Start()
    {
        door = GetComponentInParent<Door>();
        TriggersPressed = new bool[activators.Length];
    }

    void Update()
    {
        AllTriggersSetUp = true;
        for (int i = 0; i < activators.Length; i++)
        {
            if (TriggersPressed[i] != activators[i].press)
            {
                CloseDoor();
                AllTriggersSetUp = false;
            }

        }

        if(AllTriggersSetUp)
         OpenDoor();
    }

    public void Activate(DoorActivator dooractivator, int index)
    {
        TriggersPressed[index] = true;
    }

    public void Deactivate(DoorActivator dooractivator, int index)
    {
        TriggersPressed[index] = false;
    }

    public void OpenDoor()
    {
        door.open = true;
    }

    public void CloseDoor()
    {
        door.open = false;
    }
}
