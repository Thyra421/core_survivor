using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableSound : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
        Destroy(_audioSource, clip.length + 1);
    }
}