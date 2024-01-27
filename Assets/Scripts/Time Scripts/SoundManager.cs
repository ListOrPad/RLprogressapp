using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    public void PlaySound(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        sound.source.Play();
    }
    public void StopSound(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        sound.source.Stop();
    }

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
    }
}
