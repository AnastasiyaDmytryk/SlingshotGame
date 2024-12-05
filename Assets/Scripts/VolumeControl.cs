using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private AudioMixer theMixer;
    [SerializeField] private Slider theSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start(){
        SetMusicVol();
        SetSFXVol();
    }

    public void SetMusicVol(){
        float volume = theSlider.value;
        theMixer.SetFloat("music", Mathf.Log10(volume)*20);
    }
    public void SetSFXVol(){
        float volume = sfxSlider.value;
        theMixer.SetFloat("sfx", Mathf.Log10(volume)*20);
    }

}
