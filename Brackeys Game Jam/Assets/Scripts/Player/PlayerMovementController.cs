using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class PlayerMovementController : MonoBehaviour
{
    public InputMaster1 controls;
    public Rigidbody2D rb;

    public Vector2 direction;

    public float speed = 5f;
    public float sprintspeed = 8f;
    public float walkspeed = 5f;
    public float crouch = 2f;
    
    public bool isCrouching;
    private bool CrouchStarted;
    //private float timebetweenPunch;
    //public float startTimebetweenPunch;


    [Header("Rewind")]

    public float maxRewindTime;
    public float RewindLeft;
    public float RewindCooldown;
    private float RewindCooldownCountdown;
    public float SaveOffset;
    public Slider Rewind;
    public GameObject RewindGUI;

    private Rewind[] RewindScripts;
    public bool RewindStarted;

    [HideInInspector]
    public bool EPressed;

    private void Awake()
    {
        controls = new InputMaster1();

        // Sprint
        controls.Player.Sprint.performed += _ => Sprint();
        controls.Player.Sprint.canceled += _ => Walk();

        // Crouch
        controls.Player.Crouch.performed += _ => Crouch();
        controls.Player.Crouch.canceled += _ => Walk();

        // Rewind
        RewindScripts = FindObjectsOfType<Rewind>();
        Rewind.maxValue = maxRewindTime;
        controls.Player.Rewind.performed += _ => StartRewind();
        controls.Player.Rewind.canceled += _ => StopRewind();

        controls.Player.Interacting.performed += _ => EnterCode();
    }

    private void EnterCode()
    {
        EPressed = true;
    }

    void Update()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        rb.velocity = direction * speed;

        if (direction == Vector2.zero)
            isCrouching = true;
        else if (CrouchStarted)
            isCrouching = true;
        else
            isCrouching = false;

            Rewind.value = RewindLeft;
        if (!RewindStarted)
        {
            if (RewindLeft < maxRewindTime)
                RewindLeft += Time.deltaTime;
            else
                RewindLeft = maxRewindTime;
            RewindCooldownCountdown -= Time.deltaTime;
        }
        else
        {
            RewindLeft -= Time.deltaTime;
            if(RewindLeft < 0)
            {
                RewindLeft = 0;
                StopRewind();
            }

        }



    }

    void Sprint()
    {
        speed = sprintspeed;
    }

    void Walk()
    {
        CrouchStarted = false;
        speed = walkspeed; 
    }
    
    void Crouch()
    {
        CrouchStarted = true;
        speed = crouch;
    }


    void StartRewind()
    {
        if (RewindLeft > 0 && RewindCooldownCountdown < 0)
        {
            RewindStarted = true;
            RewindGUI.SetActive(true);

            foreach (Rewind rewind in RewindScripts)
            {
                rewind.RewindTime();
            }

        }
        else
        {
            StopRewind();
        }
    }

    void StopRewind()
    {
        RewindStarted = false;

        RewindGUI.SetActive(false);
        RewindCooldownCountdown = RewindCooldown;

        foreach (Rewind rewind in RewindScripts)
        {
            rewind.StopRewindTime();
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            Debug.Log("Get Lasered");
        }
    }
}
