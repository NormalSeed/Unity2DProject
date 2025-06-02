using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DasherMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private SurfaceEffector2D surfaceEffector;
    public Vector2 patrolVec;
    public float detectRange = 5f;
    public float distance;
    public float followDir;
    public float attackDir;
    public float attackRange = 2f;

    private void Awake() => Init();
    private void Init()
    {
        rb= GetComponent<Rigidbody2D>();
        distance = detectRange;
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

    public void FollowPlayer(Transform target, float speed)
    {
        if (target != null)
        {
            distance = Vector2.Distance(transform.position, target.position);
            followDir = (target.position.x - transform.position.x) / Mathf.Abs(target.position.x - transform.position.x);
            if (distance > attackRange)
            {
                rb.velocity = new Vector2(followDir * speed, rb.velocity.y);
            }
        }
    }

    public void TerrorMovement(Transform target, float speed)
    {
        if (target != null)
        {
            float terrorDir = -(target.position.x - transform.position.x) / Mathf.Abs(target.position.x - transform.position.x);
            rb.velocity = Vector2.zero;
            rb.velocity = new Vector2(terrorDir * speed / 2, rb.velocity.y);
        }
    }
}
