using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class DoorPinCode : MonoBehaviour
{

    public TMP_InputField PinInput;
    public string Pin;

    private bool EPressed;
    private bool Colliding;
    private PlayerMovementController pm;

    void Start()
    {

        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        EPressed = pm.EPressed;

        if (EPressed && Colliding && !GetComponentInParent<Door>().open)
        {
            PinInput.gameObject.SetActive(true);
            pm.EPressed = false;
        }

        if (InputSystem.GetDevice<Keyboard>().escapeKey.wasPressedThisFrame ||!Colliding)
        {
            PinInput.gameObject.SetActive(false);
        }

        if (PinInput.text == Pin && InputSystem.GetDevice<Keyboard>().enterKey.wasPressedThisFrame)
        {
            PinInput.text = "";
            GetComponentInParent<Door>().open = true;
            PinInput.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Colliding = true;
        }
    }

    void OnTriggerExit2D()
    {
        Colliding = false;
        pm.EPressed = false;
    }

}
