using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interractable
{
    public string sceneToLoad;
    public Vector2 spawnPoint;

    public override void Activate()
    {
        base.Activate();
        SceneManager.LoadScene(sceneToLoad);
        PlayerMovement player = PlayerMovement.instance;
        player.transform.position = spawnPoint;
    }
}
