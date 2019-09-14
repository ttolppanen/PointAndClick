using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public float speed;
    public float playerRadius;
    Rigidbody2D rb;
    Animator animator;
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
        animator = GetComponent<Animator>();
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

            if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y))
            {
                if(movementDirection.x >0)
                {
                    animator.SetInteger("Direction", 2);

                }
                else
                {
                    animator.SetInteger("Direction", 0);
                }
            }
            else
            {
                if(movementDirection.y > 0)
                {
                    animator.SetInteger("Direction", 1);
                }
                else
                {
                    animator.SetInteger("Direction", 3);
                }

            }
            
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

    public void Move(List<Vector2> newPath, Vector2 newGoal)
    {
        animator.SetBool("Moving", true);
        currentTarget = null;
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
        animator.SetBool("Moving", false);
        rb.velocity = Vector2.zero;
        shouldBeMoving = false;
    }

    public void GoActivate(List<Vector2> newPath, Vector2 newGoal, GameObject target)
    {
        Move(newPath, newGoal);
        currentTarget = target;
    }

    bool CheckForCollider(Vector2 start, Vector2 pointToCheck)
    {
        Vector2 direction = pointToCheck - start;
        RaycastHit2D hit = Physics2D.CircleCast(start, playerRadius, direction.normalized, direction.magnitude, LayerMask.GetMask("MapColliders"));
        Debug.DrawRay(start, direction);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    Vector2 CorrectGoal(Vector2 goal)
    {
        RaycastHit2D hit = Physics2D.CircleCast(goal, playerRadius, Vector2.zero, LayerMask.GetMask("MapColliders"));
        if (hit.collider == null)
        {
            return goal;
        }
        hit = Physics2D.Raycast(goal, hit.point - goal, LayerMask.GetMask("MapColliders"));
        goal += (goal - hit.point).normalized * (playerRadius - (goal - hit.point).magnitude);
        return CorrectGoal(goal);
    }
}
