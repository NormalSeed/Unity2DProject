using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private SurfaceEffector2D surfaceEffector;
    public Vector2 patrolVec;

    private void Awake() => Init();
    private void Init()
    {
        rb= GetComponent<Rigidbody2D>();
    }

    public void HorizontalMove(bool isFlip, float speed)
    {
        GetPatrolVec(isFlip);

        if (surfaceEffector == null)
        {
            rb.velocity = new Vector2(patrolVec.x * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(patrolVec.x * speed + surfaceEffector.speed, rb.velocity.y);
        }
    }

    

    public Vector2 GetPatrolVec(bool isFlip)
    {

        if (isFlip)
        {
            patrolVec = Vector2.left;
        }
        else
        {
            patrolVec = Vector2.right;
        }
        return patrolVec;
    }
}
