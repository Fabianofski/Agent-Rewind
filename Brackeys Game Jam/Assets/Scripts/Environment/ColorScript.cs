using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class ColorScript : MonoBehaviour
{
    public FMOD.Studio.EventInstance timertick;
    public FMOD.Studio.EventInstance timerend;
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
    public RectTransform ClockHand;
    public GameObject Explosion;


    // Rewind
    private bool Rewinding;

    void OnEnable()
    {
        // Set Timeleft to Max Time
        TimeLeft = Time_;

        // Get inner, outer Radius and Fontsize
        innerRadius = Indicator.pointLightInnerRadius;
        outerRadius = Indicator.pointLightOuterRadius;
        textSize = IndicatorText.fontSize;

        // Set first Color
        Indicator.color = ColourCode[SwitchIndex].color;
        StartCoroutine(Switch(TimeBtwColors));

        // Set Colors for Code displayed on UI
        for(int i = 0; i < Code.Length; i++)
        {
            Code[i].GetComponent<Image>().color = ColourCode[i].color;
        }

        //
        // Play Start Timer Sound 
        timertick = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/timer_start");
        timertick.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        timertick.start();
        FMOD.Studio.EventInstance timerend = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/timer_end");
        timerend.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));

    }

    void Update()
    {
        //
        // Play Timer Ticking Sound
        //

        // Decrease Time left
        TimeLeft -= Time.deltaTime;

        // Set Timer in UI
        float fillamount = 1 - TimeLeft / Time_;
        TimerFill.fillAmount = fillamount;
        ClockHand.rotation = Quaternion.Euler(0,0,450 - (fillamount * 360));
        // Explode when there is no Time left
        if (TimeLeft < 0)
        {
            Explosion.SetActive(true);

            //
            // Play End Timer Sound
            timertick.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            timerend.start();
            timerend.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);


            Debug.Log("Explode");

            //
            // Play Explosion Sound
            //

            if(TimeLeft < -3)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    IEnumerator Switch(float time)
    {
        yield return new WaitForSeconds(time * 2/3);

        // make Indicator invisible for 1/3 of wait time
        Indicator.pointLightInnerRadius = 0;
        Indicator.pointLightOuterRadius = 0;

        IndicatorText.fontSize = 0;

        yield return new WaitForSeconds(time * 1/3);

        // Increase or decrease Switchindex
        if (Rewinding)
            SwitchIndex--;
        else
            SwitchIndex++;

        // Set Indicator to right color and make it visible again
        Indicator.pointLightInnerRadius = innerRadius;
        Indicator.pointLightOuterRadius = outerRadius;
        Indicator.color = ColourCode[SwitchIndex].color;

        // Set Indicatortext to right text and make it visible again
        IndicatorText.fontSize = textSize;
        IndicatorText.text = SwitchIndex + 1 + ".";

        // Sounds
        if (ColourCode[SwitchIndex].colortype == ColorType.Blue)
        {
            //
            // Play Beep Sound
            FMODUnity.RuntimeManager.PlayOneShot("event:/Environment/color_code_blue", transform.position);
        }
        else if (ColourCode[SwitchIndex].colortype == ColorType.Pink)
        {
            //
            // Play Beep Sound
            FMODUnity.RuntimeManager.PlayOneShot("event:/Environment/color_code_pink", transform.position);
        }
        else if (ColourCode[SwitchIndex].colortype == ColorType.Red)
        {
            //
            // Play Beep Sound
            FMODUnity.RuntimeManager.PlayOneShot("event:/Environment/color_code_red", transform.position);
        }
        else if (ColourCode[SwitchIndex].colortype == ColorType.Yellow)
        {
            //
            // Play Beep Sound
            FMODUnity.RuntimeManager.PlayOneShot("event:/Environment/color_code_yellow", transform.position);
        }

        // Start new Coroutine or make the Player enter the Code
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
        // Activate the Objects to enter the Code with
        EnterCodeGameObject.SetActive(true);
        Indicator.gameObject.SetActive(false);
        IndicatorText.gameObject.SetActive(false);
    }

    public void EnterCode(ColorType type)
    {
        // Gets Called by EnterColorCode script

        if (CodeIndex < ColourCode.Length)
        {
            if (ColourCode[CodeIndex].colortype == type)
            {
                 // Input was true display code on UI
                 Code[CodeIndex].SetActive(true);
                 CodeIndex++;
            }
            else if(TimeLeft >= 0)
            {
                // Input was wrong Punish Player by decreasing Time
                TimeLeft -= TimePunish;
                Debug.Log("- " + TimePunish);
            }
        }

        if (CodeIndex >= ColourCode.Length)
        {
            // When there is no Color left open the door
            door.open = true;
            Debug.Log("Win");
        }
        
    }

    void StartRewind()
    {
        // Start Rewind

        if (!Rewinding)
        {
            // Start Color Switch Coroutine
            StopAllCoroutines();
            StartCoroutine(Switch(TimeBtwColors/2));

            // Disable Objects to enter code with
            EnterCodeGameObject.SetActive(false);
            Indicator.gameObject.SetActive(true);
            IndicatorText.gameObject.SetActive(true);
        }

        Rewinding = true;
    }

    void StopRewind()
    {
        // Stop Rewind
        Rewinding = false;
        StopAllCoroutines();
        StartCoroutine(Switch(TimeBtwColors));
    }

}
