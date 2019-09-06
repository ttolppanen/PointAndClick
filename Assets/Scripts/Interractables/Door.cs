﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public List<Vector2> activationPlaces;
    public string SceneToLoad;

    public void GiveActivationCommand()
    {
        if (activationPlaces.Count == 0) //Jos voi aktivoida mistä vain
        {
            Activate();
        }
        else
        {
            Vector2 playerPos = PlayerController.instance.transform.position;
            Vector2 closestPoint = UF.ClosestPoint(playerPos, activationPlaces);
            Vector2Int start = UF.CoordinatePosition(playerPos);
            Vector2 goal = closestPoint;
            List<Vector2> path = PlayerController.instance.map.AStarPathFinding(start, UF.CoordinatePosition(goal));
            if (path.Count != 0)
            {
                PlayerMovement.instance.GoActivate(path, goal, gameObject);
            }
        }
        
    }

    public void Activate()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneToLoad);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}