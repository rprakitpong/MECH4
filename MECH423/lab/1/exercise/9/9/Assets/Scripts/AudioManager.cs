using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource pigHitFence;
    AudioSource gameOver;

    // Start is called before the first frame update
    void Start()
    {
        // start background music
        AudioSource backgroundMusic = gameObject.AddComponent<AudioSource>();
        backgroundMusic.clip = Resources.Load<AudioClip>("Audio/music") as AudioClip;
        backgroundMusic.loop = true;
        backgroundMusic.volume = 0.1f;
        backgroundMusic.Play();

        // instantiate pig hit fence sound effect
        pigHitFence = gameObject.AddComponent<AudioSource>();
        pigHitFence.clip = Resources.Load<AudioClip>("Audio/bounce") as AudioClip;
        pigHitFence.volume = .5f;
        EventManager.Instance.PigHitFenceEvent += PlayPigHitFenceSound;

        // instantiate game over sound effect
        gameOver = gameObject.AddComponent<AudioSource>();
        gameOver.clip = Resources.Load<AudioClip>("Audio/babycry") as AudioClip;
        gameOver.volume = 0.3f;
        EventManager.Instance.StopEvent += PlayGameOverSound;
    }

    private void PlayGameOverSound()
    {
        gameOver.Play();
    }

    private void PlayPigHitFenceSound()
    {
        pigHitFence.Play();
    }
}
