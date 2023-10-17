using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum Sound
{
    GetDamage = 0,
    Defeat,
    Heal
}
[RequireComponent(typeof(AudioSource))]
public class MusicableObject : MonoBehaviour
{
    public AudioSource AudioSource;
    private AudioController AudioController;
    public AudioClip[] audioClips;
    [SerializeField] private AudioClip audioGetDamage;
    [SerializeField] private AudioClip audioDefeat;
    [SerializeField] private AudioClip audioHeal;
    [Inject]
    public void Construct(AudioController controller)
    {
        AudioController = controller;
    }
    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioController.ChangeVolumeSFX += OnChangeMusicVolume;
    }
    public void PlayOneShot(AudioClip clip)
    {
        AudioSource.PlayOneShot(clip);
    }
    public void PlayOneShot(int AudioClipId)
    {
        if (audioClips.Length > AudioClipId)
            AudioSource?.PlayOneShot(audioClips[AudioClipId]);
    }
    public void PlayOneShot(Sound sound)
    {
        switch (sound)
        {
            case Sound.GetDamage:
                if (audioGetDamage != null)
                    AudioSource.clip = audioGetDamage;
                break;
            case Sound.Defeat:
                if (audioDefeat != null)
                    AudioSource.clip = audioDefeat;
                break;
            case Sound.Heal:
                if (audioHeal != null)
                    AudioSource.clip = audioHeal;
                break;
        }
        AudioSource.Play();
    }
    private void OnChangeMusicVolume(float volume)
    {
        AudioSource.volume = volume;
    }
    private void OnDestroy()
    {
        AudioController.ChangeVolumeSFX -= OnChangeMusicVolume;
    }

}
