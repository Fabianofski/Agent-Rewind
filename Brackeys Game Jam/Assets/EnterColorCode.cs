using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterColorCode : MonoBehaviour
{

    public bool Colliding;
    public Color EnterColor;
    public InputMaster1 controls;
    private ColorScript colorscript;

    void Awake()
    {
        controls = new InputMaster1();

        controls.Player.Interacting.performed += _ => EnterCode();
        colorscript = GetComponentInParent<ColorScript>();
    }

    void EnterCode()
    {
        Debug.Log("ja");
        if (Colliding)
        {
            colorscript.EnterCode(EnterColor);
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
