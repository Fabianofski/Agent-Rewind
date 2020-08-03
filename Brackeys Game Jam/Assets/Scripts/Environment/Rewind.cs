using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewind : MonoBehaviour
{

    public enum Type { Movement, Binary, Rotation };

    public Type type;
    [HideInInspector]
    public float maxRewindTime;
    [HideInInspector]
    public float SaveOffset;

    private int SavedPoints;
    private bool Rewinding;

    // Enemy
    public List<Vector3> position;

    // Binary
    public List<bool> active;
    public bool StartValue;


    // Rotation
    public List<Quaternion> rotation;
    public GameObject target;

    private PlayerMovementController pm;

    public bool Save = true;

    void Start()
    {
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMovementController>();

        maxRewindTime = pm.maxRewindTime * 2;
        SaveOffset = pm.SaveOffset;

        // Calculate Number of Points that have to be saved
        SavedPoints = Mathf.CeilToInt(maxRewindTime / SaveOffset);

        if (Save)
        {
            for (int i = 0; i < SavedPoints; i++)
            {
                if (type == Type.Movement)
                    position.Insert(0, transform.position);
                else if (type == Type.Binary)
                    active.Insert(0, StartValue);
                else
                    rotation.Insert(0, target.transform.rotation);

            }


            // Start Loop that Saves the Types every Period of Time
            StartCoroutine(Loop());

        }
    }

    // Save every SaveOffset Seconds and call function based on chosen enum
    IEnumerator Loop()
    {
        yield return new WaitForSeconds(SaveOffset);

        switch (type)
        {
            case Type.Movement:
                if (Rewinding)
                    break;
                SaveMovement();
                break;

            case Type.Binary:
                if (Rewinding)
                    break;
                gameObject.SendMessage("CheckBool");
                break;

            case Type.Rotation:
                if (Rewinding)
                    break;
                SaveRotation();
                break;
        }

        StartCoroutine(Loop());
    }

    // Rewind Time of chosen enum
    public void RewindTime()
    {
        Rewinding = true;

        switch (type)
        {
            case Type.Movement:
                gameObject.SendMessage("StartRewind", position);
                break;
            case Type.Binary:
                gameObject.SendMessage("StartRewind", active);
                break;
            case Type.Rotation:
                gameObject.SendMessage("StartRewind", rotation);
                break;
        }

    }

    // Stop Time of chosen enum
    public void StopRewindTime()
    {
        Rewinding = false;

        gameObject.SendMessage("StopRewind");
    }

    // Save points in List of chosen Variable
    void SaveMovement()
    {
        position.Insert(0, transform.position);
        if (position.Count > SavedPoints)
            position.RemoveAt(SavedPoints);
    }

    public void SaveBinary(bool check)
    {
        active.Insert(0, check);
        if (active.Count > SavedPoints)
            active.RemoveAt(SavedPoints);
    }

    void SaveRotation()
    {
        rotation.Insert(0, target.transform.rotation);
        if (rotation.Count > SavedPoints)
            rotation.RemoveAt(SavedPoints);
    }

}