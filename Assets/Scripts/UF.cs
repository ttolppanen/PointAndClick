﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UF
{
    public static Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public static Vector2Int GetMousePosCoordinated()
    {
        return CoordinatePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public static RaycastHit2D MouseCast()
    {
        Vector2 mousePos = GetMousePos();
        return Physics2D.Raycast(mousePos, Vector2.zero);
    }

    public static GameObject FetchGameObject(Vector2 pos, int layerMask = ~0)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0, layerMask);
        if (hit.collider == null)
        {
            return null;
        }
        else
        {
            return hit.collider.gameObject;
        }
    }

    public static Vector2Int CoordinatePosition(Vector3 pos)
    {
        return new Vector2Int(Mathf.FloorToInt(pos.x / GameManager.coordinateSize.x), Mathf.FloorToInt(pos.y / GameManager.coordinateSize.y));
    }

    public static Vector2 FromCoordinatesToWorld(Vector2 pos)
    {
        return new Vector2(pos.x * GameManager.coordinateSize.x, pos.y * GameManager.coordinateSize.y) + GameManager.coordinateSize / 2;
    }

    public static bool IsOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static Quaternion TurnUnit(Vector2 lookingDirection, float phaseShift)
    {
        float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x);
        return Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + phaseShift);
    }

    public static float DistanceBetween2Units(Vector3 posA, Vector3 posB)
    {
        return ((Vector2)(posA) - (Vector2)(posB)).magnitude;
    }

    public static Vector2 ClosestPoint(Vector2 start, List<Vector2> possibleGoals)
    {
        float shortestDistance = Mathf.Infinity;
        Vector2 bestPoint = possibleGoals[0];
        foreach (Vector2 point in possibleGoals)
        {
            float distance = (start - point).magnitude;
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                bestPoint = point;
            }
        }
        return bestPoint;
    }

    public static bool CheckForMapCollider(Vector2 start, Vector2 pointToCheck)
    {
        Vector2 direction = pointToCheck - start;
        RaycastHit2D hit = Physics2D.Raycast(start, direction.normalized, direction.magnitude, LayerMask.GetMask("MapColliders"));
        Debug.DrawRay(start, direction);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }
}