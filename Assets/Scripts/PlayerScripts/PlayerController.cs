using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public MapData map;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (map == null)
        {
            SetMap();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = UF.GetMousePos();
            GameObject somethingUnderMouse = UF.FetchGameObject(UF.GetMousePos(), LayerMask.GetMask("Interractable"));
            if (somethingUnderMouse == null) //Jos hiiren alla ei ole mitään niin liikutaan
            {
                Vector2Int start = UF.CoordinatePosition(transform.position);
                Vector2 goal = mousePosition;
                List<Vector2> path = map.AStarPathFinding(start, UF.CoordinatePosition(goal));
                if (path.Count != 0)
                {
                    PlayerMovement.instance.Move(path, goal);
                }
            }
            else
            {
                somethingUnderMouse.SendMessage("GiveActivationCommand");
            }
            
        }
    }
    public void SetMap()
    {
        map = GameObject.FindWithTag("Map").GetComponent<MapData>();
    }
}
