using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public InputMaster controls;
    public Rigidbody2D rb;
    public Transform punchPoint;
    public GameObject PunchRadiusPF;

    public Vector2 direction;

    public float speed = 5f;
    public float sprintspeed = 8f;
    public float walkspeed = 5f;
    public float crouch = 2f;
    
    public bool isCrouching;
    //private float timebetweenPunch;
    //public float startTimebetweenPunch;


    [Header("Rewind")]

    public float maxRewindTime;
    public float RewindLeft;
    public float RewindCooldown;
    public float SaveOffset;
    public Slider Rewind;
    public GameObject RewindGUI;

    private Rewind[] RewindScripts;
    public bool RewindStarted;


    private void Awake()
    {
        controls = new InputMaster();

        // Punch
        controls.Player.Punch.performed += _ => Punch();

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

    }
    void Update()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        rb.velocity = direction * speed;

        if (direction == Vector2.zero)
            isCrouching = true;

            Rewind.value = RewindLeft;
        if (!RewindStarted)
        {
            if (RewindLeft < maxRewindTime)
                RewindLeft += Time.deltaTime;
            else
                RewindLeft = maxRewindTime;
            RewindCooldown -= Time.deltaTime;
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
        isCrouching = false;
        speed = walkspeed; 
    }
    
    void Crouch()
    {
        isCrouching = true;
        speed = crouch;
    }

    void Punch()
    {
        Instantiate(PunchRadiusPF, punchPoint.position, punchPoint.rotation);
        Debug.Log("Punched");
    }

    void StartRewind()
    {
        if (RewindLeft > 0 && RewindCooldown < 0)
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
        RewindCooldown = maxRewindTime - RewindLeft;

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
}
