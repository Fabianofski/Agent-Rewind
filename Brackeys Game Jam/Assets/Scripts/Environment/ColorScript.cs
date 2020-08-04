using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class ColorScript : MonoBehaviour
{
    [Header("ColorSwitch")]   
    public float TimeBtwColors;
    private int SwitchIndex;
    public Light2D Indicator;
    public TextMeshProUGUI IndicatorText;
    private float innerRadius;
    private float outerRadius;
    private float textSize;

    public enum ColorType { Blue, Red, Yellow, Pink };

    [System.Serializable]
    public class colours
    {
        public Color color;
        public ColorType colortype;
    }

    public colours[] ColourCode;

    [Header("EnterCode")]
    public GameObject EnterCodeGameObject;
    public int CodeIndex;
    public GameObject[] Code;
    public Door door;

    [Header("Timer")]
    public float Time_;
    public float TimePunish;
    private float TimeLeft;
    public Image TimerFill;

    // Rewind
    private bool Rewinding;

    void Awake()
    {
        TimeLeft = Time_;

        innerRadius = Indicator.pointLightInnerRadius;
        outerRadius = Indicator.pointLightOuterRadius;
        textSize = IndicatorText.fontSize;

        Indicator.color = ColourCode[SwitchIndex].color;
        StartCoroutine(Switch(TimeBtwColors));

        for(int i = 0; i < Code.Length; i++)
        {
            Code[i].GetComponent<Image>().color = ColourCode[i].color;
        }
    }

    void Update()
    {
        TimeLeft -= Time.deltaTime;

        float fillamount = 1 - TimeLeft / Time_;
        TimerFill.fillAmount = fillamount;

        if(TimeLeft < 0)
        {
            Debug.Log("Explode");
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
        Indicator.color = ColourCode[SwitchIndex].color;

        IndicatorText.fontSize = textSize;
        IndicatorText.text = SwitchIndex + 1 + ".";

        if (SwitchIndex < ColourCode.Length - 1 && !Rewinding)
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

    public void EnterCode(ColorType type)
    {
        if (CodeIndex < ColourCode.Length)
        {
            if (ColourCode[CodeIndex].colortype == type)
            {
                 Code[CodeIndex].SetActive(true);
                 CodeIndex++;
            }
            else
            {
                TimeLeft -= TimePunish;
                Debug.Log("- " + TimePunish);
            }
        }

        if (CodeIndex >= ColourCode.Length)
        {
            door.open = true;
            Debug.Log("Win");
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
