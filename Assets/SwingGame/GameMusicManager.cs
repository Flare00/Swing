using System;
using UnityEngine;

public class GameMusicManager : MonoBehaviour
{
    private static GameMusicManager _instance;
    public AudioClip[] tracks;
    public AudioSource source;
    private float _volume = 1.0f;

    private void Start()
    {
        if( _instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
        source = FindObjectOfType<AudioSource>();
    }

    private void Update()
    {
        if (source != null && !source.isPlaying)
        {
            source.clip = GetRandomTrack();
            source.volume = _volume;
            source.Play();
        }
    }

    private AudioClip GetRandomTrack()
    {
        return tracks[UnityEngine.Random.Range(0, tracks.Length)];
    }

    public void ChangeVolume(float volume)
    {
        _volume = volume;
        if (source != null)
        {
            this.source.volume = volume;
        }
    }
}