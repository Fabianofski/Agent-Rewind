﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public InputMaster controls;
    public Rigidbody2D rb;

    public Vector2 direction;

    public float speed = 5f;
    public float sprintspeed = 8f;
    public float walkspeed = 5f;
    public float crouch = 2f;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Sprint.performed += _ => sprintstart();
        controls.Player.Sprint.canceled += _ => sprintend();
        controls.Player.Crouch.performed += _ => Crouchstart();
        controls.Player.Crouch.canceled += _ => sprintend();
    }
    void Update()
    {

        Vector2 direction = controls.Player.Movment.ReadValue<Vector2>();
        rb.velocity = direction * speed;

    }

    void sprintstart()
    {
        speed = sprintspeed;
    }

    void sprintend()
    {
        speed = walkspeed; 
    }
    
    void Crouchstart()
    {
        speed = crouch;
    }


    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
