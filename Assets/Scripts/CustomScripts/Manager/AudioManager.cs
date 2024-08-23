using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperPlatformer
{
    public class AudioManager : SingletonGeneric<AudioManager>
    {
        [SerializeField]
        private Slider SFXSlider;
        [SerializeField]
        private Slider MusicSlider;
        [SerializeField]
        private AudioSource backgroundMusicSource;
        [SerializeField]
        private List<AudioSource> sfxSources = new List<AudioSource>();
        [SerializeField]
        private AudioSource genericAudioSource;

        private void Start()
        {
            genericAudioSource = GetComponent<AudioSource>();
            SFXSlider.onValueChanged.AddListener(SetSFXVolume);
            MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        public void SetMusicVolume(float volume)
        {
            Debug.Log("Setting music volume to " + volume);
            backgroundMusicSource.volume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            Debug.Log("Setting SFX volume to " + volume);

            for(int i = 0; i < sfxSources.Count; i++)
            {
                sfxSources[i].volume = volume;
            }
            genericAudioSource.volume = volume;
        }

        public void PlayAudio(AudioClip audioClip)
        {
            genericAudioSource.PlayOneShot(audioClip);
        }
    }
}
