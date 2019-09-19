using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Vector2 coordinateSize = new Vector2(0.5f, 0.5f);
    public static MapData map;

    public static float musicVolume = 0.05f;
    public static float effectVolume = 0.05f;
    AudioSource musicPlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.volume = musicVolume;
    }

    public void ChangeMusic(AudioClip musicClip)
    {
        StartCoroutine(StopSong(musicPlayer));
        musicPlayer = gameObject.AddComponent<AudioSource>();
        musicPlayer.volume = 0;
        musicPlayer.clip = musicClip;
        musicPlayer.loop = true;
        musicPlayer.Play();
        StartCoroutine(StartSong(musicPlayer));
    }

    IEnumerator StopSong(AudioSource player)
    {
        while (player.volume > 0.01f)
        {
            player.volume -= musicVolume / 100f;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(player);
    }

     IEnumerator StartSong(AudioSource player)
    {
        while (player.volume < musicVolume)
        {
            player.volume += musicVolume / 100f;
            yield return new WaitForSeconds(0.01f);
        }
        player.volume = musicVolume;
    }
}
