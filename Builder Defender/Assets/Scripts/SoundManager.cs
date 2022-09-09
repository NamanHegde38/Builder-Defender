using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    
    public static SoundManager Instance { get; private set; }
    
    public enum Sound {
        BuildingPlaced,
        BuildingDamaged,
        BuildingDestroyed,
        EnemyDie,
        EnemyHit,
        GameOver,
    }
    private AudioSource _audioSource;
    private Dictionary<Sound, AudioClip> _soundAudioClipDictionary;
    private float _volume = .5f;
    
    private void Awake() {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();

        _soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach (Sound sound in Enum.GetValues(typeof(Sound))) {
            _soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    public void PlaySound(Sound sound) {
        _audioSource.PlayOneShot(_soundAudioClipDictionary[sound], _volume);
    }

    public void IncreaseVolume() {
        _volume += .1f;
        _volume = Mathf.Clamp01(_volume);
    }
    
    public void DecreaseVolume() {
        _volume -= .1f;
        _volume = Mathf.Clamp01(_volume);
    }

    public float GetVolume() {
        return _volume;
    }
}
