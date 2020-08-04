using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class ColorScript : MonoBehaviour
{
    [Header("ColorSwitch")]
    public Color[] colors;
    public float TimeBtwColors;
    private int SwitchIndex;
    public Light2D Indicator;
    public TextMeshProUGUI IndicatorText;
    private float innerRadius;
    private float outerRadius;
    private float textSize;

    [Header("EnterCode")]
    public GameObject EnterCodeGameObject;
    public int CodeIndex;
    public GameObject[] Code;

    // Rewind
    private bool Rewinding;

    void Awake()
    {
        innerRadius = Indicator.pointLightInnerRadius;
        outerRadius = Indicator.pointLightOuterRadius;
        textSize = IndicatorText.fontSize;

        Indicator.color = colors[SwitchIndex];
        StartCoroutine(Switch(TimeBtwColors));

        for(int i = 0; i < Code.Length; i++)
        {
            Code[i].GetComponent<Image>().color = colors[i];
        }
    }

    IEnumerator Switch(float time)
    {
        yield return new WaitForSeconds(time * 2/3);

        Indicator.pointLightInnerRadius = 0;
        Indicator.pointLightOuterRadius = 0;

        IndicatorText.fontSize = 0;

        yield return new WaitForSeconds(time * 1/3);

        if (Rewinding)
            SwitchIndex--;
        else
            SwitchIndex++;

        Indicator.pointLightInnerRadius = innerRadius;
        Indicator.pointLightOuterRadius = outerRadius;
        Indicator.color = colors[SwitchIndex];

        IndicatorText.fontSize = textSize;
        IndicatorText.text = SwitchIndex + 1 + ".";

        if (SwitchIndex < colors.Length - 1 && !Rewinding)
        {
            StartCoroutine(Switch(time));
        }
        else if (SwitchIndex > 0 && Rewinding)
        {
            StartCoroutine(Switch(time));
        }
        else if(!Rewinding)
        {
            yield return new WaitForSeconds(time);
            ActivateEnterCode();
        }
    }

    void ActivateEnterCode()
    {
        EnterCodeGameObject.SetActive(true);
        Indicator.gameObject.SetActive(false);
        IndicatorText.gameObject.SetActive(false);
    }

    public void EnterCode(Color color)
    {
        if(colors[SwitchIndex] == color)
        {
            Code[SwitchIndex].SetActive(true);
        }
        else
        {
            Debug.Log("Verkackt");
        }
    }

    void StartRewind()
    {
        if (!Rewinding)
        {
            StopAllCoroutines();
            StartCoroutine(Switch(TimeBtwColors/2));

            EnterCodeGameObject.SetActive(false);
            Indicator.gameObject.SetActive(true);
            IndicatorText.gameObject.SetActive(true);
        }

        Rewinding = true;
    }

    void StopRewind()
    {
        Rewinding = false;
        StopAllCoroutines();
        StartCoroutine(Switch(TimeBtwColors));
    }

}
