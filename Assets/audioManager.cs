using UnityEngine;
using System.Collections.Generic;
public class audioManager : MonoBehaviour
{
    public static audioManager Instance;
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip jumpSFX;
    public AudioClip dashSFX;
    public AudioClip downDashSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
}
