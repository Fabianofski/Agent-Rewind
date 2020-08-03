using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchDestroy : MonoBehaviour
{
    private void Update()
    {
        Destroy(this.gameObject, 0.1f);
    }
}
