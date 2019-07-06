using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float playerRadius;
    Rigidbody2D rb;
    Vector2 goal;
    List<Vector2> path;
    bool shouldBeMoving;
    int ipath;

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
            if (ipath < path.Count - 1 && !CheckForCollider(transform.position, path[ipath + 1]))
            {
                ipath++;
            }
            Vector2 nextPoint = path[ipath];
            Vector2 movementDirection = (nextPoint - (Vector2)transform.position).normalized;
            rb.velocity = movementDirection * speed;
            if (UF.DistanceBetween2Units(transform.position, goal) < 0.05f)
            {
                StopMoving();
            }
        }
    }

    public void Move(List<Vector2> newPath, Vector2 newGoal)
    {
        newGoal = CorrectGoal(newGoal);
        newPath.Add(newGoal);
        path = newPath;
        goal = newGoal;
        shouldBeMoving = true;
        ipath = 0;
        if (ipath >= path.Count)
        {
            StopMoving();
        }
    }

    void StopMoving()
    {
        rb.velocity = Vector2.zero;
        shouldBeMoving = false;
    }

    bool CheckForCollider(Vector2 start, Vector2 pointToCheck)
    {
        Vector2 direction = pointToCheck - start;
        RaycastHit2D hit = Physics2D.CircleCast(start, playerRadius, direction.normalized, direction.magnitude);
        Debug.DrawRay(start, direction);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    Vector2 CorrectGoal(Vector2 goal)
    {
        RaycastHit2D hit = Physics2D.CircleCast(goal, playerRadius, Vector2.zero);
        if (hit.collider == null)
        {
            return goal;
        }
        hit = Physics2D.Raycast(goal, hit.point - goal);
        goal += (goal - hit.point).normalized * (playerRadius - (goal - hit.point).magnitude);
        return CorrectGoal(goal);
    }
}
