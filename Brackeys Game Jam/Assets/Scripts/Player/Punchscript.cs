using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punchscript : MonoBehaviour
{
    public InputMaster controls;
    public Transform punchPoint;
    public GameObject PunchRadiusPF;

    private bool PunchActive = false;

    private float timebetweenPunch;
    public float startTimebetweenPunch;

    public void Awake()
    {
        controls = new InputMaster();
        controls.Player.Punch.performed += _ => PunchActive = true;
        controls.Player.Punch.canceled += _ => PunchActive = false;
    }
    void Update()
    {
        if (timebetweenPunch <= 0)
        {
            if (PunchActive == true)
            {
                Punch();
                timebetweenPunch = startTimebetweenPunch;
            }
        }
        else
        {
            timebetweenPunch -= Time.deltaTime;
        }
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
}
