using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    Tilemap sceneTiles;

    private void Start()
    {
        PlayerController.instance.map = this;
        Transform tileMapObject = transform.GetChild(0);
        sceneTiles = tileMapObject.GetComponent<Tilemap>();
        //tileMapObject.gameObject.SetActive(false);
    }

    float HeuresticEstimate(Vector2Int a, Vector2Int b)
    {
        Vector2 BtoA = a - b;
        return BtoA.magnitude;
    }

    //Tässä tehdään myös polusta oikein muotoinenm eli siirretään pisteet keskelle tiileä, eli polunPiste + (0.5, 0.5)...
    List<Vector2> ReconstructPath(IDictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2> totalPath = new List<Vector2>();
        totalPath.Add(current + new Vector2(0.5f, 0.5f));
        List<Vector2Int> keys = new List<Vector2Int>(cameFrom.Keys);
        while (keys.Contains(current))//alkupistettä ei koskaan tule cameFrommiinn....
        {
            current = cameFrom[current];
            totalPath.Add(current + new Vector2(0.5f, 0.5f));
        }
        totalPath.Reverse();
        return totalPath;
    }

    public List<Vector2> AStarPathFinding(Vector2Int start, Vector2Int goal)
    {
        if (sceneTiles.GetTile((Vector3Int)goal) == null)
        {
            return new List<Vector2>() {};
        }
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
                return ReconstructPath(cameFrom, current);
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

    List<Vector2Int> FindNeighbors(Vector2Int point) //Versio missä ei tarvitse antaa Rakennuksen pisteitä
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        for (int iy = -1; iy <= 1; iy++)
        {
            Vector2Int pointBeingChecked = new Vector2Int(point.x, point.y + iy);
            if (sceneTiles.GetTile((Vector3Int)pointBeingChecked) != null && iy != 0)
            {
                neighbors.Add(pointBeingChecked);
            }
        }
        for (int ix = -1; ix <= 1; ix++)
        {
            Vector2Int pointBeingChecked = new Vector2Int(point.x + ix, point.y);
            if (sceneTiles.GetTile((Vector3Int)pointBeingChecked) != null && ix != 0)
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
