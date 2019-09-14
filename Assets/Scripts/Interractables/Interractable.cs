using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interractable : MonoBehaviour
{
    public List<Vector2> activationPlaces;
    public ItemEnum key = ItemEnum.noItem;

    public virtual void GiveActivationCommand()
    {
        if (activationPlaces.Count == 0) //Jos voi aktivoida mistä vain
        {
            Activate();
        }
        else
        {
            Vector2 playerPos = PlayerMovement.instance.transform.position;
            Vector2 goal = UF.ClosestPoint(playerPos, activationPlaces);
            PlayerMovement.instance.GoActivate(goal, gameObject);
        }   
    }

    public virtual void Activate()
    {
        
    }
}
