using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMono : MonoBehaviour
{
    AudioSource audioSource;
    public float time;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        time = audioSource.time / audioSource.clip.length;
        if (time==1 || !audioSource.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
