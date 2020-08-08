using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorButton : MonoBehaviour
{

    public GameObject Mirror;
    public float Speed;
    public bool ClockWise;

    public Quaternion Rotation;

    private Rewind rewind;
    private bool rewinding;
    private bool Rotate;
    private bool Entered;

    void Start()
    {
        // Get Components
        Rotation = Mirror.transform.rotation;
        rewind = GetComponent<Rewind>();
    }

    void OnTriggerEnter2D()
    {
        if (!rewinding)
        {
            //
            // Play Button Push Sound
            FMODUnity.RuntimeManager.PlayOneShot("event:/Interactables/button", transform.position);

            Rotate = false;

            // If the Trigger wasnt already entered save true value
            // in Rewind script
            if (!Entered)
            {
                Rotate = true;
                rewind.SaveBinary(Rotate);
                // Rotate the Mirror clockwise or counterclockwise by 90 degrees;
                if (ClockWise)
                    Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z + 90);
                else
                    Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z - 90);

                //
                // Play Mirror Move Sound
                //
            }
            Entered = true;

            // Rotate the Mirror clockwise or counterclockwise by 90 degrees;
            if (ClockWise)
                Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z + 90);
            else
                Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z - 90);

            // Play Mirror Move Sound
            FMOD.Studio.EventInstance mirror = FMODUnity.RuntimeManager.CreateInstance("event:/Interactables/mirror");
            mirror.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(Mirror));
            mirror.start();
            mirror.release();
        }

    }

    void OnTriggerExit2D()
    {
        // Set Booleans
        Entered = false;
        Rotate = false;
    }

    void Update()
    {
        // Set Rotation
        Mirror.transform.rotation = Rotation;
    }

    void StartRewind(List<bool> rotated)
    {
        // Start Rewind and Coroutine to Replay saved Values;
        if (!rewinding)
        {
            StartCoroutine(Replay(rotated, rewind.SaveOffset / 2));
        }
        rewinding = true;
    }

    void StopRewind()
    {
        // Stop the Rewind and all Coroutines
        rewinding = false;
        StopAllCoroutines();
    }

    void CheckBool()
    {
        // Checks and saves Bool for Rewindscript
        if (!Rotate)
            rewind.SaveBinary(Rotate);
        else
        {
            rewind.SaveBinary(false);
        }

    }

    public IEnumerator Replay(List<bool> rotated, float time)
    {

        // Every time seconds check for true values in saved List and apply new Rotation
        yield return new WaitForSeconds(time);

        if (rewind.active[0])
        {
            // Rotate the Mirror
            if (ClockWise)
                Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z - 90);
            else
                Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z + 90);

            //
            // Play Button Push Sound

            //
            // Play Mirror Move Sound
        }
        // remove replayed Inputs
        rewind.active.RemoveAt(0);

        StartCoroutine(Replay(rotated, time));
    }

}