using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    EdgeCollider2D walls;
    PolygonCollider2D walkableArea;

    private void Start()
    {
        PlayerMovement.instance.map = this;
        walls = GetComponent<EdgeCollider2D>();
        walkableArea = transform.GetChild(0).gameObject.AddComponent<PolygonCollider2D>();
        walkableArea.points = walls.points;
    }

    float HeuresticEstimate(Vector2Int a, Vector2Int b)
    {
        Vector2 BtoA = a - b;
        return BtoA.magnitude;
    }

    //Tässä tehdään myös polusta oikein muotoinenm eli siirretään pisteet keskelle tiileä, eli polunPiste + (0.5, 0.5) --> Tämän jälkeen muutetaan koordinaateista oikeiksi pisteiksi!
    List<Vector2> ReconstructPath(IDictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current, Vector2 goal)
    {
        List<Vector2> totalPath = new List<Vector2>();
        totalPath.Add(current);
        List<Vector2Int> keys = new List<Vector2Int>(cameFrom.Keys);
        while (keys.Contains(current))//alkupistettä ei koskaan tule cameFrommiinn....
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        totalPath.Reverse();
        for (int i = 0; i < totalPath.Count; i++)
        {
            totalPath[i] = UF.FromCoordinatesToWorld(totalPath[i]);
        }
        totalPath.Add(goal);
        return totalPath;
    }

    bool IsInsideMap(Vector2 point)
    {
        return walkableArea.OverlapPoint(point);
    }

    public List<Vector2> FindPath(Vector2 start, Vector2 goal)
    {
        if (!IsInsideMap(goal))
        {
            return new List<Vector2>() { };
        }
        GameObject mapColliderUnderGoal = UF.FetchGameObject(goal, LayerMask.GetMask("MapColliders"));
        if (mapColliderUnderGoal != null)
        {
            Vector2 closestPointFromCollider = mapColliderUnderGoal.GetComponent<Collider2D>().bounds.ClosestPoint(start);
            goal = closestPointFromCollider + (start - closestPointFromCollider).normalized * 0.01f;
        }
        if (!IsInsideMap(UF.FromCoordinatesToWorld(UF.CoordinatePosition(start))))
        {
            start = CorrectStart(start);
        }
        List<Vector2> path = AStarPathFinding(UF.CoordinatePosition(start), UF.CoordinatePosition(goal), goal);
        return path;
    }

    Vector2 CorrectStart(Vector2 start)
    {
        Vector2[] pointsToCheck = new Vector2[4] {new Vector2(GameManager.coordinateSize.x, 0), new Vector2(-GameManager.coordinateSize.x, 0), new Vector2(0, GameManager.coordinateSize.y), new Vector2(0, -GameManager.coordinateSize.y) };
        foreach (Vector2 pointToCheck in pointsToCheck)
        {
            Vector2 point = UF.FromCoordinatesToWorld(UF.CoordinatePosition(start)) + pointToCheck;
            if (IsInsideMap(point) && !UF.CheckForMapCollider(start, point))
            {
                return point;
            }
        }
        return start;
    }

    List<Vector2> AStarPathFinding(Vector2Int start, Vector2Int goal, Vector2 realGoal)
    {
        List<Vector2Int> closedSet = new List<Vector2Int>(); //Pisteet jotka on jo tutkittu?
        List<Vector2Int> openSet = new List<Vector2Int>(); //Pisteet mitkä pitää tutkia?
        openSet.Add(start);
        IDictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>(); //Tyhjä kartta aluksi. Lopuksi jollakin avaimella saadaan piste mistä kannattee mennä siihen pisteeseen. cameFrom[jokinPiste] = lyhinPiste tästä tuohon johonkin pisteeseen(?) EHKÄ?
        IDictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>(); //Hinta alkupisteetä tähän.
        gScore[start] = 0;
        IDictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float>(); //Koko matkan hinta tänne?
        fScore[start] = HeuresticEstimate(start, goal);
        while (openSet.Count != 0)
        {
            Vector2Int current = SmallestFScoreFromOpenSet(openSet, fScore);
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current, realGoal);
            }
            else if ((current - goal).magnitude <= 2 && !UF.CheckForMapCollider(UF.FromCoordinatesToWorld(current), realGoal))
            {
                return ReconstructPath(cameFrom, current, realGoal);
            }
            openSet.Remove(current);
            closedSet.Add(current);
            List<Vector2Int> neighbors = FindNeighbors(current);
            foreach (Vector2Int neighbor in neighbors)
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }
                float tentativeGScore = gScore[current] + (current - neighbor).magnitude;
                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = tentativeGScore + HeuresticEstimate(neighbor, goal);
                if (neighbor == goal)
                {
                    break;
                }
            }
        }
        return new List<Vector2>(); //Jos ei löydy polkua niin tulee tyhjä lista...
    }

    List<Vector2Int> FindNeighbors(Vector2Int point)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        for (int iy = -1; iy <= 1; iy++)
        {
            Vector2Int pointBeingChecked = new Vector2Int(point.x, point.y + iy);
            if (!UF.CheckForMapCollider(UF.FromCoordinatesToWorld(point), UF.FromCoordinatesToWorld(pointBeingChecked)))
            {
                neighbors.Add(pointBeingChecked);
            }
        }
        for (int ix = -1; ix <= 1; ix++)
        {
            Vector2Int pointBeingChecked = new Vector2Int(point.x + ix, point.y);
            if (!UF.CheckForMapCollider(UF.FromCoordinatesToWorld(point), UF.FromCoordinatesToWorld(pointBeingChecked)))
            {
                neighbors.Add(pointBeingChecked);
            }
        }
        return neighbors;
    }

    Vector2Int SmallestFScoreFromOpenSet(List<Vector2Int> openSet, IDictionary<Vector2Int, float> fScore)
    {
        float compare = 9999;
        Vector2Int smallestPoint = openSet[0];
        foreach (Vector2Int point in openSet)
        {
            if (fScore.ContainsKey(point) && fScore[point] < compare)
            {
                smallestPoint = point;
                compare = fScore[point];
            }
        }
        return smallestPoint;
    }
}
