using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimelineTest : Interractable
{
    public string sceneToLoad;
    public Vector2 spawnPoint;

    public override void Activate()
    {
        base.Activate();
        PlayerController.instance.pd.Play();
    }
}
