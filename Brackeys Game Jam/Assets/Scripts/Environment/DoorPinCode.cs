using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class DoorPinCode : MonoBehaviour
{

    public TMP_InputField PinInput;
    public TextMeshProUGUI text;
    public string Pin;
    public InputMaster1 controls;
    private bool Colliding;
    private PlayerMovementController pm;

    void Awake()
    {
        controls = new InputMaster1();

        controls.Player.Interacting.performed += _ => EnterPin();
        controls.Player.Restart.performed += _ => ExitPin();

        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    void EnterPin()
    {
        if (Colliding && !GetComponentInParent<Door>().open)
        {
            PinInput.gameObject.SetActive(true);
            pm.EPressed = false;
        }
        if (PinInput.text == Pin)
        {
            PinInput.text = "";
            GetComponentInParent<Door>().open = true;
            PinInput.gameObject.SetActive(false);
        }
    }

    void ExitPin()
    {
        PinInput.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Colliding = true;
            text.enabled = true;
        }
    }

    void OnTriggerExit2D()
    {
        Colliding = false;
        text.enabled = false;
        pm.EPressed = false;

        PinInput.gameObject.SetActive(false);
    }

    // for new Input System
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    public void PlaySound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactables/code_entering", transform.position);

    }
}
