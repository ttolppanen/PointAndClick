using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSong : MonoBehaviour
{
    public AudioClip song;
    private void Start()
    {
        GameManager.instance.ChangeMusic(song);
    }
}
