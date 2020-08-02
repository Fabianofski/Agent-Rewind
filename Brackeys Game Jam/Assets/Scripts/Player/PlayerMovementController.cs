using System.Collections;
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
    public Transform punchPoint;
    public GameObject PunchRadiusPF;

    public Vector2 direction;

    public float speed = 5f;
    //private float timebetweenPunch;
    //public float startTimebetweenPunch;
    float horizontalmovement;
    float verticalmovement;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Punch.performed += _ => Punch();

    }
    void Update()
    {

        Vector2 direction = controls.Player.Movment.ReadValue<Vector2>();
        rb.velocity = direction * speed;

    }

    void Punch()
    {
        Instantiate(PunchRadiusPF, punchPoint.position, punchPoint.rotation);
        Debug.Log("Punched");
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
