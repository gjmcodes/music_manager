using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    bool persistBetweenScenes;

    [SerializeField]
    int minMusicRange;

    [SerializeField]
    int maxMusicRange;

    [SerializeField]
    int secondsToFadeOut;

    [SerializeField]
    bool fadeOutEnabled;

    bool loadingMusic;

    const string bgMusicPrefix= "music-bg-";
    const string backgroundMusicPath = "BackgroundMusic";
    private void Awake()
    {
        if (persistBetweenScenes)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
    void Start()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        LoadMusic();
    }

    void Update()
    {
        if (!audioSource.isPlaying && audioSource.clip != null
            && !loadingMusic)
        {
            loadingMusic = true;
            StopCoroutine(FadeOutMusic());
            LoadMusic();
        }

        if(fadeOutEnabled && audioSource.clip != null 
            && audioSource.clip.length - secondsToFadeOut <= audioSource.time)
        {
            StartCoroutine(FadeOutMusic());
        }
    }

    void LoadMusic()
    {
        var musicName = GetRandonMusic(minMusicRange, maxMusicRange);

        // has a clip loaded once?
        if(audioSource.clip != null)
        {
            while (musicName == audioSource.clip.name)
            {
                musicName = GetRandonMusic(minMusicRange, maxMusicRange);
            }
        }
       

        var pathToMusic = $"{backgroundMusicPath}/{musicName}";
        Debug.Log(pathToMusic);

        Resources.LoadAsync<AudioClip>(pathToMusic).completed += LoadMusic_completed;
    }

    static string GetRandonMusic(int minMusicRange, int maxMusicRange)
    {
        var randomMusicValue = UnityEngine.Random.Range(minMusicRange, maxMusicRange);
        var musicName = $"{bgMusicPrefix}{randomMusicValue}";

        return musicName;
    }

    void LoadMusic_completed(AsyncOperation operation)
    {
        this.audioSource.Stop();

        var resRequest = (ResourceRequest)operation;
        var audioClip = (AudioClip)resRequest.asset;
        this.audioSource.clip = audioClip;
        this.audioSource.volume = 1;

        this.audioSource.Play();

        this.loadingMusic = false;
    }

    IEnumerator FadeOutMusic()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= .05f;
            yield return new WaitForSeconds(0.3f);
        }
    }


}
