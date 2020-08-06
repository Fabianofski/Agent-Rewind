using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterColorCode : MonoBehaviour
{

    public bool Colliding;
    public Color EnterColor;
    public PlayerMovementController controls;
    private ColorScript colorscript;

    public ColorScript.ColorType colortype;

    void Awake()
    {
        // Get Components
        controls = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();

        colorscript = GetComponentInParent<ColorScript>();
    }

    void Update()
    {
        // If the Player stands on the Button and E is pressed enter the Code

        if (Colliding && controls.EPressed)
        {
            colorscript.EnterCode(colortype);
            controls.EPressed = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check for Collision
        if(collider.gameObject.tag == "Player")
        {
            Colliding = true;
        }
    }

    void OnTriggerExit2D()
    {
        // Set Colliding to false
       Colliding = false;
    }

}
