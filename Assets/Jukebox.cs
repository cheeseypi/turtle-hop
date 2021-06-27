using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    public AudioSource Music;
    void Awake()
    {
        var other = GameObject.FindGameObjectsWithTag("MusicPlayer");
        if(other.Count() > 1)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(this.gameObject);
            PlayMusic();
        }
    }

    public void PlayMusic()
    {
        if (Music.isPlaying) return;
        Music.Play();
    }

    public void PauseMusic()
    {
        Music.Pause();
    }

    public void StopMusic()
    {
        Music.Stop();
    }
}
