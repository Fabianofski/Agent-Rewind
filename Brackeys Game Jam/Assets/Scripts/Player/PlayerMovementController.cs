﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public float LaserForce = 50;


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
    private bool Colliding;

    [Header("Animations")]

    private Animator animator;
    public GameObject spriteGO;
    private SpriteRenderer spriteRenderer;
    public Sprite waitSprite;

    private void Awake()
    {
        // Get Components
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

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
        // Player Movement
        direction = controls.Player.Movement.ReadValue<Vector2>();
        if(!Colliding)
           rb.velocity = direction * speed;

        // Check if Player is Crouching
        if (CrouchStarted)
            isCrouching = true;
        else
            isCrouching = false;

        // Set Slider to RewindLeft
            Rewind.value = RewindLeft;

        
        if (!RewindStarted)
        {
            // Reload Rewind
            if (RewindLeft < maxRewindTime)
                RewindLeft += Time.deltaTime;
            else
                RewindLeft = maxRewindTime;

            // Decrease RewindCooldown
            RewindCooldownCountdown -= Time.deltaTime;
        }
        else
        {
            // Decrease RewindLeft
            RewindLeft -= Time.deltaTime;
            if(RewindLeft < 0)
            {
                // Stop Rewind when there is no Rewind Left
                RewindLeft = 0;
                StopRewind();
            }

        }

        if(direction != Vector2.zero)
        {
            if (isCrouching)
            {
                //
                // Play Crouch Sound
                //
            }
            else
            {
                //
                // Play Footstep Sound
                //
            }
        }

        if (RewindStarted)
        {
            //
            // Play Rewind Sound
            //
        }

        if(RewindLeft <= 0)
        {
            //
            // Play Rewind Empty Sound
            //
        }
        else if (RewindLeft == maxRewindTime)
        {
            //
            // Play Rewind Refilled Sound
            //
        }

        Animations();

    }

    void Sprint()
    {
        // set Players speed to Sprint speed
        speed = sprintspeed;

        animator.speed = sprintspeed / speed;
    }

    void Walk()
    {
        // set Players speed to Walk speed
        CrouchStarted = false;
        speed = walkspeed;

        animator.speed = 1;
    }
    
    void Crouch()
    {
        // set Players speed to Crouch speed
        CrouchStarted = true;
        speed = crouch;

        animator.speed = crouch / speed;
    }


    void StartRewind()
    {
        // Start the Rewind

        if (RewindLeft > 0 && RewindCooldownCountdown < 0)
        {
            // Activate GUI 
            RewindStarted = true;
            RewindGUI.SetActive(true);

            // Rewind Time in every Rewind Script in the Scene
            foreach (Rewind rewind in RewindScripts)
            {
                rewind.RewindTime();
            }

        }
        else
        {
            // Stop the Rewind
            StopRewind();
        }
    }

    void StopRewind()
    {
        // Stop Rewind
        RewindStarted = false;

        // Deactivate Rewind GUI, reset Rewind Cooldown
        RewindGUI.SetActive(false);
        RewindCooldownCountdown = RewindCooldown;

        foreach (Rewind rewind in RewindScripts)
        {
            rewind.StopRewindTime();
        }
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for Collisions
        if (collision.gameObject.tag == "Laser")
        {
            Colliding = true;

            Vector2 pos = transform.position;
            Vector2 hit = collision.GetContact(0).point;
            Vector2 dir = pos - hit;
            dir = dir.normalized;
            rb.AddForce(dir * LaserForce);

            //
            // Play Laser Damage Sound
            //
        }

        if (collision.gameObject.tag == "Enemy")
        {
            //
            // Play Death Sound
            //
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(collision.gameObject.tag == "Box")
        {
            //
            // Play Push Box Sound
            //
        }

    }

    void OnCollisionExit2D()
    {
        StartCoroutine(col());
    }

    IEnumerator col()
    {
        yield return new WaitForSeconds(0.1f);
        Colliding = false;
    }

    void Animations()
    {
        
        if (direction == Vector2.zero)
        {
            spriteRenderer.sprite = waitSprite;
            animator.enabled = false;
        }
        else
        {
            Vector2 pos = transform.position;
            spriteGO.transform.up = -direction;
            animator.enabled = true;
        }
    }
}
