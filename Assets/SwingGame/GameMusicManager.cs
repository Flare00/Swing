using UnityEngine;

public class GameMusicManager : MonoBehaviour
{
    private static GameMusicManager _instance;
    private AudioSource audioSource;
    public AudioClip[] musicList;
    private int index = 0;
    private bool isStopByPlayer;

    private void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && !isStopByPlayer)
            Next();
    }

    public void Play()
    {
        if (audioSource != null)
        {
            this.audioSource.Play();
            isStopByPlayer = false;
        }
    }

    public void Pause()
    {
        if (audioSource != null)
        {
            this.audioSource.Pause();
            isStopByPlayer = true;
        }
    }

    public void Stop()
    {
        if (audioSource != null)
        {
            this.audioSource.Stop();
            isStopByPlayer = true;
        }
    }
    public void Next()
    {
        index++;
        if (index == musicList.Length)
            index = 0;
        if (audioSource != null)
        {
            this.audioSource.clip = musicList[index];
        }
        Play();
    }

    public void Previous()
    {
        index--;
        if (index < 0)
            index = musicList.Length - 1;
        if (audioSource != null)
        {
            this.audioSource.clip = musicList[index];
        }
        Play();
    }
    public void NextRandom()
    {
        if (audioSource != null)
        {
            this.index = Random.Range(0, (musicList.Length - 1));
            this.audioSource.clip = musicList[this.index];
        }
        Play();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        Pause();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        Play();
    }

    public void ChangeVolume(float volume)
    {
        if (audioSource != null)
        {
            this.audioSource.volume = volume;
        }
    }
}