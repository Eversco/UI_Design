using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soundtrackctrl : MonoBehaviour
{
    public Slider slider;
    public AudioSource audioSource;
    public Toggle toggle;

    public void ControlAudio()
    {
        if(toggle.isOn)
        {
            audioSource.gameObject.SetActive(true);
        }
        else
        {
            audioSource.gameObject.SetActive(false);
        }
    }

    public void Volume()
    {
        audioSource.volume=slider.value;
    }
}
