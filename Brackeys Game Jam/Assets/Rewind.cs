using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rewind : MonoBehaviour
{

    public enum Type {Enemy, Movement, Binary, Rotation};

    public Type type;
    public float maxRewindTime;
    public float SaveOffset;

    private int SavedPoints;
    private bool Rewinding;

    // Enemy
    public List<Vector3> position;

    // Binary
    public List<bool> active;


    // Rotation
    public List<Vector3> rotation;

    public InputMaster controls;

    void Update()
    {

        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        if (kb.qKey.wasPressedThisFrame)
            RewindTime();
        else if (kb.qKey.wasReleasedThisFrame)
            StopRewindTime();
    }

    void Start()
    {
        // Calculate Number of Points that have to be saved
        SavedPoints = Mathf.CeilToInt(maxRewindTime / SaveOffset);

        // Start Loop that Saves the Types every Period of Time
        StartCoroutine(Loop());

    }

    // Save every SaveOffset Seconds and call right function
    IEnumerator Loop()
    {
        yield return new WaitForSeconds(SaveOffset);

        switch (type)
        {
            case Type.Enemy:
                if (Rewinding)
                    break;
                SaveEnemy();
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
    void RewindTime()
    {
        Rewinding = true;

        switch (type)
        {
            case Type.Enemy:
                GetComponent<EnemyMovement>().RewindEnemy(position);
                break;
            case Type.Binary:
                gameObject.SendMessage("StartRewind", active);
                break;
            case Type.Rotation:
                //RewindRotation();
                break;
        }

    }

    // Stop Time of chosen enum
    void StopRewindTime()
    {
        Rewinding = false;

        switch (type)
        {
            case Type.Enemy:
                GetComponent<EnemyMovement>().StopRewindEnemy();
                break;
            case Type.Binary:
                gameObject.SendMessage("StopRewind");
                break;
            case Type.Rotation:
                //RewindRotation();
                break;
        }

    }

    // Save points
    void SaveEnemy()
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
        rotation.Insert(0, transform.eulerAngles);
        if(rotation.Count > SavedPoints)
            rotation.RemoveAt(SavedPoints);
    }

}
