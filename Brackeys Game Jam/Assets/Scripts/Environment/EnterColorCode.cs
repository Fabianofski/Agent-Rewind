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
        controls = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();

        
        colorscript = GetComponentInParent<ColorScript>();
    }

    void Update()
    {
        if (Colliding && controls.EPressed)
        {
            colorscript.EnterCode(colortype);
            controls.EPressed = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            Colliding = true;
        }
    }

    void OnTriggerExit2D()
    {
       Colliding = false;
    }

}
