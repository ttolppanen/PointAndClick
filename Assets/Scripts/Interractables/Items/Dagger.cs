using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    public List<Vector2Int> activationPlaces;

    public void GiveActivationCommand()
    {
        if (activationPlaces.Count == 0) //Jos voi aktivoida mistä vain
        {
            Activate();
        }
        else
        {
            Vector2 playerPos = PlayerController.instance.transform.position;
            Vector2 closestPoint = UF.ClosestPoint(playerPos, MakeActivationPlacesList());
            Vector2Int start = UF.CoordinatePosition(playerPos);
            Vector2 goal = closestPoint;
            List<Vector2> path = PlayerController.instance.map.AStarPathFinding(start, UF.CoordinatePosition(goal));
            if (path.Count != 0)
            {
                PlayerMovement.instance.GoActivate(path, goal, gameObject);
            }
        }
    }

    //Kait riittää aina vain muuttaa tätä(?)
    public void Activate()
    {
        Inventory.instance.AddItem(gameObject);
    }

    //Aktivointi paikat annetaan itemin suhteen, joten tehdään tässä niiden avulla lista koordinaateista maailman suhteen...
    List<Vector2> MakeActivationPlacesList()
    {
        List<Vector2> realPlaces = new List<Vector2>();
        foreach (Vector2Int point in activationPlaces)
        {
            realPlaces.Add((Vector2)transform.position + point);
        }
        return realPlaces;
    }
}