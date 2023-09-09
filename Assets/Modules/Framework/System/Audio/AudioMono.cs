using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioMono : MonoBehaviour
{
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (audioSource.time == audioSource.clip.length || !audioSource.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
