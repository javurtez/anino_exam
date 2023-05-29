using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip clickClip, coinClip, betClip, leverPullClip, lineClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayBet()
    {
        audioSource.PlayOneShot(betClip, 1);
    }
    public void PlayCoin()
    {
        audioSource.PlayOneShot(coinClip, 1);
    }
    public void PlayLever()
    {
        audioSource.PlayOneShot(leverPullClip, 1);
    }
    public void PlayLine()
    {
        audioSource.PlayOneShot(lineClip, 1);
    }
    public void PlayClick()
    {
        audioSource.PlayOneShot(clickClip, 1);
    }
}