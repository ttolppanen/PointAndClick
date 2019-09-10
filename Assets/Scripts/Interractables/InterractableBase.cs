using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterractableBase : MonoBehaviour
{
    public List<Vector2> activationPlaces;

    public void GiveActivationCommand()
    {
        if (activationPlaces.Count == 0) //Jos voi aktivoida mistä vain
        {
            Activate();
        }
        else
        {
            Vector2 playerPos = PlayerController.instance.transform.position;
            Vector2 goal = UF.ClosestPoint(playerPos, activationPlaces);
            Vector2 start = playerPos;
            List<Vector2> path = PlayerController.instance.map.FindPath(start, goal);
            if (path.Count != 0)
            {
                PlayerMovement.instance.GoActivate(path, gameObject);
            }
        }
        
    }

    public void Activate()
    {
        
    }
}
