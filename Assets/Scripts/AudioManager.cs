using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    AudioSource effectssource;
    public AudioClip[] effects;
    AudioSource musicsource;
    public AudioClip[] songs;
    GameObject player;

    int tracknumber; 

    private void Start()
    {
        player = GameObject.Find("Player");
        effectssource = player.GetComponent<AudioSource>();
        musicsource = GetComponent<AudioSource>();
        ChangeSong();        
    }

    private void Update()
    {
        if (!musicsource.isPlaying && !effectssource.isPlaying && !player.GetComponent<PlayerController>().lost)
        {
            NextSong();
        }       
    }

    public void PlayAudioEffect(int clip)
    {
        musicsource.Pause();
        effectssource.clip = effects[clip];
        effectssource.Play();
    }

    public void NextSong()
    {
        if (tracknumber + 1 >= songs.Length)
        {
            tracknumber = 0;
        }
        else
        {
            tracknumber += 1;
        }
        musicsource.clip = songs[tracknumber];
        musicsource.Play();
    }

    void ChangeSong()
    {
        if (!musicsource.isPlaying)
        {
            int rand = Random.Range(0, songs.Length + 1);
            if (rand <= songs.Length - 1)
            {
                tracknumber = rand; 
                musicsource.clip = songs[rand];
                musicsource.Play();
            }
            else
            {
                rand = Random.Range(0, songs.Length + 1);
                RepeatFunction("ChangeSong");
            }

        }
    }

    public void PauseMusic()
    {
        musicsource.Pause();
    }

    public void ResumeMusic()
    {
        musicsource.Play();
    }

    void RepeatFunction(string name)
    {
        Invoke(name, 0);
    }

}
