using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using Zenject;
public class AudioController : MonoBehaviour
{
    public AudioSource MusicSource;
    public System.Action<float> ChangeVolumeSFX;

    private float VolumeSfx;
    private float VolumeMusic;
    private StateMachine StateMachine;
    private CanvasA CanvasA;
    [Inject]
    public void Construct(StateMachine stateMachine, CanvasA canvas)
    {
        StateMachine = stateMachine;
        CanvasA = canvas;
    }
    public void Start()
    {
        LoadVolume();
        UpdateSliders();
        SetMusicVolume();
        SetNewMusic();
        SaveSounds();
    }
    public void SetMusicVolume()
    {
        VolumeMusic = CanvasA.Sliders[0].value;
        SaveSounds();
        MusicSource.volume = VolumeMusic;
    }
    public void SetActionVolume()
    {
        VolumeSfx = CanvasA.Sliders[1].value;
        SaveSounds();
        ChangeVolumeSFX?.Invoke(VolumeSfx);
    }

    private void UpdateSliders()
    {
        CanvasA.Sliders[0].value = VolumeMusic;
        CanvasA.Sliders[1].value = VolumeSfx;
    }
    private void SaveSounds()
    {
        PlayerPrefs.SetFloat("sfx", VolumeSfx);
        PlayerPrefs.SetFloat("msc", VolumeMusic);
    }
    private void LoadVolume()
    {
        if (StateMachine.FirstPlay)
        {
            VolumeSfx = 1f;
            VolumeMusic = 1f;
        }
        else
        {
            VolumeSfx = PlayerPrefs.GetFloat("sfx");
            VolumeMusic = PlayerPrefs.GetFloat("msc");
        }
    }
    private void SetNewMusic()
    {
    }
    private void Update()
    {
        if (!MusicSource.isPlaying)
            SetNewMusic();
    }
}
