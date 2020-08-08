using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorScriptActivator : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
               GetComponentInParent<ColorScript>().enabled = true;
    }

}
