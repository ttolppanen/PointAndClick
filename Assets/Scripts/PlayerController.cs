using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    MapData map;
    PlayerMovement pc;

    private void Start()
    {
        if (map == null)
        {
            SetMap();
        }
        pc = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int start = UF.CoordinatePosition(transform.position);
            Vector2 goal = UF.GetMousePos();
            List<Vector2> path = map.AStarPathFinding(start, UF.CoordinatePosition(goal));
            if (path.Count != 0)
            {
                pc.Move(path, goal);
            }
        }
    }
    public void SetMap()
    {
        map = GameObject.FindWithTag("Map").GetComponent<MapData>();
    }
}
