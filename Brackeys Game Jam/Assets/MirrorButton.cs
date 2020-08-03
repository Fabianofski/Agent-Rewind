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
    private int index;
    private bool rewinding;

    void Start()
    {
        Rotation = Mirror.transform.rotation;
        rewind = GetComponent<Rewind>();
    }

    void OnTriggerEnter2D()
    {
        if(ClockWise)
           Rotation = Quaternion.Euler(0,0,Rotation.eulerAngles.z + 45);
        else
            Rotation = Quaternion.Euler(0, 0, Rotation.eulerAngles.z - 45);

    }

    void Update()
    {
        Mirror.transform.rotation = Quaternion.Lerp(Mirror.transform.rotation, Rotation, Speed * Time.deltaTime);
    }

    void StartRewind(List<Quaternion> rotations)
    {
        if(!rewinding)
          StartCoroutine(Replay(rotations, rewind.SaveOffset));
        rewinding = true;
    }

    public IEnumerator Replay(List<Quaternion> rotations, float time)
    {
        yield return new WaitForSeconds(time);

        StartCoroutine(Replay(rotations, rewind.SaveOffset));
    }

}
