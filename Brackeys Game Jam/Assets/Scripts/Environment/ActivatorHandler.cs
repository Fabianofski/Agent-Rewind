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
        // Get Door Script in Parent
        door = GetComponentInParent<Door>();

        // Initialize bool Array 
        TriggersPressed = new bool[activators.Length];
    }

    void Update()
    {
        if (!door.Rewinding)
        {
            // Check if every Activator is set up right
            AllTriggersSetUp = true;

            for (int i = 0; i < activators.Length; i++)
            {
                // Loop through every Activator
                // if the TriggerPressed Array is not the same as the created set up the 
                // Door will close and the AllTriggersSetUp set to false
                if (TriggersPressed[i] != activators[i].press)
                {
                    CloseDoor();
                    AllTriggersSetUp = false;
                }

            }

            // Open Door if every Activator is set up right
            if (AllTriggersSetUp)
                OpenDoor();
        }
    }

    public void Activate(int index)
    {
        // Script is called by Activators
        // Gets index from Script and sets it to true
        // The script now knows that the Activator IS triggered 
        TriggersPressed[index] = true;
    }

    public void Deactivate(int index)
    {
        // Script is called by Activators
        // Gets index from Script and sets it to true
        // The script now knows that the Activator IS NOT triggered 

        TriggersPressed[index] = false;
    }

    public void OpenDoor()
    {
        // Set open Bool in Door Script to true to open Door
        door.open = true;
    }

    public void CloseDoor()
    {
        // Set open Bool in Door Script to false to close Door
        door.open = false;
    }
}
