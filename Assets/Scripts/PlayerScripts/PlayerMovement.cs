using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public float speed;
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
            while (ipath < path.Count - 1 && !UF.CheckForMapCollider(transform.position, path[ipath + 1]))
            {
                ipath++;
            }
            Vector2 nextPoint = path[ipath];
            Vector2 movementDirection = (nextPoint - (Vector2)transform.position).normalized;
            rb.velocity = movementDirection * speed;

            if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y))
            {
                if(movementDirection.x > 0)
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

    public void Move(Vector2 goal)
    {
        List<Vector2> newPath = GameManager.map.FindPath(transform.position, goal);
        if (newPath.Count != 0)
        {
            animator.SetBool("Moving", true);
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
        animator.SetBool("Moving", false);
        rb.velocity = Vector2.zero;
        shouldBeMoving = false;
    }

    public void GoActivate(Vector2 goal, GameObject target)
    {
        Move(goal);
        currentTarget = target;
    }
}
