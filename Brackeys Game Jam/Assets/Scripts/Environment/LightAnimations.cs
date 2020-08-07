using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightAnimations : MonoBehaviour
{

    [Header("Type")]
    public bool Flickering;
    public bool ChangeSizeOverTime;

    [Header("Parameters")]
    public float time;

    public Vector2 MaxMinOuterRadius;
    public float Multiplier;

    public float timebtwFlicker;
    public float FlickerChance;

    private float TimeLeft;
    private bool AlreadyFlickering;
    private Light2D TargetLight;
    private float Intensity;
    private bool minReached;
    private bool coroutine;


    void Awake()
    {
        TargetLight = GetComponent<Light2D>();
        TimeLeft = time;
        Intensity = TargetLight.intensity;

        if (Flickering)
            StartCoroutine(Flicker());
    }

    void Update()
    {
        TimeLeft -= Time.deltaTime;

        if (ChangeSizeOverTime)
        {
            ChangeOuterRadius();
        }

    }

    IEnumerator Flicker()
    {
        yield return new WaitForSeconds(timebtwFlicker);

        StartCoroutine(StartFlicker());

    }

    IEnumerator StartFlicker()
    {
        float random = Random.Range(0f, 1f);
        if (random < FlickerChance)
        {
            TargetLight.intensity = 0;
            yield return new WaitForSeconds(0.1f);
            TargetLight.intensity = Intensity;
            
            StartCoroutine(StartFlicker());
        }
        else
        {
            StartCoroutine(Flicker());
        }
    }

    void ChangeOuterRadius()
    {

        if (!minReached && TargetLight.pointLightOuterRadius <= MaxMinOuterRadius.x && !coroutine)
        {
            minReached = true; 
        }
        else if (minReached && TargetLight.pointLightOuterRadius >=  MaxMinOuterRadius.y && !coroutine)
        {
            StartCoroutine(ChangeBool(false));
        }

        if (!minReached && TargetLight.pointLightOuterRadius > MaxMinOuterRadius.x)
          TargetLight.pointLightOuterRadius -= Time.deltaTime * Multiplier;
        else if (minReached && TargetLight.pointLightOuterRadius < MaxMinOuterRadius.y)
          TargetLight.pointLightOuterRadius += Time.deltaTime * Multiplier;
    }

    IEnumerator ChangeBool(bool set)
    {
        coroutine = true;
        yield return new WaitForSeconds(time);
        minReached = set;
        coroutine = false;
    }

}
