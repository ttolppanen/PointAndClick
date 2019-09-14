using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public float speed;
    public MapData map;
    Rigidbody2D rb;
    Vector2 goal;
    List<Vector2> path;
    bool shouldBeMoving;
    int ipath;
    GameObject currentTarget;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shouldBeMoving = false;
        ipath = 0;
    }

    private void Update()
    {
        if (shouldBeMoving)
        {
            if (ipath < path.Count - 1 && !UF.CheckForMapCollider(transform.position, path[ipath + 1]))
            {
                ipath++;
            }
            Vector2 nextPoint = path[ipath];
            Vector2 movementDirection = (nextPoint - (Vector2)transform.position).normalized;
            rb.velocity = movementDirection * speed;
            if (UF.DistanceBetween2Units(transform.position, goal) < 0.05f)
            {
                StopMoving();
                if (currentTarget != null)
                {
                    currentTarget.SendMessage("Activate");
                    currentTarget = null;
                }
            }
        }
    }

    public void Move(Vector2 start, Vector2 goal)
    {
        List<Vector2> newPath = map.FindPath(start, goal);
        if (newPath.Count != 0)
        {
            currentTarget = null;
            path = newPath;
            this.goal = newPath[newPath.Count - 1];
            shouldBeMoving = true;
            ipath = 0;
            if (ipath >= path.Count)
            {
                StopMoving();
            }
        }
        
    }

    void StopMoving()
    {
        rb.velocity = Vector2.zero;
        shouldBeMoving = false;
    }

    public void GoActivate(Vector2 start, Vector2 goal, GameObject target)
    {
        Move(start, goal);
        currentTarget = target;
    }
}
